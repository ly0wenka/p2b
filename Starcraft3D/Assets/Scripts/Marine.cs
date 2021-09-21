public class Marine : IUnit, ILightUnit, IMarine
{
    public int Health { get; set; } = 100;

    public IUnitState State { get; set; }
    public bool CanMove { get; }
    public int Damage { get; set; }
    public int Armor { get; set; }

    public Marine()
    {
    }

    public Marine(int damage, int armor)
    {
        Damage = damage;
        Armor = armor;
    }

    public void Attack(Target target)
    {
        target.Health -= 6;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitLight(this);
    }
}