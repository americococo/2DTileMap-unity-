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

        //int moveX = _tileX;
        //int moveY = _tileY;
        //입력 이동(결합상태)
        eMoveDirection moveDirection= eMoveDirection.NONE;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = eMoveDirection.LEFT;
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = eMoveDirection.RIGHT;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = eMoveDirection.UP;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = eMoveDirection.DOWN;
        }


        if(eMoveDirection.NONE != moveDirection)
        {
            movoingDirection(moveDirection);
        }


    }
    void movoingDirection(eMoveDirection moveDirection)
    {
        string animationTrigger = "Down";

        int moveX = _tileX;
        int moveY = _tileY;

        switch (moveDirection)
        {
            case eMoveDirection.LEFT: animationTrigger = "Left";
                moveX--;break;
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

        //if (true == map.CanMoveTile(moveX, moveY))
        //{
        //    map.ResetObject(_tileX, _tileY, this);
        //    _tileX = moveX;
        //    _tileY = moveY;
        //    map.SetObject(_tileX, _tileY, this, eTileLayer.MIIDDLE);
        //}

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
            for(int i=0;i<collisionList.Count;i++)
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

    void Attack(MapObject Ene)
    {
        ObjectMessageParam messageParam = new ObjectMessageParam();
        messageParam.sender = this;
        messageParam.receiver = Ene;
        messageParam.attackpoint = _attackPoint;
        messageParam.message = "ATTACK";

        messageSystem.Instance.Send(messageParam);
    }
}
