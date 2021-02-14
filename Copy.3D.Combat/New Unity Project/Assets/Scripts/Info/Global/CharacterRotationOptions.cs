using FPLibrary;

[System.Serializable]
public class CharacterRotationOptions {
    public bool autoMirror = true;
    public bool rotateWhileJumping = false;
    public bool rotateOnMoveOnly = false;
    public bool fixRotationWhenStunned = false;
    public bool fixRotationWhenBlocking = true;
    public bool fixRotationOnHit = true;
    public float rotationSpeed = 10;
    public Fix64 _rotationSpeed;
    public float mirrorBlending = .1f;
    public Fix64 _mirrorBlending;
}