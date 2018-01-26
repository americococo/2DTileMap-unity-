using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingImmediately : Pathfinding
{
    public override void Start()
    {
        base.Start();
        while (0 != _pathfindingQueue.Count)
        {
            if (ePathState.BUILD == _pathState)
                break;
            base.PathUpdate();
        }
        while (null != _reverce)
        {
            BuildUpdate();
        }
        _nextState = eStateType.MOVE;
    }
    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);
        }
    }
}
