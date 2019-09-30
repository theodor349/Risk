using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    public static PopUpController Instance;

    [SerializeField] private PopUpBattle battle;
    [SerializeField] private PopUpTransfer transfer;
    [SerializeField] private PopUpReinforce reinforce;

    private Action returnFuntion;

    private void Awake()
    {
        Instance = this;
    }

    // Battle
    public void StartBattle(Province atk, Province def, Action returnFuntion)
    {
        this.returnFuntion = returnFuntion;

        battle.gameObject.SetActive(true);
        battle.SetupBattle(atk, def);
    }
    public void EndBattle()
    {
        battle.gameObject.SetActive(false);
        returnFuntion?.Invoke();
    }

    // Transfer
    public void StartTransfer(Province from, Province to, Action returnFuntion)
    {
        this.returnFuntion = returnFuntion;

        transfer.gameObject.SetActive(true);
        transfer.SetupTransfer(from, to);
    }
    public void EndTransfer()
    {
        transfer.gameObject.SetActive(false);
        returnFuntion?.Invoke();
    }

    // Reinforce
    public void StartReinforce(Province to, Action returnFuntion)
    {
        this.returnFuntion = returnFuntion;

        reinforce.gameObject.SetActive(true);
        reinforce.SetupReinforce(to);
    }
    public void EndReinforce()
    {
        reinforce.gameObject.SetActive(false);
        returnFuntion?.Invoke();
    }
}
