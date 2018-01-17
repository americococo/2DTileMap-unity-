using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    override public eMoveDirection Update()
    {

        if(_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
            return eMoveDirection.NONE;
        }

        eMoveDirection moveDirection = eMoveDirection.NONE;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = eMoveDirection.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = eMoveDirection.RIGHT;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = eMoveDirection.UP;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = eMoveDirection.DOWN;
        }



        if (eMoveDirection.NONE != moveDirection)
        {
            _character.SetNextDirection(moveDirection);
            _nextState = eStateType.MOVE;
        }


        return moveDirection;
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Stop()
    {
        base.Stop();
    }
}
