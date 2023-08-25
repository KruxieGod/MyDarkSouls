using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBossHealthBar : HealthBar
{
    private TextMeshProUGUI textMeshProUGUI;

    internal override void Initialize()
    {
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        }
        gameObject.SetActive(true);
    }

    internal override void SetOff()
    {
        Debug.Log("OFF");
        gameObject.SetActive(false);
    }

    internal override void SetName(string name)
    {
        textMeshProUGUI.text= name;
    }
}
