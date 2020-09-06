using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public GameObject tankPrefab;
    public GameObject hpIndicatorPrefab;
    void Start()
    {
        var leftPlayer = InstantiateTank(new Vector3(-1449, 0), Skin.Purple);
        leftPlayer.GetComponent<PlayerInput>().ActivateInput();
        leftPlayer.GetComponent<PlayerInput>().SwitchCurrentActionMap("LeftPlayer");

        var rightPlayer = InstantiateTank(new Vector3(1449, 0), Skin.Yellow);
        rightPlayer.GetComponent<PlayerInput>().ActivateInput();
        rightPlayer.GetComponent<PlayerInput>().SwitchCurrentActionMap("RightPlayer");

        hpIndicatorPrefab.transform.position = new Vector3(-1540, 800);
        var leftIndicator = Instantiate(hpIndicatorPrefab);
        hpIndicatorPrefab.transform.position = new Vector3(1540, 800);
        var rightIndicator = Instantiate(hpIndicatorPrefab);

        var leftPlayerTankScript = leftPlayer.GetComponent<Tank>();
        var rightPlayerTankScript = rightPlayer.GetComponent<Tank>();

        var leftIndicatorScript = leftIndicator.GetComponent<HpIndicator>();
        var rightIndicatorScript = rightIndicator.GetComponent<HpIndicator>();

        leftPlayerTankScript.HpChanged += leftIndicatorScript.ChangeHp;
        rightPlayerTankScript.HpChanged += rightIndicatorScript.ChangeHp;

        leftIndicatorScript.SetHeartColor(Resources.Load<Sprite>("Heart_Filled_Purple"));
        rightIndicatorScript.SetHeartColor(Resources.Load<Sprite>("Heart_Filled_Yellow"));
    }

    void Update()
    {

    }

    private GameObject InstantiateTank(Vector3 position, Skin skin)
    {
        tankPrefab.transform.position = position;
        var tank = Instantiate(tankPrefab);
        tank.SendMessage("SetSkin", skin);
        return tank;
    }
}
