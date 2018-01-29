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

            if(Physics.Raycast(ray,out hit,100))
            {
                MapObject mapObject = hit.collider.GetComponent<MapObject>();
                if(null!=mapObject)
                {
                    if(eMapObjectType.TILE_OBJECT == mapObject.GetObjectType())
                    { 
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        
                        TileCell selectTilecell = GameManger.Instance.GetMap().GetTileCell(mapObject.GetTileX(), mapObject.GetTileY());
                        if (true == selectTilecell.IsPathfindable())
                            _character.SetGoalTileCell(selectTilecell);
                    }
                }
            }
            TileCell destination = _character.getGoalTileCell();

            if (null != destination)
            {
                _nextState = eStateType.PATHFINDING;
            }

        }
    }
}
