using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    void Start()
    {
        _ObjectType = eMapObjectType.MONSTER;
    }


    protected override void InitState()
    {
        {
            State state = new MonsterIdle();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new Move();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new Attack();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }
        {
            State state = new Damage();
            state.Init(this);
            _stateMap[eStateType.DAMAGE] = state;
        }
        {
            State state = new Death();
            state.Init(this);
            _stateMap[eStateType.DEATH] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}
