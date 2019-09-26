using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpTransfer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fromProvinceName;
    [SerializeField] private TextMeshProUGUI fromArmySize;
    [SerializeField] private TextMeshProUGUI toProvinceName;
    [SerializeField] private TextMeshProUGUI toArmySize;
    [SerializeField] private Slider slider;

    private Province from;
    private Province to;
    private int fromStartAmount;
    private int toStartAmount;
    private PopUpController popUpController;

    private void Start()
    {
        popUpController = PopUpController.Instance;
    }

    public void SetupTransfer(Province from, Province to)
    {
        this.from = from;
        this.to = to;
        slider.value = 0;
        fromStartAmount = from.Soldiers;
        toStartAmount = to.Soldiers;

        UpdateUI();
    }

    public void Okay()
    {
        popUpController.EndTransfer();
    }

    public void OnSliderChange()
    {
        int s = from.Soldiers + to.Soldiers;

        int moveAmount = (int)((fromStartAmount - 1) * slider.value);
        from.Soldiers = fromStartAmount - moveAmount;
        to.Soldiers = toStartAmount + moveAmount;

        int a = from.Soldiers + to.Soldiers;
        Debug.Log("Diff: " + (s - a) + " m: " + moveAmount + " s: " + slider.value);
        UpdateUI();
    }

    private void UpdateUI()
    {
        fromProvinceName.text = from.provinceName;
        fromArmySize.text = from.Soldiers.ToString();
        toProvinceName.text = to.provinceName;
        toArmySize.text = to.Soldiers.ToString();
    }
}
