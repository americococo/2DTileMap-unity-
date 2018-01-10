﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetPosition(Vector2 postion)
    {
        gameObject.transform.localPosition = postion;
    }
}
