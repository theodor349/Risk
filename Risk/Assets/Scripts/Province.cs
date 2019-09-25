using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
public class Province : MonoBehaviour
{
    public string provinceName;
    [SerializeField] private Province[] borderProvinces;
    [SerializeField] private Transform textPos;
    private TextMeshProUGUI armyText;
    private SpriteRenderer provinceSprite;

    private int army;
    private Player player;
    private List<Link> links = new List<Link>();

    public int Soldiers {
        get { return army; }
        set {
            army = value <= 0 ? 0 : value;
            UpdateProvince();
        }
    }

    public Player Player {
        get { return player; }
        set {
            player = value;
            UpdateProvince();
        }
    }

    private void Awake()
    {
        provinceSprite = GetComponent<SpriteRenderer>();
        armyText = TextController.Instance.SetupText(textPos.position);
        GenerateLinks();
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void GenerateLinks()
    {
        foreach (var p in borderProvinces)
        {
            if (LinkExists(p))
                continue;

            var l = new Link(this, p);
            links.Add(l);
            p.links.Add(l);
            LineController.Links.Add(l);
        }
    }

    private bool LinkExists(Province p)
    {
        foreach (var link in links)
        {
            if (link.GetOther(this) == p)
                return true;
        }
        return false;
    }

    private void UpdateProvince()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (army == 0)
            DrawEmpty();
        else
            Draw();
    }

    private void DrawEmpty()
    {
        provinceSprite.color = Color.white;
        armyText.text = "";
    }

    private void Draw()
    {
        provinceSprite.color = player.Color;
        armyText.text = army.ToString();
    }

    public bool CanAttack()
    {
        return Soldiers > 1;
    }

    public bool Neighbours(Province other)
    {
        foreach (var link in links)
        {
            if (link.GetOther(this) == other)
                return true;
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        Soldiers -= damage;
    }

    public int Strength(bool isDefending = false)
    {
        return isDefending ? Soldiers : Soldiers - 1;
    }
}
