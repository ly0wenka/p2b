public class MarioAdapter : IUnit
{
    private Mario _mario;
    public MarioAdapter(Mario mario)
    {
        this._mario = mario;
    }

    public IUnitState State { get; set; }
    public bool CanMove { get; }
    public int Damage { get; }

    public void Attack(Target target)
    {
        target.Health -= _mario.jumpAttack();
    }
}