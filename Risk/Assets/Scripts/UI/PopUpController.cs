using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    public static PopUpController Instance;

    [SerializeField] private PopUpBattle battle;
    private Action returnFuntion;

    private void Awake()
    {
        Instance = this;
    }

    public void StartBattle(Province atk, Province def, Action returnFuntion)
    {
        this.returnFuntion = returnFuntion;

        battle.gameObject.SetActive(true);
        battle.SetupBattle(atk, def);
    }

    public void BattleDone()
    {
        battle.gameObject.SetActive(false);
        returnFuntion?.Invoke();
    }
}
