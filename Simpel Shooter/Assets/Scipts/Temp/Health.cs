using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour
{
    private byte _health;
    private bool _isDead;

    public byte CurrentHealth
    {
        get { return _health; }
        protected set
        {
            _health = value;
            Debug.Log("Health Changed To " + _health);
        }
    }
    public bool IsDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    public void TakeDamage(byte damage, Participant sender)
    {
        if (damage >= CurrentHealth)
        {
            CurrentHealth = 0;
            Die(sender);
        }
        else
        {
            CurrentHealth -= damage;
        }
    }
    public void AddHealth(byte amount)
    {
        if (CurrentHealth + amount < 100)
            CurrentHealth += amount;
        else
            CurrentHealth = 100;
    }

    public delegate void DieEventHandler(Participant killer);
    public event DieEventHandler OnDie;
    void Die(Participant killer)
    {
        if (OnDie != null)
            OnDie(killer);
    }
}
