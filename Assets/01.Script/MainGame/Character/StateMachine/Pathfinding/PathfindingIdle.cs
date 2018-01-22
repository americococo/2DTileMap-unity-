using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingIdle : State
{

    TileCell destination=null;

    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            LayerMask tileobejcet=1 << LayerMask.NameToLayer("tileobejcet");//그라운드이름의 레이어를 캐릭터 레이를 통해 검사
            RaycastHit2D hitFromTile = Physics2D.Raycast(Input.mousePosition, new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1),tileobejcet);

            //Debug.Log(hitFromTile.collider.transform.localPosition.ToString()); 테스트 코드 테스트 성공

            hitFromTile.collider.GetComponent<TileObject>();

            if(null!=destination)
            {
                _nextState = eStateType.PATHFINDINGMOVE;
            }
        }
    }
}
