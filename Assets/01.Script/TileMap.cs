using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        _sprityArray = Resources.LoadAll<Sprite>("Sprites/MapSprite");
        CreateTiles();
    }

    Sprite[] _sprityArray;
    void Update()
    {

    }

    //Tile

    public GameObject TileObjectPrefabs;

    List<TileCell> _tileCellList = new List<TileCell>();

    int _width;
    int _height;

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

        for(int y=0;y<_height;y++)
        {
            int line = y + 2;
            string[] Token = records[line].Split(',');
            //Debug.Log(records[line]);
            for(int x=0;x<_width;x++)
            {
                int spriteIndex = int.Parse(Token[x]);

                GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                tileGameObject.transform.SetParent(transform);
                tileGameObject.transform.localScale = Vector3.one;
                tileGameObject.transform.localPosition = Vector3.zero;

                TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                tileObject.Init(_sprityArray[spriteIndex]);
                // tileObject.SetPosition(x*tileSize/100.0f, y*tileSize/100.0f);

                TileCell tileCell = new TileCell();
                tileCell.Init();
                tileCell.SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);
                tileCell.AddObject(eTileLayer.GROUND, tileObject);
                _tileCellList.Add(tileCell);
            }
        }


        //2층
        scriptAsset = Resources.Load<TextAsset>("Data/Map1MapData_layer2");
        records = scriptAsset.text.Split('\n');    //레코드 받아옴
        for(int y=0;y<_height;y++)
        {
            int line = y + 2;
            string[] Token = records[line].Split(',');
            for (int x=0;x<_width;x++)
            {
                int spriteIndex = int.Parse(Token[x]);
                if(0<=spriteIndex)
                {
                    GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                    tileGameObject.transform.SetParent(transform);
                    tileGameObject.transform.localScale = Vector3.one;
                    tileGameObject.transform.localPosition = Vector3.zero;

                    TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                    tileObject.Init(_sprityArray[spriteIndex]);
                    // tileObject.SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);

                    int cellIndex = (y * _width) + x;
                    _tileCellList[cellIndex].AddObject(eTileLayer.GROUND, tileObject);
                }

            }
        }

        //for(int i=0; i< _sprityArray.Length;i++)
        //{
        //    float x = ((i % 16) * tileSize) / 100.0f;
        //    float y = -(i / 16) * tileSize / 100.0f;//값이 클수록 위로 올라가는 좌표

        //    GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
        //    tileGameObject.transform.SetParent(transform);
        //    tileGameObject.transform.localScale = Vector3.one;
        //    tileGameObject.transform.localPosition = Vector3.zero;

        //    TileObject tileObject = tileGameObject.GetComponent<TileObject>();
        //    tileObject.Init(_sprityArray[i]);
        //    tileObject.SetPosition(x, y);
        //}

    }

}
