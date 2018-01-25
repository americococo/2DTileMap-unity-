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

    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }

        TileCell tilecell = CurrentTileCell.Pop();

        if (tilecell.CanMove())
        {
            _character.MoveStart(tilecell.GetTileX(), tilecell.GetTileY());
        }

        if (tilecell.prevTileCell ==  _character.getGoalTileCell() )
        {
            _character.resetSerchRoot();
            _nextState = eStateType.IDLE;
        }
    }

}
