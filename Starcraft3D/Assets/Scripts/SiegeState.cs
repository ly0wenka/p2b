public class SiegeState : IUnitState
{
    public SiegeState()
    {
        CanMove = false;
        Damage = 20;
    }

    public bool CanMove { get; set; }
    public int Damage { get; set; }
}