using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private Image playerColorImage;

    public void Open(Color winnerColor)
    {
        playerColorImage.color = winnerColor;
    }

    public void BtnPlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BtnQuit()
    {
        Application.Quit();
    }
}
