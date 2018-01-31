using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAR : State
{
    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }

        if (Enemy.GetObjectType() == eMapObjectType.CHARACTER)
        {
            if (_character.IsAttackAble())
            {
                if (((Character)Enemy).Islive())
                {
                    _character.Attack(Enemy);
                    return;
                }
                else
                    _nextState = eStateType.IDLE;
            }
        }
        else
            _nextState = eStateType.IDLE;
    }
    MapObject Enemy;

    public override void Start()
    {
        base.Start();

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
        //Debug.Log(_character.ToString() + "x: " +moveX.ToString() +  "Y:" + moveY.ToString()) ;


        List<MapObject> collisionList = GameManger.Instance.GetMap().GetCollisionList(moveX, moveY);

        Debug.Log(collisionList.Count.ToString());

        for (int i = 0; i < collisionList.Count; i++)
        {
            Debug.Log(collisionList[i].ToString());
            
            switch (collisionList[i].GetObjectType())
            {
                case eMapObjectType.CHARACTER:
                    Enemy = collisionList[i];
                    break;
            }
        }

    }
}
