using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pathfinding : State
{
    struct sPosition
    {
        public int tileX;
        public int tileY;
    }

    enum ePathState
    {
        PATH,
        BUILD,
    }



    Queue<TileCell> _pathfindingQueue = new Queue<TileCell>();

    public override void Stop()
    {
        base.Stop();
        _pathState = ePathState.PATH;
        _pathfindingQueue.Clear();
        _character.SetGoalTileCell(null);
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();


        if (null == _character.getGoalTileCell())
            _nextState = eStateType.IDLE;
        else
        {
            TileMap map = GameManger.Instance.GetMap();
            map.ResetPathfinding();

            TileCell tileCell = _character.GetTileCell();
            tileCell.prevTileCell = null;

            _pathfindingQueue.Enqueue(tileCell);
        }
    }

    ePathState _pathState;

    // Update is called once per frame
    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);

        }

        switch (_pathState)
        {
            case ePathState.PATH:
                PathUpdate();
                break;
            case ePathState.BUILD:
                BuildUpdate();
                break;

        }

    }

    void PathUpdate()
    {
        //큐가 빌때까지 큐에 있는 커맨드패스를 커내 검사
        if (0 != _pathfindingQueue.Count)
        {
            TileCell tilecell = _pathfindingQueue.Dequeue();

            //캐맨드 검사
            //(커맨드에 포함됀 타일셀이 방문하지 않는 타일셀인경우)
            if (false == tilecell.IsPathFindingMark())
            {
                //방문 처리
                tilecell.SetPathFindingMark();

                //목표 도달시 종료 Idle상태로 전환
                if (tilecell == _character.getGoalTileCell())
                {
                    Debug.Log("Finded");

                    //TileCell MoveRootCell = tilecell;
                    //while (null != MoveRootCell.prevTileCell)
                    //{
                    //    _character.pushTilecell(MoveRootCell);
                    //    MoveRootCell = MoveRootCell.prevTileCell;
                    //}

                    //_nextState = eStateType.MOVE;
                    _pathState = ePathState.BUILD;
                    _reverce = tilecell;
                    return;
                }

                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.NONE; direction++)
                {
                    sPosition position;
                    position.tileX = tilecell.GetTileX();
                    position.tileY = tilecell.GetTileY();
                    sPosition nextPosition = GetPositopmByDirection(position, (eMoveDirection)direction);
                    //tilecell를 찾기위해서 있는 좌표값

                    if ((nextPosition.tileX >= 0) && (GameManger.Instance.GetMap().GetWidth() > nextPosition.tileX) &&
                   (nextPosition.tileY >= 0) && (GameManger.Instance.GetMap().GetHeight() > nextPosition.tileY))
                    {
                        TileCell nextTileCell = GameManger.Instance.GetMap().GetTileCell(nextPosition.tileX, nextPosition.tileY);

                        if (nextTileCell.CanMove() && false == nextTileCell.IsPathFindingMark())//이동 가능하며 탐색안한 타일만 큐에 넣엉줌
                        {

                            float distance = tilecell.GetDistanceFromStart() + nextTileCell.GetDistanceWidght();
                            nextTileCell.SetDistanceFromStart(distance);
                            // nextTileCell.PathFindingMarking();

                            TileCell serchTileCell = nextTileCell;
                            serchTileCell.prevTileCell = tilecell;
                            _pathfindingQueue.Enqueue(serchTileCell);
                        }
                    }
                }
            }
        }
    }

    TileCell _reverce;

    void BuildUpdate()
    {
        if(null != _reverce )
        {
            _character.pushPathfindingTileCell(_reverce);
            _reverce.ColorBackUp();
            _reverce = _reverce.prevTileCell;
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
    }

    sPosition GetPositopmByDirection(sPosition position, eMoveDirection direction)
    {
        sPosition newposition = position;
        switch (direction)
        {
            case eMoveDirection.LEFT:
                newposition.tileX--; break;
            case eMoveDirection.RIGHT:
                newposition.tileX++; break;
            case eMoveDirection.UP:
                newposition.tileY++; break;
            case eMoveDirection.DOWN:
                newposition.tileY--; break;
        }

        return newposition;
    }
}
