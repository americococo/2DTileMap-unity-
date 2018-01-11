using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    virtual public void SetSortingOrder(int sortingID, int sortingOrder)
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

}
