using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eTileLayer
{
    GROUND,
}


public class TileCell
{

    Vector2 _postion;

    public void Init()
    {

    }


    public void SetPosition(float x, float y)
    {
        _postion.x = x;
        _postion.y = y;
    }
    public void AddObject(eTileLayer layer,TileObject tileObject)
    {
        tileObject.SetPosition(_postion);
        //sorting order,layer
    }
}
