using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : HealthBar
{
    private RectTransform rectTransform;
    private Transform cameraTransform;
    private Transform cameraPivotTransform;
    [SerializeField] private Transform textTransform;
    [SerializeField] private GameObject prefabText;
    [SerializeField] private float maxTime;
    private float recoveryShowHealthBar;

    internal override void Initialize()
    {
        slider = GetComponentInChildren<Slider>();
        rectTransform = slider.GetComponent<RectTransform>();
        cameraTransform = FindObjectOfType<CameraManager>().gameObject.transform;
        cameraPivotTransform = cameraTransform.GetChild(0);
    }

    private void FixedUpdate()
    {
        if (slider == null)
            return;
        if (recoveryShowHealthBar <=0)
            slider.gameObject.SetActive(false);
        else
            slider.gameObject.SetActive(true);
        recoveryShowHealthBar -= Time.deltaTime;
        rectTransform.rotation = Quaternion.Euler(new Vector3(cameraPivotTransform.eulerAngles.x, cameraTransform.rotation.eulerAngles.y));
    }

    internal override void SetCurrentHealth(int currentHealth)
    {
        int damage = (int)slider.value - currentHealth;
        var prefab = Instantiate(prefabText, textTransform);
        prefab.GetComponent<TextMeshPro>().text = damage.ToString();
        Destroy(prefab, 1f);
        Debug.Log("Damage from player: "+damage);
        base.SetCurrentHealth(currentHealth);
        recoveryShowHealthBar = maxTime;
        Debug.Log(recoveryShowHealthBar);
    }
}
