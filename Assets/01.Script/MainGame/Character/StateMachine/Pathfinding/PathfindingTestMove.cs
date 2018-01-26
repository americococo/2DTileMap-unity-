using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTestMove : State
{
    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }

        if( false == _character.IsEmptyPathfindingTileCell() )
        {
            TileCell tileCell = _character.PopPathfindingTileCell();

            sPosition Curposition;
            Curposition.x =  _character.GetTileCell().GetTileX();
            Curposition.y = _character.GetTileCell().GetTileY();

            sPosition toPosition;
            toPosition.x = tileCell.GetTileX();
            toPosition.y = tileCell.GetTileY();

            eMoveDirection direction = GetDirection(toPosition, Curposition);

            _character.SetNextDirection(direction);


            _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());
        }
        else
        {
            _nextState = eStateType.IDLE;
        }

    }
    public override void Start()
    {
        base.Start();

        _character.PopPathfindingTileCell();
    }
    public override void Stop()
    { 
        base.Stop();

        _character.clearPathfindingTileCell();
    }

    eMoveDirection GetDirection(sPosition to,sPosition cur)
    {
        eMoveDirection directionbe= eMoveDirection.NONE;

        if (cur.x < to.x)
            directionbe = eMoveDirection.RIGHT; 
        if (cur.x > to.x)
            directionbe = eMoveDirection.LEFT;
        if (cur.y < to.y)
            directionbe = eMoveDirection.UP;
        if (cur.y > to.y)
            directionbe = eMoveDirection.DOWN;

        return directionbe;
    }
}
