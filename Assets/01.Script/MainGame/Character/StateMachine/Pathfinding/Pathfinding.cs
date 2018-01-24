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


    Queue<sPathCommand> _pathfindingQueue = new Queue<sPathCommand>();


    // Use this for initialization
    public override void Start()
    {
        base.Start();

        //시작타일을 큐에 넣는다.



        //길찾기 변수 초기화
        //시작지점 sPathCommand 만들어서 큐에 넣는다.

        //목표 타일 가져옴

        TileMap map = GameManger.Instance.GetMap();
        map.ResetPathfinding();

        sPathCommand startCmd;
        startCmd.tileCell = _character.GetTileCell();
        startCmd.prevTileCell = null;

        _pathfindingQueue.Enqueue(startCmd);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //큐가 빌때까지 큐에 있는 커맨드패스를 커내 검사
        if (0 != _pathfindingQueue.Count)
        {
            sPathCommand cmd = _pathfindingQueue.Dequeue();

            //캐맨드 검사
            //(커맨드에 포함됀 타일셀이 방문하지 않는 타일셀인경우)
            if (true != cmd.tileCell.IsPathFindingMark())
            {
                //방문 처리
                cmd.tileCell.SetPathFindingMark();

                //목표 도달시 종료 Idle상태로 전환
                if (cmd.tileCell != _character.getGoalTileCell())
                {
                    int moveX=(int)cmd.tileCell.GetPosition().x;
                    int moveY=(int) cmd.tileCell.GetPosition().y;

                    //4방향 검사
                    for (int i = 0; i < (int)eMoveDirection.NONE; i++)
                    {
                        switch ((eMoveDirection)i)
                        {
                            case eMoveDirection.LEFT:
                                moveX-=32;
                                break;
                            case eMoveDirection.RIGHT:
                                moveX+=32;
                                break;
                            case eMoveDirection.UP:
                                moveY+=32;
                                break;
                            case eMoveDirection.DOWN:
                                moveY-=32;
                                break;
                        }

                        TileCell tileCell = GameManger.Instance.GetMap().GetTileCell(moveX / 32, moveY / 32);

                        if(true==tileCell.CanMove()&& tileCell.IsPathFindingMark())
                        {
                            tileCell.SetPathFindingMark();
                            sPathCommand newTileCmd;
                            newTileCmd.prevTileCell = cmd.tileCell;
                            newTileCmd.tileCell = tileCell;

                            _pathfindingQueue.Enqueue(newTileCmd);
                        }
                        //각 방향별 타일셀 도출
                        //canmove true && 방문 처리 안됀 타일
                        //거리 값계산 (Hyrist)
                        //새로운  커맨드를 만들어 큐에 넣어줌
                        //이전 타일 세팅해줌
                        //큐에 넣어줌
                        //방향에 따라 찾은 타일 거리값 갱신 (hyrist)
                    }
                }
                else//목표 도달 했으므로 IDle 봔환
                    _nextState = eStateType.IDLE;
            }
        }
    }

}
