using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    private void Awake()
    {
        
    }

    void Start()
    {
        _attackPoint = 10;
        _hp = _fullHp;
        _ObjectType = eMapObjectType.CHARACTER;
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
