using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMapObjectType
{
    NONE,
    MONSTER,
}


public class MapObject : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }
    public void SetPosition(Vector2 postion)
    {
        gameObject.transform.localPosition = postion;
    }

    public void BecomeViewr()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, Camera.main.transform.localPosition.z);
    }

    virtual public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _currentlayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    protected eTileLayer _currentlayer;

    public eTileLayer GetLayer()
    {
        return _currentlayer;
    }


    bool _canMove = true;
    public bool CanMove()
    {
        return _canMove;
    }

    public void SetCanMove(bool canmove)
    {
        _canMove = canmove;
    }

    protected eMapObjectType _ObjectType=eMapObjectType.NONE;

    public eMapObjectType GetObjectType()
    {
        return _ObjectType;
    }

    //message
    public void ReceiverObjcectMessage(ObjectMessageParam messageParam)
    {
        switch (messageParam.message)
        {
            case "ATTACK":
                Debug.Log("ATTACK" + messageParam.attackpoint);break;
        }

    }
}
