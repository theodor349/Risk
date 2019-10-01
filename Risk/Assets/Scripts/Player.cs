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
            if (provinces <= 0)
                GameController.Instance.KillPlayer(this);
        }
    }

    public int Reinforcements = 10;
    private int provinces;

    public Player(Color color)
    {
        Color = color;
    }
}
