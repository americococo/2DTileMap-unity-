using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStateType
{
    IDLE,
    MOVE,
    ATTACK,
    DAMAGE,
    DEATH,
    NONE,
    PATHFINDING,
    BATTLE
}

public class State
{
    virtual public void Update()
    {

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
    public void NextState(eStateType nextState)
    {
        _nextState = nextState;
    }

    public eStateType GetNextState()
    {
        return _nextState;
    }
        

}
