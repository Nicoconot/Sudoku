using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveUI : MonoBehaviour
{
    private bool isVertical = true;

    [SerializeField] private GameObject title;
    private void Awake()
    {
        float ratio = Screen.width / Screen.height;

        if (ratio > 0.9f)
        {
            isVertical = false;
        }
        
        AdjustUI();
    }

    void AdjustUI()
    {
        if (isVertical) return;

        title.SetActive(false);
    }
}
