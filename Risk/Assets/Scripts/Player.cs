using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Color Color { get; private set; }

    public Player(Color color)
    {
        Color = color;
    }
}
