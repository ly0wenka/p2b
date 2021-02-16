using UnityEngine;

public struct InstantiatedGameObjectState
{
    public GameObject gameObject;
    public MrFusion mrFusion;
    public long creationFrame;
    public long? destructionFrame;
    public TransformState transformState;
}