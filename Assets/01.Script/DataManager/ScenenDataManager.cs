using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenenDataManager 
{

    static ScenenDataManager _instance;
    public static ScenenDataManager Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new ScenenDataManager();
            }
            return _instance;
        }
    }
    Character PlayerData;

    public void SetPlayerData(Character player)
    {
        PlayerData = player;
    }

    public Character getPlayer()
    {
        return PlayerData;
    }

}
