public class Marauder : IArmoredUnit
{
    public int Health { get; set; } = 125;

    public void Accept(IVisitor visitor)
    {
        visitor.VisitArmored(this);
    }
}