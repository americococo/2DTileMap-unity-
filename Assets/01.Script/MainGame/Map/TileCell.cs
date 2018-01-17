﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eTileLayer
{
    GROUND,
    MIIDDLE,
    MAXCOUNT,
}


public class TileCell
{

    Vector2 _postion;

    List<List<MapObject>> _MapObjectMap = new List<List<MapObject>>();
    

    public void Init()
    {
        for(int i=0;i<(int)eTileLayer.MAXCOUNT;i++)
        {
            List<MapObject> MapObjectList = new List<MapObject>();
            _MapObjectMap.Add(MapObjectList);
        }
    }


    public void SetPosition(float x, float y)
    {
        _postion.x = x;
        _postion.y = y;
    }


    //add / Remove
    public void AddObject(eTileLayer layer,MapObject mapObject)
    {
        List<MapObject> mapObjectList = _MapObjectMap[(int)layer];

        //int sortingID = SortingLayer.NameToID(layer.ToString());
        int sortingOder = mapObjectList.Count;

        //mapObject.SetSortingOrder(sortingID, sortingOder);
        mapObject.SetSortingOrder(layer, sortingOder);
        mapObject.SetPosition(_postion);

        mapObjectList.Add(mapObject);
    }
    public void RemoveObject(MapObject mapObject)
    {
        List<MapObject> mapObjectList = _MapObjectMap[(int)mapObject.GetLayer()];
        mapObjectList.Remove(mapObject);
    }


    public bool CanMove()
    {
        for(int layer=0; layer<(int)eTileLayer.MAXCOUNT;layer++)
        {
            List<MapObject> mapObject = _MapObjectMap[layer];
            for(int i=0;i<mapObject.Count;i++)
                if(false==mapObject[i].CanMove())
            return false;
        }
        return true;
    }
    public List<MapObject> GetCollsionList()
    {
        List<MapObject> CollsionList = new List<MapObject>();

        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> mapObject = _MapObjectMap[layer];
            for (int i = 0; i < mapObject.Count; i++)
                if (false == mapObject[i].CanMove())
                    CollsionList.Add(mapObject[i]);
        }
        return CollsionList;
    }
}