using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{

    override public void Start()
    {
        base.Start();
        Debug.Log("ATK");

        

        _character.UpdateAttackCoolTime();
        int moveX = _character.GetTileX();
        int moveY = _character.GetTileY();

        switch (_character.GetNextDirection())
        {
            case eMoveDirection.LEFT:
                moveX--;
                break;
            case eMoveDirection.RIGHT:
                moveX++;
                break;
            case eMoveDirection.UP:
                moveY++;
                break;
            case eMoveDirection.DOWN:
                moveY--;
                break;
        }

        TileMap map = GameManger.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
        for (int i = 0; i < collisionList.Count; i++)
        {
            switch (collisionList[i].GetObjectType())
            {
                case eMapObjectType.CHARACTER:
                    _character.Attack(collisionList[i]);
                    
                    break;
            }
        }

        _character.SetNextDirection(eMoveDirection.NONE);
        _nextState = eStateType.IDLE;
    }

}
