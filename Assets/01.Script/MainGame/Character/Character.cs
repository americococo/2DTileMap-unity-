using System.Collections;
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


public struct sPosition
{
    public int tileX;
    public int tileY;
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

        UpdateAttackCoolTime();
        UpdateUI();


    }

    TileCell _goalTIleCell = null;

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
            State state = new WAR();
            state.Init(this);
            _stateMap[eStateType.WAR] = state;
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

    public eMoveDirection getMoveDirection(sPosition CurPostion, sPosition toPostion)
    {
        eMoveDirection signalDirection = eMoveDirection.NONE;

        if (CurPostion.tileX > toPostion.tileX)
            signalDirection = eMoveDirection.LEFT;
        if (toPostion.tileX > CurPostion.tileX)
            signalDirection = eMoveDirection.RIGHT;

        if (CurPostion.tileY > toPostion.tileY)
            signalDirection = eMoveDirection.DOWN;

        if (toPostion.tileY > CurPostion.tileY)
            signalDirection = eMoveDirection.UP;

        return signalDirection;
    }
    public void SetMoveDireCtion(eMoveDirection direction)
    {
        _chracterView.GetComponent<Animator>().SetTrigger(direction.ToString());
    }

    //message
    override public void ReceiverObjcectMessage(ObjectMessageParam messageParam)
    {
        switch (messageParam.message)
        {
            case "ATTACK":
                _damagePoint = messageParam.attackpoint;

                sPosition curPosition;
                curPosition.tileX = messageParam.receiver.GetTileX();
                curPosition.tileY = messageParam.receiver.GetTileY();

                sPosition targetPosition;
                targetPosition.tileX = messageParam.sender.GetTileX();
                targetPosition.tileY = messageParam.sender.GetTileY();

                eMoveDirection moveDirection = getMoveDirection(curPosition, targetPosition);

                SetNextDirection(moveDirection);

                _chracterView.GetComponent<Animator>().SetTrigger(moveDirection.ToString());
                //Debug.Log("Damage: " + _hp);
                _state.NextState(eStateType.DAMAGE);
                //Debug.Log(messageParam.receiver);
                break;
        }

    }


    //attack
    public MapObject Attack()
    {
        ResetCoolTime();

        SoundPlayer.Instance.PlayEffect("player_hit");
        ObjectMessageParam messageParam = new ObjectMessageParam();

        int moveX = _tileX;
        int moveY = _tileY;

        switch (GetNextDirection())
        {
            case eMoveDirection.LEFT:
                moveX--;
                break;
            case eMoveDirection.RIGHT:
                moveX++;
                break;
            case eMoveDirection.UP:
                moveY++;
                break;
            case eMoveDirection.DOWN:
                moveY--;
                break;
        }

        TileMap map = GameManger.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
        for (int i = 0; i < collisionList.Count; i++)
        {
            switch (collisionList[i].GetObjectType())
            {
                case eMapObjectType.CHARACTER:
                    messageParam.receiver = collisionList[i];
                    break;
                
            }
        }

        messageParam.sender = this;
        messageParam.attackpoint = _attackPoint;
        messageParam.message = "ATTACK";

        messageSystem.Instance.Send(messageParam);

        return messageParam.receiver;
    }

    //attack
    protected int _attackPoint;
    protected int _damagePoint;

    float _attackCoolTime = 1.0f;
    float _deltaAttackCoolTime = 0.0f;

    public void UpdateAttackCoolTime()
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


    public int getHp()
    {
        return _hp;
    }

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

    //Pathfind
    Stack<TileCell> serchRoot = new Stack<TileCell>();

    public void pushPathfindingTileCell(TileCell Rootcell)
    {
        serchRoot.Push(Rootcell);
    }

    public void resetSerchRoot()
    {
        serchRoot.Clear();
    }

    public bool IsEmptyPathFindingTileCell()
    {
        if (0 == serchRoot.Count)
            return true;
        else
            return false;
    }
    public TileCell popPathFindingTileCell()
    {
        return serchRoot.Pop();
    }

    //UI
    Slider _hpGuage;
    Slider _attackcoolTimeGuage;

    void UpdateUI()
    {
        _hpGuage.value = _hp / 200.0f;
        _attackcoolTimeGuage.value = _deltaAttackCoolTime / _attackCoolTime;
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

    public void setMoveCursor(Vector2 position)
    {
        string filePath = "Prefabs/Effect/MoveCursor";
        GameObject effcetPrefabs = Resources.Load<GameObject>(filePath);
        GameObject effctObject = GameObject.Instantiate(effcetPrefabs, position, Quaternion.identity);
        effctObject.transform.localPosition = position;
        GameObject.Destroy(effctObject, 2.0f);
    }

}

