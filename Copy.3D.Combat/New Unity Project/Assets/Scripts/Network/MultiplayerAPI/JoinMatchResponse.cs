using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public struct JoinMatchResponse{
    public string accessTokenString{
        get{
            return this._accessTokenString;
        }
    }

    public NetworkID networkId{
        get{
            return this._matchInfo.networkId;
        }
    }

    public NodeID nodeId{
        get{
            return this._matchInfo.nodeId;
        }
    }

    public bool success{
        get {
            return this._success;
        }
    }


    private string _accessTokenString;
    private MatchInfo _matchInfo;
    private bool _success;

    public JoinMatchResponse(bool success, string accessTokenString, MatchInfo matchInfo){
        this._success = success;
        this._accessTokenString = accessTokenString;
        this._matchInfo = matchInfo;
    }
}