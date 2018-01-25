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
}
