public struct BasicResponse
{
    public bool success
    {
        get { return this._success; }
    }

    private bool _success;

    public BasicResponse(bool success)
    {
        this._success = success;
    }
}