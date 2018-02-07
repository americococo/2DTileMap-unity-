using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{

    override public void Start()
    {
        base.Start();
        Debug.Log("ATK");
        MapObject mapObject;

        if (_character.IsAttackAble())
            mapObject = _character.Attack();

        _nextState = eStateType.IDLE;
    }

}
