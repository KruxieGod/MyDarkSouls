using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image WeaponIcon;
    public Image ParentWeaponIcon;
    private AspectRatioFitter aspect;

    private void Awake()
    {
        aspect = WeaponIcon.GetComponentInChildren<AspectRatioFitter>();
    }

    public void UpdateWeaponQuickSlotsUI(Item weapon)
    {
        if (weapon.ItemIcon != null)
        {
            WeaponIcon.sprite = weapon.ItemIcon;
            float aspectRatio = Mathf.Clamp((float)WeaponIcon.sprite.texture.width / WeaponIcon.sprite.texture.height,
                0.43f,
                10f);
            aspect.aspectRatio = aspectRatio;

            WeaponIcon.gameObject.SetActive(true);
        }
        else
        {
            WeaponIcon.sprite = null;
            WeaponIcon.gameObject.SetActive(false);
        }
    }
}
