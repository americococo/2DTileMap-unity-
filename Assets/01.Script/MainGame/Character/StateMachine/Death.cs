using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : State
{
    public override void Start()
    {
        base.Start();

        _character.SetCanMove(false);
        _character.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);

    }
}
