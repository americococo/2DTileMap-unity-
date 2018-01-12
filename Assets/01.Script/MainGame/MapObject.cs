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

    public void BecomeViewr()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, Camera.main.transform.localPosition.z);
    }

    virtual public void SetSortingOrder(int sortingID, int sortingOrder)
    {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
}
