using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAR : State
{
    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }

        if (Enemy.GetObjectType() == eMapObjectType.CHARACTER)
        {
            if (_character.IsAttackAble())
            {
                if (((Character)Enemy).Islive())
                {
                    _character.Attack();
                    return;
                }
                else
                    _nextState = eStateType.IDLE;
            }
        }
        else
            _nextState = eStateType.IDLE;
    }
    MapObject Enemy;

    public override void Start()
    {
        base.Start();

        if (_character.IsAttackAble())
            Enemy= _character.Attack();

    }
}
