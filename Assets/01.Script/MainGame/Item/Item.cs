using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MapObject
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject _itemView;

    public void Init(string resourceName)
    {
        string filePath = "Prefabs/ItemView/" + resourceName ;
        GameObject ItemViewPrefabs = Resources.Load<GameObject>(filePath);
        _itemView = GameObject.Instantiate(ItemViewPrefabs);
        _itemView.transform.SetParent(transform);

        _itemView.transform.localScale = Vector3.one;
        _itemView.transform.localPosition = Vector3.zero;

        TileMap map = GameManger.Instance.GetMap();

        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);

    }
    public override void ReceiverObjcectMessage(ObjectMessageParam messageParam)
    {
        switch(messageParam.message)
        {
            case "EAT":

                break;

        }


    }
}
