using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public enum GameState { Reinforce, Battle, Transfer }
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static bool PopUp = true;

    [SerializeField] private GameObject map;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI stateText;

    private Continent[] continents;
    private GameState state;
    private List<Player> players;
    private int activePlayer;

    private void Awake()
    {
        Instance = this;

        continents = map.GetComponentsInChildren<Continent>();
    }

    public void StartGame(List<Player> players)
    {
        this.players = players;

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

        PopUp = false;
    }

    public void ProvinceClicked(Province p)
    {
        if (PopUp)
            return;

        switch (state)
        {
            case GameState.Battle:
                BattleState.ProvinceClicked(p, players[activePlayer]);
                break;
            case GameState.Transfer:
                TransferState.ProvinceClicked(p, players[activePlayer]);
                break;
            case GameState.Reinforce:
                ReinforceState.ProvinceClicked(p, players[activePlayer]);
                break;
        }
    }

    internal void RightClick()
    {
        if (PopUp)
            return;

        switch (state)
        {
            case GameState.Battle:
                BattleState.RightClick();
                break;
            case GameState.Transfer:
                TransferState.RightClick();
                break;
            case GameState.Reinforce:
                ReinforceState.RightClick();
                break;
        }
    }

    public void PlayerDone()
    {
        if (!CanEndTurn(players[activePlayer]))
            return;

        players[activePlayer].TurnDone();

        bool nextStage = false;
        activePlayer++;
        if(activePlayer == players.Count)
        {
            activePlayer = 0;
            nextStage = true;
        }

        // In case the player is dead
        while (players[activePlayer].Provinces == 0)
        {
            activePlayer++;
            if (activePlayer == players.Count)
            {
                activePlayer = 0;
                nextStage = true;
            }
        }

        if(nextStage)
            NextState();

        playerImage.color = players[activePlayer].Color;
        AudioController.Instance.PlayNext();
    }

    private void EndGame()
    {
        PopUpController.Instance.EndGame(GetWinner());
    }

    private Color GetWinner()
    {
        Color c = Color.black;

        foreach (var player in players)
        {
            if (player.Provinces != 0)
                c = player.Color;
        }

        return c;
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

    public void OpenCards()
    {
        PopUpController.Instance.OpenCards(players[activePlayer]);
    }

    public void CheckGameOver()
    {
        int alive = 0;
        foreach (var player in players)
        {
            if (player.Provinces > 0)
                alive++;
        }

        if(alive == 1)
            EndGame();
    }
}

public static class ReinforceState
{
    public static bool DoneReinforcing = false;

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
        if (DoneReinforcing)
            GameController.Instance.PlayerDone();

        DoneReinforcing = false;
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
        {
            attacker = other;
            return;
        }
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

            attacker.Player.GiveCard();

            // Create Transfer Window and call TransferArmy when amount is found (Action)
            PopUpController.Instance.StartTransfer(attacker, defender, BattleDone);
        }
        else
            ResetBattle();

        GameController.Instance.CheckGameOver();
    }

    public static void BattleDone()
    {
        ResetBattle();
    }
}

public static class TransferState
{
    public static bool DidTransfer = false;

    private static Province from;

    public static void RightClick()
    {
        ResetTransfer();
    }

    public static void ProvinceClicked(Province p, Player activePlayer)
    {
        DidTransfer = false;

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
        DidTransfer = false;
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
        if (DidTransfer)
            GameController.Instance.PlayerDone();

        ResetTransfer();
    }
}