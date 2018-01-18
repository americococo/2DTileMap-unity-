using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eMoveDirection
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}




public class Character : MapObject
{

    protected GameObject _chracterView;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
     void Update()
    {
        
        //if (false == _isLive)
        //    return;

        
        _state.Update();
    }
    public void Init(string viewName)
    {
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);
        _chracterView = GameObject.Instantiate(characterViewPrefabs);
        _chracterView.transform.SetParent(transform);

        _chracterView.transform.localScale = Vector3.one;
        _chracterView.transform.localPosition = Vector3.zero;

        TileMap map = GameManger.Instance.GetMap();

        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);

        //TileCell tileCell = map.GetTileCell(x, y);
        //tileCell.AddObject(eTileLayer.MIIDDLE, this);

        map.SetObject(_tileX, _tileY, this, eTileLayer.MIIDDLE);

        InitState();
    }


    //state
    protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();
    protected State _state;

    void InitState()
    {
        {
            State state = new Idle();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new Move();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new Attack();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }
        {
            State state = new Damage();
            state.Init(this);
            _stateMap[eStateType.DAMAGE] = state;
        }
        {
            State state = new Death();
            state.Init(this);
            _stateMap[eStateType.DEATH] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _currentlayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        _chracterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _chracterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    protected int _tileX;
    protected int _tileY;

    public int GetTileX()
    {
        return _tileX;
    }
    public int GetTileY()
    {
        return _tileY;
    }

    eMoveDirection _nextDirection = eMoveDirection.NONE;
    public eMoveDirection GetNextDirection() { return _nextDirection; }
    public void SetNextDirection(eMoveDirection nextDirection) { _nextDirection = nextDirection; }


    //message
    override public void ReceiverObjcectMessage(ObjectMessageParam messageParam)
    {
        switch (messageParam.message)
        {
            case "Attack":
                _damagePoint = messageParam.attackpoint;
                Debug.Log("Damage: " + _hp);
                _state.NextState(eStateType.DAMAGE);
                break;
        }

    }

    //attack
    public void Attack(MapObject Ene)
    {
        ObjectMessageParam messageParam = new ObjectMessageParam();
        messageParam.sender = this;
        messageParam.receiver = Ene;
        messageParam.attackpoint = _attackPoint;
        messageParam.message = "Attack";

        messageSystem.Instance.Send(messageParam);
    }
    //attack
    protected int _attackPoint;
    protected int _damagePoint;

    public int GetDamagePoint()
    {
        return _damagePoint;
    }

    public void DecreaseHp(int damage)
    {
        _hp -= damage;
        if(0>= _hp)
        {
            _hp = 0;
            _isLive = false;
        }
    }

    //life
    protected int _hp = 200; // hp<0 -> Live(false)
    protected bool _isLive = true;


    public bool Islive()
    {
        return _isLive;
    }

    //State
    public void ChangeState(eStateType nextstate)
    {
        if (null != _state)
            _state.Stop();

        _state = _stateMap[nextstate];
        _state.Start();
    }

    public bool MoveStart(int tileX, int tileY)
    {
        string animationTrigger = "Up";

        switch (_nextDirection)
        {
            case eMoveDirection.LEFT:
                animationTrigger = "Left";
                break;
            case eMoveDirection.RIGHT:
                animationTrigger = "Right";
                break;
            case eMoveDirection.UP:
                animationTrigger = "Up";
                break;
            case eMoveDirection.DOWN:
                animationTrigger = "Down";
                break;
        }

        _chracterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManger.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(tileX, tileY);
        if (0 == collisionList.Count)
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = tileX;
            _tileY = tileY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIIDDLE);

            return true;
        }
        return false;
    }
}

