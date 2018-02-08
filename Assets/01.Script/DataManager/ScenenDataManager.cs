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
    Player _player;

    public void setCharacterData(Player player)
    {
        _player = player;
    }

    public Player getCharacterData()
    {
        return _player;
    }


}