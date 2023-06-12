using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsBar : MonoBehaviour, ISoulsBar
{
    [SerializeField]private TextMeshProUGUI textMeshPro;

    void ISoulsBar.SetSouls(int count)
    {
        textMeshPro.text = count.ToString();
    }
}
