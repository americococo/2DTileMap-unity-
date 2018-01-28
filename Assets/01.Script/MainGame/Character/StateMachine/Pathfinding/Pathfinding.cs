using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct sPosition
{
    public int tileX;
    public int tileY;
}

public class Pathfinding : State
{


    protected struct sPathCommand
    {
        public TileCell tileCell;
        public float heuristic;
    }


    protected enum ePathState
    {
        PATH,
        BUILD,
    }
    
    protected List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();

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

            sPathCommand cmd;
            cmd.tileCell = _character.GetTileCell();
            cmd.heuristic = 0.0f;
            PushCommand(cmd);
        }
    }

    protected ePathState _pathState;

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

    virtual protected  void PathUpdate()
    {
        //큐가 빌때까지 큐에 있는 커맨드패스를 커내 검사
        if (0 != _pathfindingQueue.Count)
        {
            sPathCommand cmd;
            cmd = _pathfindingQueue[0];
            _pathfindingQueue.RemoveAt(0);

            //캐맨드 검사
            //(커맨드에 포함됀 타일셀이 방문하지 않는 타일셀인경우)
            if (false == cmd.tileCell.IsPathFindingMark())
            {
                //방문 처리
                cmd.tileCell.SetPathFindingMark();

                //목표 도달시 종료 Idle상태로 전환
                if (cmd.tileCell == _character.getGoalTileCell())
                {

                    Debug.Log("Finded");
                    _pathState = ePathState.BUILD;
                    _reverce = cmd.tileCell;
                    return;
                }

                for (int direction = (int)eMoveDirection.LEFT; direction < (int)eMoveDirection.NONE; direction++)
                {
                    sPosition position;
                    position.tileX = cmd.tileCell.GetTileX();
                    position.tileY = cmd.tileCell.GetTileY();
                    sPosition nextPosition = GetPositopmByDirection(position, (eMoveDirection)direction);
                    //tilecell를 찾기위해서 있는 좌표값

                    if ((nextPosition.tileX >= 0) && (GameManger.Instance.GetMap().GetWidth() > nextPosition.tileX) &&
                   (nextPosition.tileY >= 0) && (GameManger.Instance.GetMap().GetHeight() > nextPosition.tileY))
                    {
                        TileCell nextTileCell = GameManger.Instance.GetMap().GetTileCell(nextPosition.tileX, nextPosition.tileY);

                        if (nextTileCell.CanMove()&& false == nextTileCell.IsPathFindingMark()|| nextTileCell==_character.getGoalTileCell())//이동 가능하며 탐색안한 타일만 큐에 넣엉줌
                        {

                            float distanceFromStart = cmd.tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceWidght();
                            // float heuristic = CalcSimpleHeuristic(cmd.tileCell, nextTileCell, _character.getGoalTileCell());
                            float heuristic = CalcAstarHeuristic(distanceFromStart,nextTileCell, _character.getGoalTileCell());
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
                               if (heuristic < nextTileCell.Getheuristic())
                                {
                                  nextTileCell.SetDistanceFromStart(distanceFromStart);
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
    }

    protected TileCell _reverce = null;

    protected void BuildUpdate()
    {
        if (null != _reverce)
        {
            _character.pushPathfindingTileCell(_reverce);
            _reverce.ColorBackUp();
            _reverce = _reverce.GetPrevfindingCell();
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

    void PushCommand(sPathCommand command)
    {
        _pathfindingQueue.Add(command);

        //sorting
        _pathfindingQueue.Sort(delegate (sPathCommand a, sPathCommand b)
        {
            if (a.heuristic > b.heuristic) return 1;
            if (a.heuristic < b.heuristic) return -1;
            else return 0;
        });
    }

    float CalcSimpleHeuristic(TileCell tileCell, TileCell nextTileCell, TileCell targetTileCell)
    {
        float heuristic = 0.0f;

        int diffFromCurrent;
        int diffFromNext;

        // x 축
        {
            // 현재 타일부터 목표 까지의 거리를 구한다.
            diffFromCurrent = tileCell.GetTileX() - targetTileCell.GetTileX();
            if (diffFromCurrent < 0)
                diffFromCurrent = -diffFromCurrent;

            // 검사할 타일 부터 목표 까지의 거리를 구한다.
            diffFromNext = nextTileCell.GetTileX() - targetTileCell.GetTileX();
            if (diffFromNext < 0)
                diffFromNext = -diffFromNext;

            if (diffFromNext < diffFromCurrent)     // 검사할 타일이 현재 타일보다 목표와 더 가까우면
            {
                heuristic -= 1.0f;
            }
            else if (diffFromCurrent < diffFromNext)    // 현재 타일이 검사할 타일보다 목표와 더 가까우면
            {
                heuristic += 1.0f;
            }
        }

        // y 축
        {
            // 현재 타일부터 목표 까지의 거리를 구한다.
            diffFromCurrent = tileCell.GetTileY() - targetTileCell.GetTileY();
            if (diffFromCurrent < 0)
                diffFromCurrent = -diffFromCurrent;

            // 검사할 타일 부터 목표 까지의 거리를 구한다.
            diffFromNext = nextTileCell.GetTileY() - targetTileCell.GetTileY();
            if (diffFromNext < 0)
                diffFromNext = -diffFromNext;

            if (diffFromNext < diffFromCurrent)     // 검사할 타일이 현재 타일보다 목표와 더 가까우면
            {
                heuristic -= 1.0f;
            }
            else if (diffFromCurrent < diffFromNext)    // 현재 타일이 검사할 타일보다 목표와 더 가까우면
            {
                heuristic += 1.0f;
            }
        }

        return heuristic;
    }
    float CalccomplexHeuristic(TileCell nextTileCell, TileCell targetTileCell)
    {

        float distanceW = targetTileCell.GetTileX() - nextTileCell.GetTileX();
        float distanceH = targetTileCell.GetTileY() - nextTileCell.GetTileY();

        distanceH = Mathf.Abs(distanceH);
        distanceW = Mathf.Abs(distanceW);

        float distance;
        distance = distanceH + distanceW;
        distance = Mathf.Sqrt(distance);

        return distance;
    }
    float CalcAstarHeuristic(float distanceFromStart,TileCell nextTileCell, TileCell targetTileCell)
    {
        return distanceFromStart + CalccomplexHeuristic(nextTileCell, targetTileCell);
    }
}
