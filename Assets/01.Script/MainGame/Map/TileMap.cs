using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    Sprite[] _sprityArray;
    public void Init()
    {
        _sprityArray = Resources.LoadAll<Sprite>("Sprites/MapSprite");
        //CreateTiles();
        CreateRandomMaze();
    }

    void Update()
    {

    }

    //Tile

    public GameObject TileObjectPrefabs;

    TileCell[,] _tileCellList;

    int _width;
    int _height;

    void CreateRandomMaze()
    {
        float tileSize = 32.0f;


        //1층
        TextAsset scriptAsset = Resources.Load<TextAsset>("Data/Map1MapData_layer1");
        string[] records = scriptAsset.text.Split('\n');    //레코드 받아옴

        {
            string[] Token = records[0].Split(','); //레코드 토큰으로 쪼갬
            _width = int.Parse(Token[1]);
            _height = int.Parse(Token[2]);

        }
        _tileCellList = new TileCell[_height, _width];

        for (int y = 0; y < _height; y++)
        {
            int line = y + 2;
            string[] Token = records[line].Split(',');
            for (int x = 0; x < _width; x++)
            {
                int spriteIndex = int.Parse(Token[x]);

                GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                tileGameObject.transform.SetParent(transform);
                tileGameObject.transform.localScale = Vector3.one;
                tileGameObject.transform.localPosition = Vector3.zero;

                TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                tileObject.Init(_sprityArray[spriteIndex]);
                tileObject.setTilePostion(x, y);
                //TileCell tileCell = new TileCell();
                //tileCell.Init();
                //tileCell.SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);
                //tileCell.AddObject(eTileLayer.GROUND, tileObject);
                //_tileCellList.Add(tileCell);

                _tileCellList[y, x] = new TileCell();
                GetTileCell(x, y).Init();
                GetTileCell(x, y).SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);
                GetTileCell(x, y).SetTilePosition(new Vector2(x, y));
                GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
            }
        }


        //2층
        //scriptAsset = Resources.Load<TextAsset>("Data/Map1MapData_layer2");
        //records = scriptAsset.text.Split('\n');    //레코드 받아옴
        //for (int y = 0; y < _height; y++)
        //{
        //    int line = y + 2;
        //    string[] Token = records[line].Split(',');
        //    for (int x = 0; x < _width; x++)
        //    {
        //        int spriteIndex = int.Parse(Token[x]);
        //        if (0 < spriteIndex)
        //        {
        //            GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
        //            tileGameObject.transform.SetParent(transform);
        //            tileGameObject.transform.localScale = Vector3.one;
        //            tileGameObject.transform.localPosition = Vector3.zero;

        //            TileObject tileObject = tileGameObject.GetComponent<TileObject>();
        //            tileObject.Init(_sprityArray[spriteIndex]);
        //            tileObject.SetCanMove(false);
        //            GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
        //        }

        //    }
        //}

        for (int y = 0; y < _height; y++)
        {
            if (0 == (y % 2))
            {
                for (int x = 0; x < _width; x++)
                {
                    if (0 == (x % 2))
                    {
                        int spriteIndex = 63;

                        GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                        tileGameObject.transform.SetParent(transform);
                        tileGameObject.transform.localScale = Vector3.one;
                        tileGameObject.transform.localPosition = Vector3.zero;

                        TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                        tileObject.Init(_sprityArray[spriteIndex]);
                        tileObject.SetCanMove(false);
                        tileObject.setTilePostion(x, y);
                        GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
                    }
                }
            }

        }
        eMoveDirection direction = (eMoveDirection)Random.Range(0, (int)eMoveDirection.NONE - 1);
        //기둥 세운후 가지치기
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if(false==GetTileCell(x,y).CanMove())
                {
                    //연결돼지 않은 블록인 경우
                    if(false == IsConnectedCell(x,y))
                    {
                        //램덤한 방향으로 블럭이 연결됄때까지 이어준다

                        direction++;
                        if (direction == eMoveDirection.NONE)
                            direction = eMoveDirection.LEFT;

                        int serchTileX = x;
                        int serchTileY = y;
                        while(false == IsConnectedCell(serchTileX,serchTileY))
                        {
                            switch (direction)
                            {
                                case eMoveDirection.LEFT:
                                    serchTileX--;
                                    break;
                                case eMoveDirection.RIGHT:
                                    serchTileX++;
                                    break;
                                case eMoveDirection.UP:
                                    serchTileY++;
                                    break;
                                case eMoveDirection.DOWN:
                                    serchTileY--;
                                    break;
                            }
                            if(0<=serchTileX && serchTileX<_width && 0<= serchTileY && serchTileY < _height)
                            {
                                //새로운 블록을 심어줌
                                int spriteIndex = 63;

                                GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                                tileGameObject.transform.SetParent(transform);
                                tileGameObject.transform.localScale = Vector3.one;
                                tileGameObject.transform.localPosition = Vector3.zero;

                                TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                                tileObject.Init(_sprityArray[spriteIndex]);
                                tileObject.SetCanMove(false);
                                tileObject.setTilePostion(serchTileX,serchTileY);
                                GetTileCell(serchTileX, serchTileY).AddObject(eTileLayer.GROUND, tileObject);
                            }
                        }
                    }
                }
            }

        }


    }

    bool IsConnectedCell(int tileX,int tileY)
    {
        //주변에 하나라도 붙은 블럭이 있으면 연굘됀 블럭
        for(int direction = (int)eMoveDirection.LEFT; direction <(int)eMoveDirection.NONE;direction++)
        {
            int serchTileX = tileX;
            int serchTileY = tileY;

            switch ((eMoveDirection)direction)
            {
                case eMoveDirection.LEFT:
                    serchTileX--;
                    break;
                case eMoveDirection.RIGHT:
                    serchTileX++;
                    break;
                case eMoveDirection.UP:
                    serchTileY++;
                    break;
                case eMoveDirection.DOWN:
                    serchTileY--;
                    break;
            }
            if (0 <= serchTileX && serchTileX < _width && 0 <= serchTileY && serchTileY < _height)
            {
                if (false == GetTileCell(serchTileX, serchTileY).CanMove())
                    return true;
            }
            else
            {
                return true;
            }
        }


        return false;

    }

    void CreateTiles()
    {
        float tileSize = 32.0f;


        //1층
        TextAsset scriptAsset = Resources.Load<TextAsset>("Data/Map1MapData_layer1");
        string[] records = scriptAsset.text.Split('\n');    //레코드 받아옴

        {
            string[] Token = records[0].Split(','); //레코드 토큰으로 쪼갬
            _width = int.Parse(Token[1]);
            _height = int.Parse(Token[2]);

        }
        _tileCellList = new TileCell[_height, _width];

        for (int y = 0; y < _height; y++)
        {
            int line = y + 2;
            string[] Token = records[line].Split(',');
            for (int x = 0; x < _width; x++)
            {
                int spriteIndex = int.Parse(Token[x]);

                GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                tileGameObject.transform.SetParent(transform);
                tileGameObject.transform.localScale = Vector3.one;
                tileGameObject.transform.localPosition = Vector3.zero;

                TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                tileObject.Init(_sprityArray[spriteIndex]);
                tileObject.setTilePostion(x, y);
                //TileCell tileCell = new TileCell();
                //tileCell.Init();
                //tileCell.SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);
                //tileCell.AddObject(eTileLayer.GROUND, tileObject);
                //_tileCellList.Add(tileCell);

                _tileCellList[y, x] = new TileCell();
                GetTileCell(x, y).Init();
                GetTileCell(x, y).SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);
                GetTileCell(x, y).SetTilePosition(new Vector2(x, y));
                GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
            }
        }


        //2층
        scriptAsset = Resources.Load<TextAsset>("Data/Map1MapData_layer2");
        records = scriptAsset.text.Split('\n');    //레코드 받아옴
        for (int y = 0; y < _height; y++)
        {
            int line = y + 2;
            string[] Token = records[line].Split(',');
            for (int x = 0; x < _width; x++)
            {
                int spriteIndex = int.Parse(Token[x]);
                if (0 < spriteIndex)
                {
                    GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                    tileGameObject.transform.SetParent(transform);
                    tileGameObject.transform.localScale = Vector3.one;
                    tileGameObject.transform.localPosition = Vector3.zero;

                    TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                    tileObject.Init(_sprityArray[spriteIndex]);
                    tileObject.SetCanMove(false);
                    GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
                }

            }
        }

    }


    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
    public TileCell GetTileCell(int x, int y)
    {
        if (0 <= x && x < _width && 0 <= y && y < _height)
            return _tileCellList[y, x];
        else
            return null;
    }

    //타일 이동 
    public bool CanMoveTile(int tileX, int tileY)
    {
        if (tileX < 0 || _width <= tileX)
            return false;
        if (tileY < 0 || _height <= tileY)
            return false;

        TileCell tileCell = GetTileCell(tileX, tileY);

        return tileCell.CanMove();

    }

    public List<MapObject> GetCollisionList(int tileX, int tileY)
    {
        if (tileX < 0 || _width <= tileX)
            return null;
        if (tileY < 0 || _height <= tileY)
            return null;

        TileCell tileCell = GetTileCell(tileX, tileY);

        return tileCell.GetCollsionList();
    }

    public void ResetObject(int tileX, int tileY, MapObject tileObject)
    {
        TileCell tileCell = GetTileCell(tileX, tileY);
        tileCell.RemoveObject(tileObject);
    }

    public void SetObject(int tileX, int tileY, MapObject tileObject, eTileLayer tileLayer)
    {
        TileCell tileCell = GetTileCell(tileX, tileY);
        tileCell.AddObject(tileLayer, tileObject);
    }
    public void ResetPathfinding()
    {
        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                _tileCellList[y, x].ResetPathfinding();
    }
}
