[System.Serializable]
public class DebugOptions {
    public bool startGameImmediately;
    public bool skipLoadingScreen;
    public MatchType matchType;
    public bool debugMode;
    public bool emulateNetwork;
    public bool trainingModeDebugger;
    public bool preloadedObjects;

    public bool networkToggle;
    public bool connectionLog = true;
    public bool ping = true;
    public bool frameDelay = true;
    public bool currentLocalFrame = true;
    public bool currentNetworkFrame = true;
    public bool desyncErrorLog = false;
    public bool stateTrackerTest = false;

    public CharacterDebugInfo p1DebugInfo;
    public CharacterDebugInfo p2DebugInfo;
}