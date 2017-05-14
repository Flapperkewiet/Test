using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth = 100;
    public float DieForce = 3f;

    public bool IsDead;
    GameObject player;
    GameObject HealthBarObject;
    public Material DeadMaterial;
    public Material AliveMaterial;


    public float FadeDuration;
    float fadeTimer = 0f;
    float fadeDurationInv;
    public GameObject Model;
    public GameObject Collider;
    Renderer[] ModelParts;
    void Start()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
        player = Settings.Player;
        HealthBarObject = transform.FindChild("HealthBar").gameObject;
        fadeDurationInv = 1f / (FadeDuration != 0f ? FadeDuration : 1f);
        SetHealthBarScale();
        ModelParts = new Renderer[Model.transform.childCount];
        for (int i = 0; i < Model.transform.childCount; i++)
            ModelParts[i] = Model.transform.GetChild(i).GetComponent<Renderer>();
    }

    void Respawn()
    {
        Destroy(GetComponent<Rigidbody>());
        CurrentHealth = MaxHealth;
        SetHealthBarScale();
        HealthBarObject.SetActive(true);
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        for (int i = 0; i < ModelParts.Length; i++)
            ModelParts[i].material = AliveMaterial;
        IsDead = false;
        Collider.layer = 8;
        SpawnManager.Respawn(gameObject);
    }

    void Update()
    {
        if (IsDead)
        {
            Color32 color = ModelParts[0].material.color;

            color.a = (byte)Mathf.Lerp(255, 0, fadeTimer * fadeDurationInv);
            if (color.a == 0)
            {
                Respawn();
            }
            for (int i = 0; i < ModelParts.Length; i++)
                ModelParts[i].material.color = color;
            fadeTimer += Time.deltaTime;
        }
        else
        {
            HealthBarObject.transform.eulerAngles = new Vector3(90, player.transform.eulerAngles.y - 180, 0);
        }
    }

    public void SetHealthBarScale()
    {
        HealthBarObject.transform.localScale = new Vector3(0.15f * (CurrentHealth / MaxHealth), 0.015f, 0.015f);
    }
    public void TakeDamage(float Damage)
    {
        if (IsDead)
            return;
        CurrentHealth -= Damage;
        SetHealthBarScale();
        if (CurrentHealth <= 0)
            Die();
        else if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public void Die()
    {
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position).normalized * DieForce, ForceMode.Impulse);
        HealthBarObject.SetActive(false);
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        for (int i = 0; i < ModelParts.Length; i++)
            ModelParts[i].material = DeadMaterial;
        IsDead = true;
        Collider.layer = 9;
    }
}
