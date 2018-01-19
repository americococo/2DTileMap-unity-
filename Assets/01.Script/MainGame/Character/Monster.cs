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
        base.InitState();
        {
            State state = new NPCIdle();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }
}
