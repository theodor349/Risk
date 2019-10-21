using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Color Color { get; private set; }
    public int Provinces {
        get {
            return provinces;
        }
        set {
            provinces = value;
        }
    }

    public int Reinforcements = 10;

    // Cards
    public int Soldiers = 0;
    public int Horses = 0;
    public int Canons= 0;

    private int provinces;
    private bool gottenCard;

    public Player(Color color)
    {
        Color = color;
    }

    public void TurnDone()
    {
        gottenCard = false;
    }

    public void GiveCard()
    {
        if (gottenCard)
            return;

        gottenCard = true;
        switch (Random.Range(0, 3))
        {
            case 0:
                Soldiers++;
                break;
            case 1:
                Horses++;
                break;
            case 2:
                Canons++;
                break;
            default:
                break;
        }
    }
}
