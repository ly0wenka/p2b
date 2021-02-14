using UnityEngine.Networking.Types;

public struct CreateMatchResponse
{
    public NetworkAccessToken accessTokenString
    {
        get { return this._accessTokenString; }
    }

    public NetworkID networkId
    {
        get { return this._networkId; }
    }

    public NodeID nodeId
    {
        get { return this._nodeId; }
    }

    public bool success
    {
        get { return this._success; }
    }

    private NetworkAccessToken _accessTokenString;
    private NetworkID _networkId;
    private NodeID _nodeId;
    private bool _success;

    public CreateMatchResponse(bool success, NetworkID networkId, NodeID nodeId, NetworkAccessToken accessTokenString)
    {
        this._success = success;
        this._networkId = networkId;
        this._nodeId = nodeId;
        this._accessTokenString = accessTokenString;
    }
}