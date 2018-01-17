using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eMoveDirection
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}




public class Character : MapObject
{

    protected GameObject _chracterView;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(string viewName)
    {
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);
        _chracterView = GameObject.Instantiate(characterViewPrefabs);
        _chracterView.transform.SetParent(transform);

        _chracterView.transform.localScale = Vector3.one;
        _chracterView.transform.localPosition = Vector3.zero;

        TileMap map = GameManger.Instance.GetMap();

        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);

        //TileCell tileCell = map.GetTileCell(x, y);
        //tileCell.AddObject(eTileLayer.MIIDDLE, this);

        map.SetObject(_tileX, _tileY, this, eTileLayer.MIIDDLE);
    }


    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _currentlayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        _chracterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _chracterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    protected int _tileX;
    protected int _tileY;


    //attack
    protected int _attackPoint;
}
