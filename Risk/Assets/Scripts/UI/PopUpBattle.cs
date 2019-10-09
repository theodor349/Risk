using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpBattle : MonoBehaviour
{
    [SerializeField] private Image atkImage;
    [SerializeField] private TextMeshProUGUI atkArmy;
    [SerializeField] private TextMeshProUGUI atkProvince;
    [SerializeField] private TextMeshProUGUI atkDice1;
    [SerializeField] private TextMeshProUGUI atkDice2;
    [SerializeField] private TextMeshProUGUI atkDice3;

    [SerializeField] private Image defImage;
    [SerializeField] private TextMeshProUGUI defArmy;
    [SerializeField] private TextMeshProUGUI defProvince;
    [SerializeField] private TextMeshProUGUI defDice1;
    [SerializeField] private TextMeshProUGUI defDice2;

    private Province atk;
    private Province def;

    private PopUpController popUpController;

    private void Start()
    {
        popUpController = PopUpController.Instance;
    }

    public void SetupBattle(Province atk, Province def)
    {
        this.atk = atk;
        this.def = def;

        atkImage.color = atk.Player.Color;
        atkArmy.text = (atk.Soldiers - 1).ToString();
        atkProvince.text = "Attacker\n" +  atk.provinceName;

        defImage.color = def.Player.Color;
        defArmy.text = (def.Soldiers).ToString();
        defProvince.text = "Defender\n" + def.provinceName;

        // Set Dice
        atkDice2.transform.parent.gameObject.SetActive(atk.Soldiers > 2);
        atkDice3.transform.parent.gameObject.SetActive(atk.Soldiers > 3);
        defDice2.transform.parent.gameObject.SetActive(def.Soldiers > 1 && atk.Soldiers > 2);
    }

    public void Retreate()
    {
        popUpController.EndBattle();
    }

    private void BattleWon()
    {
        popUpController.EndBattle();
    }

    public void Fight()
    {
        // Set UI
        atkDice2.transform.parent.gameObject.SetActive(atk.Soldiers > 2);
        atkDice3.transform.parent.gameObject.SetActive(atk.Soldiers > 3);
        defDice2.transform.parent.gameObject.SetActive(def.Soldiers > 1 && atk.Soldiers > 2);

        // Calculate Outcome
        atkDice1.text = Random.Range(1, 7).ToString();
        if(atk.Soldiers > 2)
            atkDice2.text = Random.Range(1, 7).ToString();
        if(atk.Soldiers > 3)
            atkDice3.text = Random.Range(1, 7).ToString();

        defDice1.text = Random.Range(1, 7).ToString();
        if(def.Soldiers > 1 && atk.Soldiers > 2)
            defDice2.text = Random.Range(1, 7).ToString();

        // What is biggest value
        int attack = Mathf.Max(int.Parse(atkDice1.text), atk.Soldiers > 2 ? int.Parse(atkDice2.text) : 0, atk.Soldiers > 3 ? int.Parse(atkDice3.text) : 0);
        int deffender = Mathf.Max(int.Parse(defDice1.text), atk.Soldiers > 2 && def.Soldiers > 1? int.Parse(defDice2.text) : 0);

        // Take damage
        if (attack > deffender)
            def.TakeDamage(atk.Soldiers > 2 && def.Soldiers > 1 ? 2 : 1);
        else
            atk.TakeDamage(atk.Soldiers > 2 && def.Soldiers > 1 ? 2 : 1);

        if (def.Soldiers == 0)
        {
            BattleWon();
            return;
        }
        else if (atk.Soldiers == 1)
        {
            BattleWon();
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        atkArmy.text = (atk.Soldiers - 1).ToString();
        defArmy.text = (def.Soldiers).ToString();
    }
}
