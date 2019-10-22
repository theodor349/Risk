using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerGameObjects = new List<GameObject>();

    private int playerCount = 0;

    private void Start()
    {
        AddPlayer();
        AddPlayer();
    }

    public void BtnStartGame()
    {
        GameController.Instance.StartGame(GeneratePlayers());
        gameObject.SetActive(false);
    }

    public void BtnQuit()
    {
        Application.Quit();
    }

    public void SliderChangeColor(int i)
    {
        ChangeColor(i);
    }

    public void BtnAddPlayer()
    {
        AddPlayer();
    }

    public void BtnRemovePlayer()
    {
        RemovePlayer();
    }

    private void AddPlayer()
    {
        if (playerCount == 7)
            return;

        playerGameObjects[playerCount].SetActive(true);
        ChangeColor(playerCount);
        playerCount++;
    }

    private void RemovePlayer()
    {
        if (playerCount == 2)
            return;

        playerCount--;
        playerGameObjects[playerCount].SetActive(false);
    }

    private void ChangeColor(int i)
    {
        playerGameObjects[i].GetComponentInChildren<Image>().color = GetColorOfSlider(i);
    }

    private Color GetColorOfSlider(int i)
    {
        Color c = new Color();
        switch (playerGameObjects[i].GetComponentInChildren<TMP_Dropdown>().value)
        {
            case 0:
                c = Color.blue;
                break;
            case 1:
                c = Color.red;
                break;
            case 2:
                c = Color.green;
                break;
            case 3:
                c = Color.white;
                break;
            case 4:
                c = Color.yellow;
                break;
            case 5:
                c = Color.magenta;
                break;
            case 6:
                c = Color.cyan;
                break;
        }

        return c;
    }

    private List<Player> GeneratePlayers()
    {
        List<Player> players = new List<Player>();

        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new Player(playerGameObjects[i].GetComponentInChildren<Image>().color));
        }

        return players;
    }
}
