using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    override public eMoveDirection Update()
    {
        if (eStateType.NONE != _nextState)
        {
            _character.ChangeState(_nextState);
            return eMoveDirection.NONE;
        }

        eMoveDirection moveDirection = eMoveDirection.NONE;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = eMoveDirection.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = eMoveDirection.RIGHT;
        }   
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = eMoveDirection.UP;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
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
}