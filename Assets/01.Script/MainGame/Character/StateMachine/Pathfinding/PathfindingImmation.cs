using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingImmation : Pathfinding {

    override protected void UpdatePathfinding()
    {
        base.UpdatePathfinding();

        while(0!= _queue.count)
        {
            pathfinding();
        }

        while(null!= _reverce)
        {
            UpdateBuildPath();
        }
    }

}
