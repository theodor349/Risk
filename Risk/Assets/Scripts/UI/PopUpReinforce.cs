using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpReinforce : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fromArmySize;
    [SerializeField] private TextMeshProUGUI toProvinceName;
    [SerializeField] private TextMeshProUGUI toArmySize;
    [SerializeField] private Slider slider;

    private Province to;
    private int reinforceStartAmount;
    private int toStartAmount;
    private PopUpController popUpController;

    private void Start()
    {
        popUpController = PopUpController.Instance;
    }

    public void SetupReinforce(Province to)
    {
        this.to = to;
        reinforceStartAmount = to.Player.Reinforcements;
        toStartAmount = to.Soldiers;
        slider.value = 0;

        UpdateUI();
    }

    public void Okay()
    {
        popUpController.EndReinforce();
    }

    public void OnSliderChange()
    {
        int moveAmount = (int)(reinforceStartAmount * slider.value);
        to.Player.Reinforcements = reinforceStartAmount - moveAmount;
        to.Soldiers = toStartAmount + moveAmount;

        UpdateUI();
    }

    private void UpdateUI()
    {
        fromArmySize.text = to.Player.Reinforcements.ToString();
        toProvinceName.text = to.provinceName;
        toArmySize.text = to.Soldiers.ToString();
    }
}
