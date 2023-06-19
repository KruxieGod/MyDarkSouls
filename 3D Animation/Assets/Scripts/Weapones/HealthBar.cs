using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SG;

public class HealthBar : MonoBehaviour,IHealthBar
{
    protected Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void IHealthBar.SetMaxHealth(int maxHealth)
    {
        this.SetMaxHealth(maxHealth);
    }

    void IHealthBar.SetCurrentHealth(int currentHealth)
    {
        this.SetCurrentHealth(currentHealth);
    }

    internal virtual void SetMaxHealth(int maxHealth)
    {
        if (slider == null)
            slider = GetComponentInChildren<Slider>();
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    internal virtual void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
