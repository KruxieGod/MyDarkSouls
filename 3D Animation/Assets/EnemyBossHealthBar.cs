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
        slider = GetComponentInChildren<Slider>();
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    internal override void SetName(string name)
    {
        textMeshProUGUI.text= name;
    }
}
