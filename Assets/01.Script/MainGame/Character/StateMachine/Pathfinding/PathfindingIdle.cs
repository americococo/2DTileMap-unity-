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
                        Debug.Log("찾음");
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                        TileCell selectTilecell = GameManger.Instance.GetMap().GetTileCell(mapObject.GetTileX(), mapObject.GetTileY());
                        _character.SetGoalTileCell(selectTilecell);
                    }
                }
            }
            TileCell destination = _character.getGoalTileCell();

            if (null != destination)
            {
                _nextState = eStateType.PATHFINDING;
            }

            //LayerMask tileobejcet = 1 << LayerMask.NameToLayer("tileobejcet");//그라운드이름의 레이어를 캐릭터 레이를 통해 검사
            //RaycastHit2D hitFromTile = Physics2D.Raycast(Input.mousePosition, new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1), tileobejcet);

            //Debug.Log(hitFromTile.collider.transform.localPosition.ToString());// 테스트 코드 테스트 성공

            ////hitFromTile.collider.GetComponent<SpriteRenderer>().color=Color.red;

            //int TileX = hitFromTile.collider.GetComponent<TileObject>().GetTileX();
            //int TileY = hitFromTile.collider.GetComponent<TileObject>().GetTileY();

            //destination = GameManger.Instance.GetMap().GetTileCell(TileX, TileY);
        }
    }
}
