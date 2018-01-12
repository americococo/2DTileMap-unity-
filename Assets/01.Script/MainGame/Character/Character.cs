using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MapObject
{

    GameObject _chracterView;

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

        int x = Random.Range(1, map.GetWidth() - 2);
        int y = Random.Range(1, map.GetHeight() - 2);
        TileCell tileCell = map.GetTileCell(x, y);
        tileCell.AddObject(eTileLayer.MIIDDLE, this);
    }


    override public void SetSortingOrder(int sortingID, int sortingOrder)
    {
        _chracterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _chracterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
}
