public interface IMarine
{
    int Damage { get; set; }
    int Armor { get; set; }
}

public class MarineArmorUpgrade : IMarine
{
    public MarineArmorUpgrade(IMarine marine)
    {
        Damage = marine.Damage;
        Armor = marine.Armor;
        Armor++;
    }

    public int Damage { get; set; }

    public int Armor { get; set; }
}