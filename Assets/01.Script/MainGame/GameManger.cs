using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger
{
    //싱글턴
    static GameManger _instance;
    public static GameManger Instance
    {
        get
        {
            if(null ==_instance)
            {
                _instance = new GameManger();
                _instance.Init();
            }
            return _instance;
        }
    }

    void Init()
    {
        
    }


    //map
    TileMap _tileMap;
    public TileMap GetMap()
    {
        return _tileMap;
    }

    public void SetMap(TileMap map)
    {
        _tileMap = map;
    }
}
