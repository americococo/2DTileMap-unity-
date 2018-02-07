using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messageSystem
{
    static messageSystem _instance;
    public static messageSystem Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new messageSystem();
            }
            return _instance;
        }
    }

    Queue<ObjectMessageParam> _messageQueue = new Queue<ObjectMessageParam>();

    public void Send(ObjectMessageParam messageParam)
    {
        _messageQueue.Enqueue(messageParam);
    }
    public void ProcessMessage()
    {
        while(0!= _messageQueue.Count)
        {
            //ReceiverObjectMessage
            ObjectMessageParam messageParam = _messageQueue.Dequeue();
            messageParam.receiver.ReceiverObjcectMessage(messageParam);
        }
    }

}
