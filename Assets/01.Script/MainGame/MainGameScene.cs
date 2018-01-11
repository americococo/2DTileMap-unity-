using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{

    public TileMap _tileMap;
    public Player _TestPlayer;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }
    void Init()
    {
        _tileMap.Init();
        GameManger.Instance.SetMap(_tileMap);
        _TestPlayer.Init();
    }
}
