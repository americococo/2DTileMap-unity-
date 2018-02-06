using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
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
            {
                State state = new PathfindingIdle();
                state.Init(this);
                _stateMap[eStateType.IDLE] = state;
            }
            {
                State state = new PathfindingTestMove();
                state.Init(this);
                _stateMap[eStateType.MOVE] = state;
            }
            {
                State state = new PathfindingImmediately();
                state.Init(this);
                _stateMap[eStateType.PATHFINDING] = state;
            }

            _state = _stateMap[eStateType.IDLE];
        }
    }
}
