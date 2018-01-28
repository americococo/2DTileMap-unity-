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
                MapObject mapObject = hit.collider.gameObject.GetComponent<MapObject>();
                if (null != mapObject)
                {
                    hit.collider.GetComponent<SpriteRenderer>().color = Color.black;

                    TileMap map = GameManger.Instance.GetMap();

                    TileCell GoalCell = map.GetTileCell(mapObject.GetTileX(), mapObject.GetTileY());
                    //Debug.Log("찾음" + GoalCell.ToString());
                    List<MapObject> mapCollision = map.GetCollisionList(GoalCell.GetTileX(), GoalCell.GetTileY());
                    if (0 != mapCollision.Count)
                    {
                        if (eMapObjectType.CHARACTER == mapCollision[0].GetObjectType())
                        {
                            _character.SetGoalTileCell(GoalCell);
                        }
                    }
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
