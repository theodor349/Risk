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
    [SerializeField] private PopUpCard cards;
    [SerializeField] private EndGameUI endGame;

    private Action returnFuntion;

    private void Awake()
    {
        Instance = this;
    }

    // UI
    private void OpenUI()
    {
        GameController.PopUp = true;
    }
    private void CloseUI()
    {
        GameController.PopUp = false;
    }

    // Battle
    public void StartBattle(Province atk, Province def, Action returnFuntion)
    {
        OpenUI();
        this.returnFuntion = returnFuntion;

        battle.gameObject.SetActive(true);
        battle.SetupBattle(atk, def);
    }
    public void EndBattle()
    {
        CloseUI();
        battle.gameObject.SetActive(false);
        returnFuntion?.Invoke();
    }

    // Transfer
    public void StartTransfer(Province from, Province to, Action returnFuntion)
    {
        OpenUI();
        this.returnFuntion = returnFuntion;

        transfer.gameObject.SetActive(true);
        transfer.SetupTransfer(from, to);
    }
    public void EndTransfer()
    {
        CloseUI();
        transfer.gameObject.SetActive(false);
        returnFuntion?.Invoke();
    }

    // Reinforce
    public void StartReinforce(Province to, Action returnFuntion)
    {
        OpenUI();
        this.returnFuntion = returnFuntion;

        reinforce.gameObject.SetActive(true);
        reinforce.SetupReinforce(to);
    }
    public void EndReinforce()
    {
        CloseUI();
        reinforce.gameObject.SetActive(false);
        returnFuntion?.Invoke();
    }

    // Cards
    internal void OpenCards(Player player)
    {
        OpenUI();
        cards.gameObject.SetActive(true);
        cards.Open(player);
    }
    public void CloseCards()
    {
        CloseUI();
        cards.gameObject.SetActive(false);
    }

    // EndGame
    public void EndGame(Color winner)
    {
        endGame.gameObject.SetActive(true);
        endGame.Open(winner);
    }
}