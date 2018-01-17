using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Awake()
    {
        _attackPoint = 10;
    }

    void Start()
    {

    }

    void Update()
    {

        if (false == _isLive)
            return;
        eMoveDirection movedirection = _state.Update();


        //moveState에게 먹히기 때문에 필요없읍
        //if (eMoveDirection.NONE != moveDirection)
        //{
        //    movoingDirection(moveDirection);
        //}


    }
    void movoingDirection(eMoveDirection moveDirection)
    {
        string animationTrigger = "Down";

        int moveX = _tileX;
        int moveY = _tileY;

        switch (moveDirection)
        {
            case eMoveDirection.LEFT:
                animationTrigger = "Left";
                moveX--; break;
            case eMoveDirection.RIGHT:
                animationTrigger = "Right";
                moveX++; break;
            case eMoveDirection.UP:
                animationTrigger = "Up";
                moveY++; break;
            case eMoveDirection.DOWN:
                animationTrigger = "Down";
                moveY--; break;

        }

        _chracterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        //이동 가능여부 체크
        TileMap map = GameManger.Instance.GetMap();

        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);//충돌list불러옴
        if (0 == collisionList.Count)//충돌list가 있으면 이동x 
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIIDDLE);
        }
        else//충돌 list에서 값불러옴
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                switch (collisionList[i].GetObjectType())
                {
                    case eMapObjectType.MONSTER:
                        Attack(collisionList[i]);
                        break;
                }

            }
        }
    }


}
