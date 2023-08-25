using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SG;

public class HealthBar : MonoBehaviour,IHealthBar
{
    protected Slider slider;

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
        Debug.Log("HPISSETTED");
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    internal virtual void SetCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }

    void IHealthBar.SetName(string name)
    {
        this.SetName(name);
    }

    internal virtual void SetName(string name)
    {

    }

    internal virtual void Initialize()
    {
        slider = GetComponent<Slider>();
    }

    internal virtual void SetOff()
    {

    }

    void IHealthBar.Initialize()
    {
        this.Initialize();
    }

    void IHealthBar.SetOff()
    {
        SetOff();
    }
}
