using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStateType
{
    IDLE,
    MOVE,
    NONE,
}

public class State
{
    virtual public eMoveDirection Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
            return eMoveDirection.NONE;
        }

        return eMoveDirection.NONE;
    }

    public void Init(Character character)
    {
        _character = character;
    }

    virtual public void Start()
    {
        _nextState = eStateType.NONE;
    }

    virtual public void Stop()
    {

    }
    protected eStateType _nextState = eStateType.NONE;
    protected Character _character;
}
