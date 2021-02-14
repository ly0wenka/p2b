using FPLibrary;

[System.Serializable]
public class ComboOptions
{
    public ComboDisplayMode comboDisplayMode;
    public Sizes hitStunDeterioration;
    public Sizes damageDeterioration;
    public Sizes airJuggleDeterioration;
    public float minHitStun = 1;
    public int _minHitStun;
    public float minDamage = 5;
    public Fix64 _minDamage;
    public float minPushForce = 5;
    public Fix64 _minPushForce;
    public int maxConsecutiveCrumple = 1;
    public AirJuggleDeteriorationType airJuggleDeteriorationType;
    public bool neverAirRecover = false;
    public AirRecoveryType airRecoveryType = AirRecoveryType.CantMove;
    public bool resetFallingForceOnHit = true;
    public int maxCombo = 99;
    public float knockBackMinForce = 0;
    public Fix64 _knockBackMinForce;
    public bool neverCornerPush;
    public bool fixJuggleWeight = true;
    public float juggleWeight = 200;
    public Fix64 _juggleWeight;
}