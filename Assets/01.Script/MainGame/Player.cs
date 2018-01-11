using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MapObject
{
    public GameObject _chracterView;
    void Start()
    {
        
    }

    void Update()
    {

    }
    public void Init()
    {
        TileMap map = GameManger.Instance.GetMap();

        int x = Random.Range(1,map.GetWidth()-2);
        int y = Random.Range(1, map.GetHeight() - 2);
        TileCell tileCell= map.GetTileCell(x,y);
        tileCell.AddObject(eTileLayer.MIIDDLE, this);
    }
    override public void SetSortingOrder(int sortingID, int sortingOrder)
    {
        _chracterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _chracterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
}
