using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : State
{

    public override void Start()
    {
        base.Start();
        Debug.Log(_character);
    }
    public override void Update()
    {
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
                    if (_character.IsAttackAble())
                    {
                        _character.Attack(collisionList[i]);
                        if (false== ((Character)collisionList[i]).Islive())
                        {
                            Debug.Log(collisionList[i].GetTileX().ToString() + collisionList[i].GetTileY().ToString());
                            _nextState = eStateType.IDLE;
                        }
                    }
                    break;
            }
        }
    }

}
