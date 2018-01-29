using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : State
{
   protected struct sPathCommand
    {
        public TileCell tileCell;
        //public TileCell prevTileCell;
        public float heuristic;
    }
    //타일셀로 
    
    enum eUpdateState
    {
        PATHFINDING,
        BUILDPATH
    }

    eUpdateState _updateState = eUpdateState.PATHFINDING;

    protected List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();

    public override void Stop()
    {
        base.Stop();

        _updateState = eUpdateState.PATHFINDING;

        _pathfindingQueue.Clear();
        _character.SetGoalTileCell(null);
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        TileMap map = GameManger.Instance.GetMap();


        if (null != _character.getGoalTileCell())
        {
            map.ResetPathfinding();
            sPathCommand cmd;

            cmd.tileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());
            cmd.heuristic = 0;

            PushCommand(cmd);
        }
        else
            _nextState = eStateType.IDLE;

    }

    // Update is called once per frame
    public override void Update()
    {


        switch (_updateState)
        {
            case eUpdateState.PATHFINDING:
                UpdatePathfinding();
                break;
            case eUpdateState.BUILDPATH:
                UpdateBuildPath();
                break;
        }
    }

    protected virtual void UpdatePathfinding()
    {
        //큐가 빌때까지 큐에 있는 커맨드패스를 커내 검사
        if (0 != _pathfindingQueue.Count)
        {
            //sPathCommand cmd = _pathfindingQueue.Dequeue();
            sPathCommand cmd = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);
            //캐맨드 검사
            //(커맨드에 포함됀 타일셀이 방문하지 않는 타일셀인경우)
            if (false == cmd.tileCell.IsPathFindingMark())
            {
                //방문 처리
                cmd.tileCell.PathFindingMarking();
                //목표 도달시 종료 Idle상태로 전환
                if (cmd.tileCell.GetTileX() == _character.getGoalTileCell().GetTileX() &&
                    cmd.tileCell.GetTileY() == _character.getGoalTileCell().GetTileY())
                {

                    Debug.Log("Finded");
                    //_nextState = eStateType.IDLE;
                    _updateState = eUpdateState.BUILDPATH;
                    _reverseTileCell = cmd.tileCell;
                    return;

                }


                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.NONE; direction++)
                {
                    sPosition curPosition;
                    curPosition.x = cmd.tileCell.GetTileX();
                    curPosition.y = cmd.tileCell.GetTileY();
                    sPosition nextPosition = GetPositopmByDirection(curPosition, direction);
                    //tilecell를 찾기위해서 있는 좌표값

                    TileCell nextTileCell = GameManger.Instance.GetMap().GetTileCell(nextPosition.x, nextPosition.y);

                    if (nextTileCell.IsPathfindable() && false == nextTileCell.IsPathFindingMark())//이동 가능하며 탐색안한 타일만 큐에 넣엉줌
                    {
                        float distanceFromStart = cmd.tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceWidght();
                        //float heuristic = CalcSimpleHeuristic(cmd.tileCell, nextTileCell, _character.getGoalTileCell());// 현타일과 다음타일 을 목표타일로 부터 좌표 비교
                        //float heuristic = CalcComplexcHeuristic(nextTileCell, _character.getGoalTileCell());//거리과 가까운지 최단거리
                        float heuristic = CalcAStarHeuristic(distanceFromStart, nextTileCell, _character.getGoalTileCell()); //시작점으로 부터 얼마나 가까운가 + 얼마나 목적지까지 가까운가

                        if (null == nextTileCell.GetPrevfindingCell())
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevPathfindingCell(cmd.tileCell);
                            sPathCommand newCmd;
                            newCmd.tileCell = nextTileCell;
                            
                            newCmd.heuristic = heuristic;
                            
                            PushCommand(newCmd);
                        }

                        else
                        {
                            if (distanceFromStart < nextTileCell.GetDistanceFromStart())
                            {
                                nextTileCell.SetDistanceFromStart(direction);
                                nextTileCell.SetPrevPathfindingCell(cmd.tileCell);

                                sPathCommand newCmd;
                                newCmd.tileCell = nextTileCell;
                                
                                newCmd.heuristic = heuristic;
                                PushCommand(newCmd);
                            }
                        }

                    }

                }

            }
        }
    }


    protected TileCell _reverseTileCell = null;

    protected void UpdateBuildPath()
    {
        if (null != _reverseTileCell)
        {
            _reverseTileCell.RestPathfindMark();
            _character.PushPathfindingTileCell(_reverseTileCell);
            _reverseTileCell = _reverseTileCell.GetPrevfindingCell();
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
    }

    sPosition GetPositopmByDirection(sPosition position, int direction)
    {
        sPosition newPosition = position;

        switch ((eMoveDirection)direction)
        {
            case eMoveDirection.LEFT:
                newPosition.x--;
                break;
            case eMoveDirection.DOWN:
                newPosition.y--;
                break;
            case eMoveDirection.UP:
                newPosition.y++;
                break;
            case eMoveDirection.RIGHT:
                newPosition.x++;
                break;
        }
        return newPosition;
    }

    void PushCommand(sPathCommand command)//가중치 부여 서포트
    {
        _pathfindingQueue.Add(command);

        //sorting
        _pathfindingQueue.Sort(delegate (sPathCommand a, sPathCommand b)
        {
            return a.heuristic.CompareTo(b.heuristic);
        });
    }
    float CalcSimpleHeuristic(TileCell tileCell, TileCell nextTileCell, TileCell targetTileCell)
    {
        //현재타일과 목표타일까지 거리 와
        //다음타일과 목표타일까지 거리 비교
        //더 좋은좌표값에 높은 가중치 부여

        float heuristic = 0.0f;

        int diffFromCurrent;
        int diffFromNext;

        // x 축

        {
            //현재 타일과 목표 차이비교
            diffFromCurrent = tileCell.GetTileX() - targetTileCell.GetTileX();

            if (diffFromCurrent < 0)
                diffFromCurrent = -diffFromCurrent;

            //다음에 갈타일과 목표  차이 비교
            diffFromNext = nextTileCell.GetTileX() - targetTileCell.GetTileX();
            if (diffFromNext < 0)
                diffFromNext = -diffFromNext;

            if (diffFromNext < diffFromCurrent)
            {
                heuristic -= 1.0f;
            }
            else if (diffFromCurrent < diffFromNext)   
            {
                heuristic += 1.0f;
            }
        }

        // y 축
        {
            diffFromCurrent = tileCell.GetTileY() - targetTileCell.GetTileY();
            if (diffFromCurrent < 0)
                diffFromCurrent -= diffFromCurrent;

            diffFromNext = nextTileCell.GetTileY() - targetTileCell.GetTileY();
            if (diffFromNext < 0)
                diffFromNext = -diffFromNext;

            if (diffFromNext < diffFromCurrent)
            {
                heuristic -= 1.0f;
            }
            else if (diffFromCurrent < diffFromNext)
            {
                heuristic += 1.0f;
            }
        }

        return heuristic;
    }

    float CalcComplexcHeuristic(TileCell  nextTilecell, TileCell targetTileCell)
    {

        int distanceW = nextTilecell.GetTileX() - targetTileCell.GetTileX();

        int distanceH = nextTilecell.GetTileY() - targetTileCell.GetTileY();

        distanceH *= distanceH;
        distanceW *= distanceW;

        float distance = (float)((double)distanceH + (double)distanceW);

        //distance /= distance;
        return distance;
    }

    float CalcAStarHeuristic(float distanceFromStart, TileCell nextTilecell, TileCell targetTileCell)
    {
        return distanceFromStart + CalcComplexcHeuristic(nextTilecell, targetTileCell);
    }

}
