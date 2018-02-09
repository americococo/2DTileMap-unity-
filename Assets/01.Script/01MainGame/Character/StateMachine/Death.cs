using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : State
{
    public override void Start()
    {
        base.Start();

        _character.SetCanMove(true);
        _character.gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);

        Item item= _character.GetDeathItem();

        TileMap map = GameManger.Instance.GetMap();

        map.SetObject(_character.GetTileX(), _character.GetTileY(), item, eTileLayer.ITEM);

    }
}
