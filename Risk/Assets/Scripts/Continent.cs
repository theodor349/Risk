using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent : MonoBehaviour
{
    [SerializeField] private int bonusArmies;

    public Province[] provineces { get; private set; }

    private void Awake()
    {
        provineces = GetComponentsInChildren<Province>();
    }
}
