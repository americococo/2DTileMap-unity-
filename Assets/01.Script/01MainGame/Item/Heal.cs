using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Item
{

    // Use this for initialization
    void Start()
    {
        _ObjectType = eMapObjectType.ITEM;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ReceiverObjcectMessage(ObjectMessageParam messageParam)
    {
        switch (messageParam.message)
        {
            case "EAT":
                UseItem(messageParam.sender.GetComponent<Character>());
                DestroyObject(_itemView);
                
                Debug.Log(messageParam.sender.GetComponent<Character>().getHp());
                break;
        }

    }

    void UseItem(Character ItemUser)
    {
        ItemUser.recovery(10);
    }
}
