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

    protected GameObject _itemView;


    public void Init(string viewName)
    {
        string filePath = "Prefabs/Item/ItemView/" + viewName;
        GameObject ItemViewPrefabs = Resources.Load<GameObject>(filePath);
        _itemView = GameObject.Instantiate(ItemViewPrefabs);
        _itemView.transform.SetParent(transform);

        _itemView.transform.localScale = Vector3.one;
        _itemView.transform.localPosition = Vector3.zero;

        
    }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _currentlayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        _itemView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _itemView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

}
