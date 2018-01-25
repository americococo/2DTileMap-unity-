using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : State
{
    struct sPathCommand
    {
        public TileCell tileCell;
        //public TileCell prevTileCell;
        public float heuristic;
    }
    //타일셀로 

    struct sPosition
    {
        public int x;
        public int y;
    }

    enum eUpdateState
    {
        PATHFINDING,
        BUILDPATH
    }

    eUpdateState _updateState = eUpdateState.PATHFINDING;

    List<sPathCommand> _pathfindingQueue = new List<sPathCommand>();

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

            // _pathfindingQueue.Add(cmd);
            PushCommand(cmd);
        }
        else
            _nextState = eStateType.IDLE;

    }

    // Update is called once per frame
    public override void Update()
    {
        if (_nextState != eStateType.NONE)
        {
            _character.ChangeState(_nextState);

        }

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

    void UpdatePathfinding()
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

                    if (nextTileCell.CanMove() && false == nextTileCell.IsPathFindingMark())//이동 가능하며 탐색안한 타일만 큐에 넣엉줌
                    {
                        float distanceFromStart = cmd.tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceWidght();
                        if(null == nextTileCell.GetPrevfindingCell())
                        {
                            nextTileCell.SetDistanceFromStart(distanceFromStart);
                            nextTileCell.SetPrevPathfindingCell(cmd.tileCell);
                            sPathCommand newCmd;
                            newCmd.tileCell = nextTileCell;
                            newCmd.heuristic = distanceFromStart;
                            // _pathfindingQueue.Enqueue(nextTileCell);
                            //_pathfindingQueue.Add(newCmd);
                            PushCommand(newCmd);
                        }

                        else
                        {
                            if(distanceFromStart < nextTileCell.GetDistanceFromStart())
                            {
                                nextTileCell.SetDistanceFromStart(direction);
                                // nextTileCell.PathFindingMarking();
                                nextTileCell.SetPrevPathfindingCell(cmd.tileCell);

                                sPathCommand newCmd;
                                newCmd.tileCell = nextTileCell;
                                newCmd.heuristic = distanceFromStart;
                                //_pathfindingQueue.Enqueue(nextTileCell);
                                _pathfindingQueue.Add(newCmd);
                                PushCommand(newCmd);
                            }
                        }
                        
                    }
                    
                }

            }
        }
    }


    TileCell _reverseTileCell = null;

    void UpdateBuildPath()
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

    void PushCommand(sPathCommand command)
    {
        _pathfindingQueue.Add(command);

        //sorting
        _pathfindingQueue.Sort();
    }
}
