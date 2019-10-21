using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public CardType Type;
    public string ProvinceName;

    public Card(CardType type, string provinceName)
    {
        Type = type;
        ProvinceName = provinceName;
    }
}
