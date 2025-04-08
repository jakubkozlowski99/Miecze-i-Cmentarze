using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color lowStateColor;
    public Color highStateColor;
    public Vector3 offset;
    public bool isBoss;

    public virtual void SetHealth(float health, float maxHealth)
    {
        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(lowStateColor, highStateColor, slider.normalizedValue);
    }
    protected virtual void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
