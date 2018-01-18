using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : State
{
    public override void Start()
    {
        base.Start();

        _character.SetCanMove(true);

    }
}
