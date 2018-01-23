using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingIdle : State
{

    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                MapObject mapObject = hit.collider.GetComponent<MapObject>();
                if (null != mapObject)
                {
                    hit.collider.GetComponent<SpriteRenderer>().color = Color.black;

                    TileCell GoalCell = GameManger.Instance.GetMap().GetTileCell(mapObject.GetTileX(), mapObject.GetTileY());
                    Debug.Log("찾음" + GoalCell.ToString());

                    _character.SetGoalTileCell(GoalCell);
                }

                if (null != _character.getGoalTileCell())
                {
                    _nextState = eStateType.PATHFINDING;
                }

            }
        }
    }
}
