public interface IArmoredUnit
{
    int Health { get; set; }

    void Accept(IVisitor visitor);
}