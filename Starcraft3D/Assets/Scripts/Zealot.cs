public class Zealot : IUnit
{
    public IUnitState State { get; set; }
    public bool CanMove { get; }
    public int Damage { get; }

    public void Attack(Target target)
    {
        target.Health -= 8;
    }
}