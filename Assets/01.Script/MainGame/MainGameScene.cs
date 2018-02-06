using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainGameScene : MonoBehaviour
{


    public MainGameUI GameUI;

    public TileMap _tileMap;
    //public Player _TestPlayer;


    void Start()
    {
        Init();
    }

    void Update()
    {
        messageSystem.Instance.ProcessMessage();
    }
    void Init()
    {

        _tileMap.Init();
        GameManger.Instance.SetMap(_tileMap);

        Character player = CreateCharacter("Player", "character01");
        player.SetCanMove(false);

        Character monster = CreateCharacter("Monster", "character02");
        monster.SetCanMove(false);

        

        GameManger.Instance.TargetCharacter = monster;
        player.BecomeViewr();

    }

    Character CreateCharacter(string fileName,string resourceName)
    {
        string filePath = "Prefabs/CharacterFrame/Character" ;
        GameObject charPrefabs = Resources.Load<GameObject>(filePath);
        GameObject charGameObject = GameObject.Instantiate(charPrefabs);
        charGameObject.transform.SetParent(_tileMap.transform);
        charGameObject.transform.localPosition = Vector3.zero;

        Character character = charGameObject.GetComponent<Player>();
        switch (fileName)
        {
            case "Player":
                character = charGameObject.AddComponent<Player>();
                break;
            case "Monster":
                //character = charGameObject.GetComponent<Monster>(); //프리펩과 뷰의 1대1 대응
                character = charGameObject.AddComponent<Monster>(); //프리펩컴퍼넌트를 뷰에 추가 다 대 1 대응
                break;
        }


        character.Init(resourceName);

        Slider hpGuage = GameUI.CreateHPSlider();
        character.LinkHPGuage(hpGuage);

        Slider AttackCoolGuage = GameUI.CreateAttackCoolSlider();
        character.LinkAttackCoolTimeGuage(AttackCoolGuage);

        Slider ExpGuage = GameUI.CreateLevelSlider();
        character.LinkExpGuage(ExpGuage);

        return character;
    }

}
