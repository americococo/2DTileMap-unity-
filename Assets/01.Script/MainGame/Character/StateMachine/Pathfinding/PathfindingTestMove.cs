using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTestMove : State
{

    Stack<TileCell> CurrentTileCell;

    public override void Start()
    {
        base.Start();
        CurrentTileCell = _character.getRoot();
        CurrentTileCell.Pop();
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

        if (true == _character.IsEmptyPathFindingTileCell())
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
