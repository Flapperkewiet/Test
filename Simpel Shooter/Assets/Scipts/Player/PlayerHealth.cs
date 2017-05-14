using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    public UnityEngine.UI.Slider HealthBarSlider;
    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void Die()
    {
        Debug.Log("You Died!");
        CurrentHealth = MaxHealth;
        HealthBarSlider.value = CurrentHealth / MaxHealth;
        SpawnManager.Respawn(gameObject);
    }

    public void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
            Die();
        else if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        HealthBarSlider.value = CurrentHealth / MaxHealth;
    }

}
