using System.Collections.Generic;
using System.Collections.ObjectModel;

public class RandomMatchScreen : CombatScreen {
	#region public instance fields
	public int pageSize = 20;
	#endregion

	#region protected instance field
	protected bool _connecting = false;
	protected int _currentPage = 0;
	protected IList<MultiplayerAPI.MatchInformation> _foundMatches = new List<MultiplayerAPI.MatchInformation>();
	#endregion

	#region public override methods
	public override void OnShow(){
		base.OnShow ();

		this.StopSearchingMatchGames();

        MainScript.multiplayerMode = MainScript.MultiplayerMode.Online;
        this._currentPage = 0;
        this.JoinOrCreateMatchGame();
	}
	#endregion

	#region public instance methods
	public virtual void GoToMainMenuScreen(){
		this.StopSearchingMatchGames();
		MainScript.StartMainMenuScreen();
	}

	public virtual void GoToConnectionLostScreen(){
		this.StopSearchingMatchGames();
		MainScript.StartConnectionLostScreen();
	}

	public virtual void JoinOrCreateMatchGame()
    {
        this._connecting = true;
        MainScript.multiplayerAPI.OnMatchCreated -= this.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchCreationError -= this.OnMatchCreationError;

		MainScript.multiplayerAPI.OnMatchesDiscovered += this.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError += this.OnMatchDiscoveryError;
        
        MainScript.multiplayerAPI.StartSearchingMatches(this._currentPage, this.pageSize, null);
    }

	public virtual void StopSearchingMatchGames(){
		MainScript.multiplayerAPI.OnMatchesDiscovered -= this.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= this.OnMatchDiscoveryError;

		this._foundMatches.Clear();
		this._connecting = false;
	}
	#endregion

	#region protected instance methods
	protected virtual void OnMatchCreated(MultiplayerAPI.CreatedMatchInformation match){
		MainScript.multiplayerAPI.OnMatchCreated -= this.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchCreationError -= this.OnMatchCreationError;

		this.StopSearchingMatchGames();
	}

	protected virtual void OnMatchCreationError(){
		MainScript.multiplayerAPI.OnMatchCreated -= this.OnMatchCreated;
		MainScript.multiplayerAPI.OnMatchCreationError -= this.OnMatchCreationError;

		this.GoToConnectionLostScreen();
	}

	protected virtual void OnMatchesDiscovered(ReadOnlyCollection<MultiplayerAPI.MatchInformation> matches) {
		MainScript.multiplayerAPI.OnMatchesDiscovered -= this.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= this.OnMatchDiscoveryError;

		if (matches != null){
			for (int i = 0; i < matches.Count; ++i){
				if (matches[i] != null){
					this._foundMatches.Add(matches[i]);
				}
			}
		}

		this.TryConnect();
	}

	protected virtual void OnMatchDiscoveryError() {
		MainScript.multiplayerAPI.OnMatchesDiscovered -= this.OnMatchesDiscovered;
		MainScript.multiplayerAPI.OnMatchDiscoveryError -= this.OnMatchDiscoveryError;

		this.GoToConnectionLostScreen();
	}

	protected virtual void OnJoined(MultiplayerAPI.JoinedMatchInformation match){
		MainScript.multiplayerAPI.OnJoined -= this.OnJoined;
		MainScript.multiplayerAPI.OnJoinError -= this.OnJoinError;

		MainScript.multiplayerMode = MainScript.MultiplayerMode.Online;
		this.StopSearchingMatchGames();
	}

	protected virtual void OnJoinError(){
		MainScript.multiplayerAPI.OnJoined -= this.OnJoined;
		MainScript.multiplayerAPI.OnJoinError -= this.OnJoinError;

		// Try to connect to other found matches
		this._connecting = false;
		this.TryConnect();
	}

	protected virtual void OnLanGameNotFound(){
		this.GoToConnectionLostScreen();
	}

	protected virtual void TryConnect(){
		// First, we check that we aren't already connected to a client or a server...
		if (!MainScript.multiplayerAPI.IsConnected() && !this._connecting){
			MultiplayerAPI.MatchInformation match = null;

			// After that, check if we have found one match with at least one player which isn't already full...
			while(
				this._foundMatches.Count > 0 && 
				(match == null || match.currentPlayers == 0 || match.currentPlayers >= match.maxPlayers)
			){
				match = this._foundMatches[0];
				this._foundMatches.RemoveAt(0);

				if (match != null && match.currentPlayers > 0 && match.currentPlayers < match.maxPlayers){
					// In that case, try connecting to that match
					this._connecting = true;

					MainScript.multiplayerAPI.OnJoined += this.OnJoined;
					MainScript.multiplayerAPI.OnJoinError += this.OnJoinError;
					MainScript.multiplayerAPI.JoinMatch(match);

					return;
				}
			}

			// Otherwise, create a new match
			this._connecting = true;
			MainScript.multiplayerAPI.OnMatchCreated += this.OnMatchCreated;
			MainScript.multiplayerAPI.OnMatchCreationError += this.OnMatchCreationError;
			MainScript.multiplayerAPI.CreateMatch(new MultiplayerAPI.MatchCreationRequest());

		}
	}
	#endregion
}
