using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Awake()
    {
        _attackPoint = 10;
    }

    void Start()
    {

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
