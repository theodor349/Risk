using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private int provinceLayer = 1 << 8;
    private Camera cam;
    private GameController gameController;

    private void Start()
    {
        cam = Camera.main;
        gameController = GameController.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick();
        if (Input.GetMouseButtonUp(1))
            RightClick();
    }

    private void RightClick()
    {
        gameController.RightClick();
    }

    private void LeftClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono), -Vector2.up, Mathf.Infinity, provinceLayer);
        if (hit.transform != null)
        {
            gameController.ProvinceClicked(hit.transform.GetComponent<Province>());
        }
    }
}
