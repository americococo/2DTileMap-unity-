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
            _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }

}
