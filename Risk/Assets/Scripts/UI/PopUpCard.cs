using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum CardType { Soldier, Horse, Canon }
public class PopUpCard : MonoBehaviour
{
    public static PopUpCard Instance;

    [SerializeField] private TextMeshProUGUI soldiers;
    [SerializeField] private TextMeshProUGUI horses;
    [SerializeField] private TextMeshProUGUI canons;

    [SerializeField] private TextMeshProUGUI extraSoldiers;

    [SerializeField] private Transform cardSpawn;
    [SerializeField] private GameObject[] buttons;

    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private GameObject horsePrefab;
    [SerializeField] private GameObject canonPrefab;

    private Player player;
    private List<GameObject> selectedCards = new List<GameObject>();
    private List<CardType> selectedTypes = new List<CardType>();

    private void Awake()
    {
        Instance = this;
    }

    public void Open(Player player)
    {
        this.player = player;

        UpdateUI();
    }

    public void BtnCancel()
    {
        GiveBackCardsToPlayer();
        Reset();
        PopUpController.Instance.CloseCards();
    }

    public void BtnOkay()
    {
        int reinforcements = CalculateExtraSoldiers();
        player.Reinforcements += reinforcements;
        if(reinforcements == 0)
            GiveBackCardsToPlayer();
        Reset();
        PopUpController.Instance.CloseCards();
    }

    public void BtnReset()
    {
        Reset();
    }

    public void BtnSelect(int t)
    {
        if (selectedCards.Count == 3)
            return;

        var type = (CardType)t;
        if (type == CardType.Soldier && player.Soldiers == 0)
            return;
        if (type == CardType.Horse && player.Horses == 0)
            return;
        if (type == CardType.Canon && player.Canons == 0)
            return;

                selectedTypes.Add(type);
        RemoveCardFromPlayer(type);

        var go = Instantiate(soldierPrefab);
        switch (type)
        {
            case CardType.Soldier:
                go.transform.SetParent(cardSpawn);
                selectedCards.Add(go);
                break;
            case CardType.Horse:
                go = Instantiate(horsePrefab);
                go.transform.SetParent(cardSpawn);
                selectedCards.Add(go);
                break;
            case CardType.Canon:
                go = Instantiate(canonPrefab);
                go.transform.SetParent(cardSpawn);
                selectedCards.Add(go);
                break;
            default:
                break;
        }
        go.transform.localScale = Vector3.one;
        UpdateUI();
    }

    public void BtnDeselect(int i)
    {
        GiveCardBack(selectedTypes[i]);
        Destroy(selectedCards[i]);
        selectedCards.RemoveAt(i);
        selectedTypes.RemoveAt(i);

        UpdateUI();
    }

    private void UpdateUI()
    {
        soldiers.text = player.Soldiers.ToString();
        horses.text = player.Horses.ToString();
        canons.text = player.Canons.ToString();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        for (int i = 0; i < selectedCards.Count; i++)
        {
            buttons[i].SetActive(true);
        }

        extraSoldiers.text = CalculateExtraSoldiers().ToString();
    }

    private int CalculateExtraSoldiers()
    {
        if (selectedCards.Count != 3)
            return 0;
        if (!IsCardsTheSame())
            return 0;

        int result = selectedTypes[0] == CardType.Soldier ? 5 : 0;
        if (selectedTypes[1] == CardType.Horse)
            result = 8;
        else if (selectedTypes[2] == CardType.Canon)
            result = 10;

        return result;
    }

    private bool IsCardsTheSame()
    {
        var type = selectedTypes[0];
        foreach (var t in selectedTypes)
        {
            if (type != t)
                return false;
        }
        return true;
    }

    private void RemoveCardFromPlayer(CardType type)
    {
        switch (type)
        {
            case CardType.Soldier:
                player.Soldiers--;
                break;
            case CardType.Horse:
                player.Horses--;
                break;
            case CardType.Canon:
                player.Canons--;
                break;
            default:
                break;
        }
    }

    private void GiveCardBack(CardType type)
    {
        switch (type)
        {
            case CardType.Soldier:
                player.Soldiers++;
                break;
            case CardType.Horse:
                player.Horses++;
                break;
            case CardType.Canon:
                player.Canons++;
                break;
            default:
                break;
        }
    }

    private void GiveBackCardsToPlayer()
    {
        foreach (var card in selectedTypes)
        {
            switch (card)
            {
                case CardType.Soldier:
                    player.Soldiers++;
                    break;
                case CardType.Horse:
                    player.Horses++;
                    break;
                case CardType.Canon:
                    player.Canons++;
                    break;
                default:
                    break;
            }
        }
    }

    private void Reset()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        for (int i = selectedCards.Count - 1; i >= 0; i--)
        {
            Destroy(selectedCards[i]);
        }
        selectedCards.Clear();

        foreach (var btn in buttons)
        {
            btn.SetActive(false);
        }

        selectedTypes.Clear();
    }
}
