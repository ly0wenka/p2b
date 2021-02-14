using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class SearchMatchScreen : CombatScreen {
	#region public instance fields
	public int pageSize = 20;
	public int searchDelay = 180;
	public int maxSearchTimes = 3;
	public MainScript.MultiplayerMode multiplayerMode;
	#endregion

	#region protected instance field
	protected bool _connecting = false;
	protected int _currentPage = 0;
	protected int _currentSearchTime = 0;
	protected IList<MultiplayerAPI.MatchInformation> _foundMatches = new List<MultiplayerAPI.MatchInformation>();
    protected IList<MultiplayerAPI.MatchInformation> _triedMatches = new List<MultiplayerAPI.MatchInformation>();
    protected MultiplayerAPI.MatchInformation current_match = null;
    #endregion

    #region public override methods
    public override void OnShow(){
		base.OnShow ();

		//this.StopSearchingMatchGames();

        MainScript.multiplayerMode = multiplayerMode;
        this._currentPage = 0;
        this._currentSearchTime = 0;
        this.StartSearchingGames();
	}
	#endregion

	#region public instance methods
	public virtual void GoToMainMenuScreen(){
		this.StopSearchingMatchGames();
        //MainScript.EnsureNetworkDisconnection();
        MainScript.StartMainMenuScreen();
	}

	public virtual void GoToConnectionLostScreen(){
		this.StopSearchingMatchGames();
        //MainScript.EnsureNetworkDisconnection();
        MainScript.StartConnectionLostScreen();
    }

	public virtual void StartSearchingGames()
    {
        MainScript.multiplayerAPI.OnMatchesDiscovered += this.OnMatchesDiscovered;
        MainScript.multiplayerAPI.OnMatchDiscoveryError += this.OnMatchDiscoveryError;
        
        MainScript.multiplayerAPI.StartSearchingMatches(this._currentPage, this.pageSize, null);
    }

	public virtual void StopSearchingMatchGames(bool enforce = true)
    {
        this._connecting = false;
        MainScript.multiplayerAPI.OnMatchesDiscovered -= this.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= this.OnMatchDiscoveryError;

        if (enforce) MainScript.FindAndRemoveDelayLocalAction(StartSearchingGames);
        MainScript.multiplayerAPI.StopSearchingMatches();
        this._foundMatches.Clear();
	}
    #endregion

    #region protected instance methods
    protected virtual void OnMatchCreated(MultiplayerAPI.CreatedMatchInformation match){
		MainScript.multiplayerAPI.OnMatchCreated -= this.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchCreationError -= this.OnMatchCreationError;
        MainScript.multiplayerAPI.OnPlayerConnectedToMatch += this.OnPlayerConnectedToMatch;

        this.StopSearchingMatchGames();
        this.current_match = new MultiplayerAPI.MatchInformation(match);
        this._triedMatches.Add(this.current_match);
        
		if (MainScript.config.networkOptions.networkService == NetworkService.Unity){
			if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Created: "+ match.unityNetworkId);
		}else{
			if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Created: "+ match.matchName);
		}
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("Waiting for opponent...");
    }

	protected virtual void OnMatchCreationError(){
		MainScript.multiplayerAPI.OnMatchCreated -= this.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchCreationError -= this.OnMatchCreationError;

		//this.GoToConnectionLostScreen();
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("OnMatchCreationError");
    }

    protected virtual void OnMatchesDiscovered(ReadOnlyCollection<MultiplayerAPI.MatchInformation> matches)
    {
        int unique = 0;
        if (matches != null)
        {
            for (int i = 0; i < matches.Count; ++i)
            {
                if (matches[i] != null)
                {
                    bool duplicate = false;
                    for (int f = 0; f < _foundMatches.Count; f++)
                    {
                        if (_foundMatches[f].unityNetworkId == matches[i].unityNetworkId)
                            duplicate = true;
                    }
                    for (int t = 0; t < _triedMatches.Count; t++)
                    {
                        if (_triedMatches[t].unityNetworkId == matches[i].unityNetworkId)
                            duplicate = true;
                    }

                    if (MainScript.config.networkOptions.networkService == NetworkService.Photon)
                    {
                        duplicate = false;
                    }


                    if (duplicate)
                    {
                        if (MainScript.config.networkOptions.networkService == NetworkService.Unity)
                        {
                            if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Found: " + matches[i].unityNetworkId + " [duplicate]");
                        }
                        else
                        {
                            if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Found: " + matches[i].matchName + " [duplicate]");
                        }
                    }
                    else
                    {
                        if (MainScript.config.networkOptions.networkService == NetworkService.Unity)
                        {
                            if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Found: " + matches[i].unityNetworkId);
                        }
                        else
                        {
                            if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Found: " + matches[i].matchName);
                        }

                        this._foundMatches.Add(matches[i]);
                        unique++;
                    }
                }
            }
            if (MainScript.config.debugOptions.connectionLog) Debug.Log("Matches Found (available/total): " + unique + "/" + matches.Count);
        }

        if (unique > 0 || _currentSearchTime >= maxSearchTimes)
        {
            this.TryConnect();
        }
        else
        {
            MainScript.DelayLocalAction(StartSearchingGames, searchDelay);
            _currentSearchTime++;
        }
        this.StopSearchingMatchGames(false);
    }

	protected virtual void OnMatchDiscoveryError() {
		MainScript.multiplayerAPI.OnMatchesDiscovered -= this.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= this.OnMatchDiscoveryError;

		this.GoToConnectionLostScreen();
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("OnMatchDiscoveryError");
    }

	protected virtual void OnJoined(MultiplayerAPI.JoinedMatchInformation match){
		MainScript.multiplayerAPI.OnJoined -= this.OnJoined;
		MainScript.multiplayerAPI.OnJoinError -= this.OnJoinError;

		MainScript.multiplayerMode = MainScript.MultiplayerMode.Online;
		this.StopSearchingMatchGames();

        if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Starting...");
        MainScript.StartNetworkGame(0.5f, 2, false);
    }

	protected virtual void OnJoinError(){
		MainScript.multiplayerAPI.OnJoined -= this.OnJoined;
		MainScript.multiplayerAPI.OnJoinError -= this.OnJoinError;

		// Try to connect to other found matches
		this._connecting = false;
		this.TryConnect();
	}
    
    protected virtual void OnPlayerConnectedToMatch(MultiplayerAPI.PlayerInformation player)
    {
        MainScript.multiplayerAPI.OnPlayerConnectedToMatch -= this.OnPlayerConnectedToMatch;
        MainScript.multiplayerAPI.OnPlayerDisconnectedFromMatch += this.OnPlayerDisconnectedFromMatch;

        if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Starting...");
        MainScript.StartNetworkGame(0.5f, 1, false);
    }

    protected virtual void OnPlayerDisconnectedFromMatch(MultiplayerAPI.PlayerInformation player)
    {
        MainScript.multiplayerAPI.OnPlayerDisconnectedFromMatch -= this.OnPlayerDisconnectedFromMatch;

        this.GoToConnectionLostScreen();
        if (MainScript.config.debugOptions.connectionLog) Debug.Log("OnPlayerDisconnectedFromMatch");
    }

    protected virtual void TryConnect(){
		// First, we check that we aren't already connected to a client or a server...
		if (!MainScript.multiplayerAPI.IsConnected() && !this._connecting){

            if (MainScript.config.debugOptions.connectionLog) Debug.Log("Connecting...");
            MultiplayerAPI.MatchInformation match = null;

			// After that, check if we have found one match with at least one player which isn't already full...
			while(this._foundMatches.Count > 0){
				match = this._foundMatches[0];
				this._foundMatches.RemoveAt(0);
                this._triedMatches.Add(match);

                if (match != null && match.currentPlayers > 0 && match.currentPlayers < match.maxPlayers){
					// In that case, try connecting to that match
					this._connecting = true;
                    
					MainScript.multiplayerAPI.OnJoined += this.OnJoined;
					MainScript.multiplayerAPI.OnJoinError += this.OnJoinError;
                    if (MainScript.config.debugOptions.connectionLog) Debug.Log("Match Found! Joining Match...");
                    MainScript.multiplayerAPI.JoinMatch(match);

					return;
				}
			}

			// Otherwise, create a new match
			this._connecting = true;
            MainScript.multiplayerAPI.OnMatchCreated += this.OnMatchCreated;
			MainScript.multiplayerAPI.OnMatchCreationError += this.OnMatchCreationError;
			MainScript.multiplayerAPI.CreateMatch(new MultiplayerAPI.MatchCreationRequest());

            if (MainScript.config.debugOptions.connectionLog) Debug.Log("No Matches Found. Creating Match...");

        }
	}
	#endregion
}
