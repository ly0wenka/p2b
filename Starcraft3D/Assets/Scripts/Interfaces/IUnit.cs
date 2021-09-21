public interface IUnit
{
    IUnitState State { get; set; }
    bool CanMove { get; }
    int Damage { get; }
    void Attack(Target target);
}