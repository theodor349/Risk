using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continent : MonoBehaviour
{
    [SerializeField] private int bonusArmies;

    private Province[] provineces;

    private void Start()
    {
        provineces = GetComponentsInChildren<Province>();
        Debug.Log(transform.name + " has " + provineces.Length + " provinces");

        Test();
    }

    private void Test()
    {
        foreach (var p in provineces)
        {
            p.Player = new Player(Random.Range(1,10) > 5 ? Color.red : Color.green);
            p.Soldiers = Random.Range(1, 100);
        }
    }
}
