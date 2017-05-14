using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public float Damage;
    float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player")
            collision.transform.GetComponent<PlayerHealth>().TakeDamage(Damage);
        Destroy(gameObject);
    }
}
