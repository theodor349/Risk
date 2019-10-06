using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Soldier, Horse, Canon }
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
