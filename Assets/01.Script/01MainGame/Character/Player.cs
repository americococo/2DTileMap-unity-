using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Awake()
    {
        _level = 1;
        _Exp = 0;
        _hp = _fullHp;
        _attackCoolTime = 0.3f;
    }

    void Start()
    { 
        _ObjectType = eMapObjectType.CHARACTER;

        if (null != ScenenDataManager.Instance.getCharacterData())
        {
            _level = ScenenDataManager.Instance.getCharacterData().getLevel();
            _Exp = ScenenDataManager.Instance.getCharacterData().getExp();
            _hp = ScenenDataManager.Instance.getCharacterData().getHp();
        }
        
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
