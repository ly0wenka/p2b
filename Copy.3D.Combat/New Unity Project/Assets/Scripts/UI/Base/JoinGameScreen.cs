using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.UI;

public class JoinGameScreen : CombatScreen
{
    #region protected instance fields

    protected bool _connecting = false;
    protected IList<MultiplayerAPI.MatchInformation> _foundServers = new List<MultiplayerAPI.MatchInformation>();

    #endregion

    #region public override methods

    public override void OnShow()
    {
        base.OnShow();
        this.StopSearchingLanGames();
    }

    #endregion

    #region public instance methods

    public virtual void GoToNetworkGameScreen()
    {
        this.StopSearchingLanGames();
        MainScript.StartNetworkGameScreen();
    }

    public virtual void GoToConnectionLostScreen()
    {
        this.StopSearchingLanGames();
        MainScript.StartConnectionLostScreen();
    }

    public virtual void RefreshGameList()
    {
    }

    public virtual void JoinGame(Text textUI)
    {
        this.StopSearchingLanGames();
        MainScript.JoinGame(new MultiplayerAPI.MatchInformation(textUI.text, MainScript.config.networkOptions.port));
    }

    public virtual void JoinFirstLanGame()
    {
        MainScript.multiplayerAPI.OnMatchesDiscovered -= this.OnMatchesDiscovered;
        MainScript.multiplayerAPI.OnMatchDiscoveryError -= this.OnMatchDiscoveryError;

        MainScript.multiplayerAPI.OnMatchesDiscovered += this.OnMatchesDiscovered;
        MainScript.multiplayerAPI.OnMatchDiscoveryError += this.OnMatchDiscoveryError;

        MainScript.multiplayerAPI.StartSearchingMatches();
    }

    public virtual void StopSearchingLanGames()
    {
        MainScript.multiplayerAPI.OnMatchesDiscovered -= this.OnMatchesDiscovered;
        MainScript.multiplayerAPI.OnMatchDiscoveryError -= this.OnMatchDiscoveryError;

        MainScript.multiplayerAPI.StopSearchingMatches();
        this._foundServers.Clear();
        this._connecting = false;
    }

    #endregion

    #region protected instance methods

    protected virtual void OnJoined(MultiplayerAPI.JoinedMatchInformation match)
    {
        MainScript.multiplayerAPI.OnJoined -= this.OnJoined;
        MainScript.multiplayerAPI.OnJoinError -= this.OnJoinError;
    }

    protected virtual void OnJoinError()
    {
        MainScript.multiplayerAPI.OnJoined -= this.OnJoined;
        MainScript.multiplayerAPI.OnJoinError -= this.OnJoinError;

        // Try to connect to other found matches
        this._connecting = false;
        this.TryConnect();
    }

    protected virtual void OnMatchesDiscovered(ReadOnlyCollection<MultiplayerAPI.MatchInformation> matches)
    {
        this.StopSearchingLanGames();

        if (matches != null && matches.Count > 0)
        {
            for (int i = 0; i < matches.Count; ++i)
            {
                if (matches[i] != null)
                {
                    this._foundServers.Add(matches[i]);
                }
            }

            this.TryConnect();
        }
        else
        {
            this.GoToConnectionLostScreen();
        }
    }

    protected virtual void OnMatchDiscoveryError()
    {
        this.StopSearchingLanGames();
        this.GoToConnectionLostScreen();
    }

    protected virtual void OnLanGameNotFound()
    {
        this.GoToConnectionLostScreen();
    }

    protected virtual void TryConnect()
    {
        // First, we check that we aren't already connected to a client or a server...
        if (!MainScript.multiplayerAPI.IsConnected() && !this._connecting)
        {
            MultiplayerAPI.MatchInformation match = null;

            // After that, check if we have found one match with at least one player which isn't already full...
            while (match == null && this._foundServers.Count > 0)
            {
                match = this._foundServers[0];
                this._foundServers.RemoveAt(0);
            }


            if (match != null)
            {
                // In that case, try connecting to that match
                this._connecting = true;

                MainScript.multiplayerAPI.OnJoined += this.OnJoined;
                MainScript.multiplayerAPI.OnJoinError += this.OnJoinError;
                MainScript.JoinGame(match);
            }
            else
            {
                // Otherwise, return a net a new match
                this.OnLanGameNotFound();
            }
        }
    }

    #endregion
}