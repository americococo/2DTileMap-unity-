using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : State
{

    override public void Start()
    {
        base.Start();

        int damage = _character.GetDamagePoint();

        _character.DecreaseHp(damage);

        if (false == _character.Islive())
        {
            _nextState = eStateType.DEATH;
        }
        else
            _nextState = eStateType.IDLE;
    }

}