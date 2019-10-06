using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCard : MonoBehaviour
{
    public PopUpCard Instance;


    private void Awake()
    {
        Instance = this;
    }

    public void Open(Player player)
    {

    }
}
