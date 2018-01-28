using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTestMove : State
{

    public override void Start()
    {
        base.Start();

        _character.popPathFindingTileCell();
    }
    public override void Stop()
    {
        base.Stop();

        _character.resetSerchRoot();
    }

    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }

        if (false == _character.IsEmptyPathFindingTileCell())
        {
            TileCell tileCell = _character.popPathFindingTileCell();

            sPosition CurPostion;
            CurPostion.tileX = _character.GetTileX();
            CurPostion.tileY = _character.GetTileY();

            sPosition toPostion;
            toPostion.tileX = tileCell.GetTileX();
            toPostion.tileY = tileCell.GetTileY();

            eMoveDirection direction = getMoveDirection(CurPostion, toPostion);
            _character.SetNextDirection(direction);

            bool Canmove = tileCell.CanMove();

            if (Canmove)
            {
                _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());
            }
            else
            {
                _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());
                List<MapObject> collisionList = GameManger.Instance.GetMap().GetCollisionList(tileCell.GetTileX(), tileCell.GetTileY());
                for (int i = 0; i < collisionList.Count; i++)
                {
                    if (eMapObjectType.CHARACTER == collisionList[i].GetObjectType())
                    {
                        _nextState = eStateType.WAR;

                        break;
                    }
                }
            }
        }
    
        else
        {
            _nextState = eStateType.IDLE;
        }
    }

    eMoveDirection getMoveDirection(sPosition CurPostion, sPosition toPostion)
{
    eMoveDirection signalDirection = eMoveDirection.NONE;

    if (CurPostion.tileX > toPostion.tileX)
        signalDirection = eMoveDirection.LEFT;
    if (toPostion.tileX > CurPostion.tileX)
        signalDirection = eMoveDirection.RIGHT;

    if (CurPostion.tileY > toPostion.tileY)
        signalDirection = eMoveDirection.DOWN;

    if (toPostion.tileY > CurPostion.tileY)
        signalDirection = eMoveDirection.UP;

    return signalDirection;
}
}
