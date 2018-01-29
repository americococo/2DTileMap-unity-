using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAR : State
{
    public override void Update()
    {
       
    }
    Character Enemy;

    public override void Start()
    {
        base.Start();

        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }

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
            case eMoveDirection.DOWN:
                moveY--;
                break;
            case eMoveDirection.UP:
                moveY++;
                break;
        }

        List<MapObject> collisionList = GameManger.Instance.GetMap().GetCollisionList(moveX, moveY);
        for (int i = 0; i < collisionList.Count; i++)
        {
            switch (collisionList[i].GetObjectType())
            {
                case eMapObjectType.CHARACTER:
                    Enemy = (Character)collisionList[i];
                    if (_character.IsAttackAble())
                    {
                        Enemy = (Character)collisionList[i];
                        _character.Attack(collisionList[i]);
                        if (false == Enemy.Islive())
                        {
                            _nextState = eStateType.IDLE;
                            break;
                        }
                    }
                    break;
            }
        }
        _nextState = eStateType.IDLE;

    }
}
