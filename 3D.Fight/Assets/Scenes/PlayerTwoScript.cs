using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatePosition
{
    Ground,
    Fly,
    Sit
}

public interface IState 
{
    void DoFly();
    void DoGround();
    void DoSit();
}

public abstract class State : IState
{
    protected PlayerBehaviour player;

    protected State(PlayerBehaviour player)
    {
        this.player = player;
    }

    public abstract void DoFly();
    public abstract void DoGround();
    public abstract void DoSit();
}

public class GroundState : State
{
    public GroundState(PlayerBehaviour player) : base(player)
    {
    }

    public override void DoFly()
    {
        player.ChangeState(new FlyState(player));
    }

    public override void DoGround()
    {
        
    }

    public override void DoSit()
    {
        player.ChangeState(new FlyState(player));
    }
}

public class FlyState : State
{
    public FlyState(PlayerBehaviour player) : base(player)
    {
    }

    public override void DoFly()
    {
        throw new System.NotImplementedException();
    }

    public override void DoGround()
    {
        throw new System.NotImplementedException();
    }

    public override void DoSit()
    {
        throw new System.NotImplementedException();
    }
}

public class PlayerBehaviour
{
    private IState state;
    public void ChangeState(IState state)
    {
        this.state = state;
    }
}

public class PlayerTwoScript : MonoBehaviour
{
    public StatePosition currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentPosition == StatePosition.Ground)
            {
                {
                    transform.Translate(new Vector3(0, 1, 0));
                }
                currentPosition = StatePosition.Fly;
            }
            if (currentPosition == StatePosition.Sit)
            {
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.Translate(new Vector3(0, .25f, 0));
                }
                currentPosition = StatePosition.Ground;
            }
        }
        //transform.Translate(new Vector3(0, 1, 0));
        //transform.localScale = new Vector3(1, 2, 1);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            transform.Translate(new Vector3(-1, 0, 0));
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //transform.Translate(new Vector3(0, -1, 0));
            if (currentPosition == StatePosition.Ground)
            {
                {
                    transform.localScale = new Vector3(1, .5f, 1);
                    transform.Translate(new Vector3(0, -.25f, 0));
                }
                currentPosition = StatePosition.Sit;
            }
            if (currentPosition == StatePosition.Fly)
            {
                {
                    transform.Translate(new Vector3(0, -1, 0));
                }
                currentPosition = StatePosition.Ground;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
            transform.Translate(new Vector3(1, 0, 0));
    }
}
