using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdle : State
{
    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }

    }

}
