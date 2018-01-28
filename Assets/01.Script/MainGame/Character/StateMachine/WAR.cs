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
    }

    public override void Start()
    {
        base.Start();

        Debug.Log(_character.ToString());
    }
}
