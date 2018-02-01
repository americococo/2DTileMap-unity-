﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eMoveDirection
{
    
    LEFT,
    RIGHT,
    UP,
    DOWN,
    NONE
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
        if (eStateType.NONE != _state.GetNextState())
            ChangeState(_state.GetNextState());


        _state.Update();

        UpdateAttackCoolTime();
        
        UpdateUI();

    }

    TileCell _goalTIleCell;

    public void SetGoalTileCell(TileCell selectTilecell)
    {
        _goalTIleCell = selectTilecell;
    }
    public TileCell getGoalTileCell()
    {
        return _goalTIleCell;
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

    virtual protected void InitState()
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
        {
            State state = new Battle();
            state.Init(this);
            _stateMap[eStateType.BATTLE] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }

    public TileCell GetTileCell()
    {
        return GameManger.Instance.GetMap().GetTileCell(_tileX, _tileY);
    }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _currentlayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        _chracterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _chracterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

  

    eMoveDirection _nextDirection = eMoveDirection.NONE;
    public eMoveDirection GetNextDirection() { return _nextDirection; }
    public void SetNextDirection(eMoveDirection nextDirection) { _nextDirection = nextDirection; }

    public eMoveDirection GetDirection(sPosition to, sPosition cur)
    {
        eMoveDirection directionbe = eMoveDirection.NONE;

        if (cur.x < to.x)
            directionbe = eMoveDirection.RIGHT;
        if (cur.x > to.x)
            directionbe = eMoveDirection.LEFT;
        if (cur.y < to.y)
            directionbe = eMoveDirection.UP;
        if (cur.y > to.y)
            directionbe = eMoveDirection.DOWN;

        return directionbe;
    }

    //message
    override public void ReceiverObjcectMessage(ObjectMessageParam messageParam)
    {
        switch (messageParam.message)
        {
            case "Attack":
                _damagePoint = messageParam.attackpoint;
                _state.NextState(eStateType.DAMAGE);


                sPosition senderTile;
                senderTile.x = messageParam.sender.GetTileX();
                senderTile.y = messageParam.sender.GetTileY();

                sPosition myTile;
                myTile.x = _tileX;
                myTile.y = _tileY;

                SetNextDirection(GetDirection(senderTile, myTile)) ;
                _chracterView.GetComponent<Animator>().SetTrigger(GetNextDirection().ToString());

                //Debug.Log("character " + );
                break;
        }

    }

    public State getstateType()
    {
        return _state;
    }


    //attack
    public void Attack(MapObject Ene)
    {
        ResetCoolTime();

        SoundPlayer.Instance.PlayEffect("player_hit");

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

    float _attackCoolTime = 0.5f;
    float _deltaAttackCoolTime = 0.0f;

    void UpdateAttackCoolTime()
    {
        if (_deltaAttackCoolTime <= _attackCoolTime)
            _deltaAttackCoolTime += Time.deltaTime;
        else
            _deltaAttackCoolTime = _attackCoolTime;
    }

    public bool IsAttackAble()
    {
        if (_attackCoolTime <= _deltaAttackCoolTime)
            return true;
        return false;

    }

    void ResetCoolTime()
    {
        _deltaAttackCoolTime = 0.0f;
    }


    public int GetDamagePoint()
    {
        return _damagePoint;
    }

    public void DecreaseHp(int damage)
    {
        string filePath = "Prefabs/Effect/DamageEffect";
        GameObject effcetPrefabs = Resources.Load<GameObject>(filePath);
        GameObject effctObject = GameObject.Instantiate(effcetPrefabs, transform.position, Quaternion.identity);

        GameObject.Destroy(effctObject, 1.2f);

        _chracterView.GetComponent<SpriteRenderer>().color = Color.red;

        Invoke("ResetColor", 0.1f);//0.1초 후 ResetColor 함수 호출

        _hp -= damage;
        if (0 >= _hp)
        {
            _hp = 0;
            _isLive = false;
        }
    }

    void ResetColor()
    {
        _chracterView.GetComponent<SpriteRenderer>().color = Color.white;
    }

    //life
    protected int _hp = 200; // hp<0 -> Live(false)
    protected bool _isLive = true;


    public bool Islive()
    {
        return _isLive;
    }

    //State
    private void ChangeState(eStateType nextstate)
    {
        if (null != _state)
            _state.Stop();

        _state = _stateMap[nextstate];
        _state.Start();
    }

    public bool MoveStart(int tileX, int tileY)
    {
        string animationTrigger = "UP";

        switch (_nextDirection)
        {
            case eMoveDirection.LEFT:
                animationTrigger = "LEFT";
                break;
            case eMoveDirection.RIGHT:
                animationTrigger = "RIGHT";
                break;
            case eMoveDirection.UP:
                animationTrigger = "UP";
                break;
            case eMoveDirection.DOWN:
                animationTrigger = "DOWN";
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

    Stack<TileCell> serchRoot = new Stack<TileCell>();

    public bool IsEmptyPathfindingTileCell()
    {
        if (0 != serchRoot.Count)
            return false;
        else
            return true;
    }
        
    public TileCell PopPathfindingTileCell()
    {
        return serchRoot.Pop();   
    }

    public void PushPathfindingTileCell(TileCell pathfindingTileCell)
    {
        serchRoot.Push(pathfindingTileCell);
    }

    public void clearPathfindingTileCell()
    {
        serchRoot.Clear();
    }

    //UI
    Slider _hpGuage;
    Slider _attackcoolTimeGuage;

    void UpdateUI()
    {
        _hpGuage.value = _hp / 100.0f;
        _attackcoolTimeGuage.value = _deltaAttackCoolTime * 2.0f;
    }

    public void LinkHPGuage(Slider HpSlider)
    {
        LinkGuage(HpSlider, Vector3.zero);
        _hpGuage = HpSlider;
    }

    public void LinkAttackCoolTimeGuage(Slider AttackCoolTimeSlider)
    {
        LinkGuage(AttackCoolTimeSlider, new Vector3(0, -0.3f, 0));
        _attackcoolTimeGuage = AttackCoolTimeSlider;
    }
    public void LinkGuage(Slider slider, Vector3 postion)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        slider.transform.SetParent(canvasObject.transform);
        slider.transform.localPosition = postion;
        slider.transform.localScale = Vector3.one;

    }

}

