using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Fight }
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private GameState state;

    private void Awake()
    {
        Instance = this;
    }

    public void ProvinceClicked(Province p)
    {
        if(state == GameState.Fight)
        {
            FigthState.ProvinceClicked(p);
        }
    }

    internal void RightClick()
    {
        if (state == GameState.Fight)
        {
            FigthState.RightClick();
        }
    }
}

public static class FigthState
{
    public static Tuple<int, int> Damage;
    public static int Transfer;

    private static Province attacker;
    private static Province defender;

    public static void RightClick()
    {
        ResetBattle();
    }

    public static void ProvinceClicked(Province p)
    {
        if (attacker == null)
        {
            if(p.CanAttack())
                attacker = p;
        }
        else
            Battle(p);
    }

    private static void ResetBattle()
    {
        attacker = null;
        defender = null;
    }

    private static void Battle(Province other)
    {
        if (other.Player == attacker.Player)
            return;
        if (!attacker.CanAttack())
            return;
        if (!attacker.Neighbours(other))
            return;
        defender = other;

        // Create Battle Window and call Battle Done when damage is found (Action)
        PopUpController.Instance.StartBattle(attacker, other, FightDone);
    }

    public static void FightDone()
    {
        if (defender.Soldiers < 1)
        {
            defender.Player = attacker.Player;
            defender.Soldiers = 1;
            attacker.Soldiers -= 1;
            // Create Transfer Window and call TransferArmy when amount is found (Action)
            PopUpController.Instance.StartTransfer(attacker, defender, BattleDone);
        }
        else
            ResetBattle();
    }

    public static void BattleDone()
    {
        Debug.Log("Battle Done");
        ResetBattle();
    }
}
