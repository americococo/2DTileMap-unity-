using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : State
{
    struct sPathCommand
    {
        public TileCell tileCell;
        public TileCell prevTileCell;
    }

    struct sPosition
    {
        public int x;
        public int y;
    }


    Queue<sPathCommand> _pathfindingQueue = new Queue<sPathCommand>();

    public override void Stop()
    {
        base.Stop();

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
            cmd.prevTileCell = null;
            cmd.tileCell = map.GetTileCell(_character.GetTileX(), _character.GetTileY());

            _pathfindingQueue.Enqueue(cmd);
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

        //큐가 빌때까지 큐에 있는 커맨드패스를 커내 검사
        if (0 != _pathfindingQueue.Count)
        {
            sPathCommand cmd = _pathfindingQueue.Dequeue();
            Debug.Log("1");
            //캐맨드 검사
            //(커맨드에 포함됀 타일셀이 방문하지 않는 타일셀인경우)
            if (false == cmd.tileCell.IsPathFindingMark())
            {
                //방문 처리
                cmd.tileCell.PathFindingMarking();
                Debug.Log("2");
                //목표 도달시 종료 Idle상태로 전환
                if (cmd.tileCell.GetTileX() == _character.getGoalTileCell().GetTileX() &&
                    cmd.tileCell.GetTileY() == _character.getGoalTileCell().GetTileY())
                {

                    Debug.Log("Finded");
                    _nextState = eStateType.IDLE;
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
                        float distance = cmd.tileCell.GetDistanceFromStart() + nextTileCell.GetDistanceWidght();
                        nextTileCell.SetDistanceFromStart(direction);
                       // nextTileCell.PathFindingMarking();
                        
                        sPathCommand newCmd;
                        newCmd.tileCell = nextTileCell;
                        newCmd.prevTileCell = cmd.tileCell;
                        _pathfindingQueue.Enqueue(newCmd);
                    }

                }

            }
        }
    }
    sPosition GetPositopmByDirection(sPosition position,int direction)
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
}
