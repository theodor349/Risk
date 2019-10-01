using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent : MonoBehaviour
{
    [SerializeField] private int bonusReinforcements;

    public Province[] provinces { get; private set; }

    private void Awake()
    {
        provinces = GetComponentsInChildren<Province>();
    }

    public void AddReinforcements()
    {
        var last = provinces[0];
        foreach (var province in provinces)
        {
            if (last.Player != province.Player)
                return;
        }
        last.Player.Reinforcements += bonusReinforcements;
    }
}
