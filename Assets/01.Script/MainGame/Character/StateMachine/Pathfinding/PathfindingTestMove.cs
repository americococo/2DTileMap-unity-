using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTestMove : State
{
    public override void Update()
    {


        if( false == _character.IsEmptyPathfindingTileCell() )
        {
            TileCell tileCell = _character.PopPathfindingTileCell();

            sPosition Curposition;
            Curposition.x =  _character.GetTileCell().GetTileX();
            Curposition.y = _character.GetTileCell().GetTileY();

            sPosition toPosition;
            toPosition.x = tileCell.GetTileX();
            toPosition.y = tileCell.GetTileY();

            eMoveDirection direction = _character.GetDirection(toPosition, Curposition);

            _character.SetNextDirection(direction);

            if (tileCell.CanMove())
                _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());

            else
            {
                _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());
                _nextState = eStateType.BATTLE;
            }
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


}
