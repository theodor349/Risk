using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum GameState { Reinforce, Battle, Transfer }
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private GameObject map;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI stateText;

    private Continent[] continents;
    private GameState state;
    private List<Player> players;
    private int activePlayer;
    private int playersKilled = 0;

    private void Awake()
    {
        Instance = this;

        continents = map.GetComponentsInChildren<Continent>();
    }

    private void Start()
    {
        players = new List<Player>();
        players.Add(new Player(Color.green));
        players.Add(new Player(Color.red));

        foreach (var continent in continents)
        {
            foreach (var province in continent.provinces)
            {
                int p = Random.Range(0, players.Count);
                province.Player = players[p];
                province.Soldiers = 1;
            }
        }

        playerImage.color = players[activePlayer].Color;
        stateText.text = state.ToString();
    }

    public void ProvinceClicked(Province p)
    {
        if (state == GameState.Battle)
        {
            BattleState.ProvinceClicked(p, players[activePlayer]);
        }
        else if (state == GameState.Transfer)
        {
            TransferState.ProvinceClicked(p, players[activePlayer]);
        }
        else if (state == GameState.Reinforce)
        {
            ReinforcetState.ProvinceClicked(p, players[activePlayer]);
        }
    }

    internal void RightClick()
    {
        if (state == GameState.Battle)
        {
            BattleState.RightClick();
        }
        else if (state == GameState.Transfer)
        {
            TransferState.RightClick();
        }
        else if (state == GameState.Reinforce)
        {
            ReinforcetState.RightClick();
        }
    }

    public void PlayerDone()
    {
        if (!CanEndTurn(players[activePlayer]))
            return;

        activePlayer++;
        if(activePlayer == players.Count)
        {
            activePlayer = 0;
            NextState();
        }

        // In case the player is dead
        if (players[activePlayer].Provinces == 0)
        {
            activePlayer++;
            if (activePlayer == players.Count)
            {
                activePlayer = 0;
                NextState();
            }
        }

        playerImage.color = players[activePlayer].Color;
    }

    private bool CanEndTurn(Player player)
    {
        return !(state == GameState.Reinforce && player.Reinforcements > 0);
    }

    private void NextState()
    {
        switch (state)
        {
            case GameState.Reinforce:
                state = GameState.Battle;
                break;
            case GameState.Battle:
                state = GameState.Transfer;
                break;
            case GameState.Transfer:
                state = GameState.Reinforce;
                CalculateReinforcements();
                break;
        }

        stateText.text = state.ToString();
    }

    private void CalculateReinforcements()
    {
        foreach (var player in players)
        {
            player.Reinforcements = player.Provinces / 3;
        }

        ReinforcementsFromContinents();
    }

    private void ReinforcementsFromContinents()
    {
        foreach (var continent in continents)
        {
            continent.AddReinforcements();
        }
    }

    internal void KillPlayer(Player player)
    {
        playersKilled++;
    }
}

public static class ReinforcetState
{
    public static void RightClick()
    {
    }

    public static void ProvinceClicked(Province p, Player activePlayer)
    {
        if (p.Player != activePlayer)
            return;

        PopUpController.Instance.StartReinforce(p, ReinforcementDone);
    }

    public static void ReinforcementDone()
    {
    }
}

public static class BattleState
{
    private static Province attacker;
    private static Province defender;

    public static void RightClick()
    {
        ResetBattle();
    }

    public static void ProvinceClicked(Province p, Player activePlayer)
    {
        if (attacker == null)
        {
            if (p.Player != activePlayer)
                return;

            if (p.CanAttack())
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
        ResetBattle();
    }
}

public static class TransferState
{
    private static Province from;

    public static void RightClick()
    {
        ResetTransfer();
    }

    public static void ProvinceClicked(Province p, Player activePlayer)
    {
        if (from == null)
        {
            if (p.Player != activePlayer)
                return;

            from = p;
        }
        else
        {
            Transfer(p);
        }
    }

    private static void ResetTransfer()
    {
        from = null;
    }

    private static void Transfer(Province to)
    {
        if(from.Player != to.Player)
        {
            ResetTransfer();
            return;
        }

        if (!from.Neighbours(to))
            return;

        PopUpController.Instance.StartTransfer(from, to, TransferDone);
    }

    private static void TransferDone()
    {
        ResetTransfer();
    }
}