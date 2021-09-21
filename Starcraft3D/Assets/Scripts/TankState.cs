public class TankState : IUnitState
{
    public TankState()
    {
        CanMove = true;
        Damage = 5;
    }

    public bool CanMove { get; set; }
    public int Damage { get; set; }
}