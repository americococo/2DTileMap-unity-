using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    private void Awake()
    {
        _attackPoint = 5;
        _ObjectType = eMapObjectType.CHARACTER;
    }

    void Start()
    {
        
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
