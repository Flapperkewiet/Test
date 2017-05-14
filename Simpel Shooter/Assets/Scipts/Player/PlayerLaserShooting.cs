using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerLaserShooting : MonoBehaviour
{
    [Header("Laser")]
    public GameObject laserStart;
    public float LaserDamage;
    public Color LaserColor;
    [Range(0.05f, 1.0f)]
    public float LaserWidth;
    [Header("Energy")]
    public UnityEngine.UI.Slider EnergySlider;
    float _currentEnergy;
    public float CurrentEnergy
    {
        get { return _currentEnergy; }
        set
        {
            _currentEnergy = value;
            EnergySlider.value = CurrentEnergy / MaxEnergy;
        }
    }

    public float MaxEnergy;
    public float EnergyDrain;
    public float EnergyRechargeRate;
    public float TimeBeforeRecharing;

    float TimeSinceLastFired;
    bool IsFiring;
    LineRenderer lineRenderer;
    AudioSource LaserAudioSource;
    [Range(0, 1.0f)]
    public float SoundFadeDuration;

    void Awake()
    {
        OnValidate();
        CurrentEnergy = MaxEnergy;
        LaserAudioSource = GetComponent<AudioSource>();
    }

    void Recharge()
    {
        if (IsFiring == true)
            TimeSinceLastFired = 0;
        else if (TimeSinceLastFired < TimeBeforeRecharing)
            TimeSinceLastFired += Time.deltaTime;
        if (TimeSinceLastFired >= TimeBeforeRecharing)
        {
            CurrentEnergy = CurrentEnergy + Time.deltaTime * EnergyRechargeRate;
            if (CurrentEnergy > MaxEnergy)
                CurrentEnergy = MaxEnergy;
        }
    }

    void DrawLaser(Vector3 EndPoint)
    {
        lineRenderer.SetPosition(0, laserStart.transform.position);
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, EndPoint);
        if (soundIsFading)
        {
            StopCoroutine("FadeOutSound");
            soundIsFading = false;
        }
        if (LaserAudioSource.isPlaying == false)
            LaserAudioSource.Play();
    }
    void DisableLaser()
    {
        lineRenderer.enabled = false;
        IsFiring = false;
        if (soundIsFading == false && LaserAudioSource.isPlaying)
            StartCoroutine(FadeOutSound(LaserAudioSource, SoundFadeDuration));
    }

    void Fire()
    {
        IsFiring = true;
        Ray FromCentreRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit FromCentreRayhit;
        Ray FromGunRay = new Ray(laserStart.transform.position, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10000)) - laserStart.transform.position);
        RaycastHit FromGunRayhit;

        if (Physics.Raycast(FromCentreRay, out FromCentreRayhit, 10000, -513))
        {
            FromGunRay = new Ray(laserStart.transform.position, FromCentreRayhit.point - laserStart.transform.position);
            Debug.DrawRay(laserStart.transform.position, FromCentreRayhit.point - laserStart.transform.position);
            if (Physics.Raycast(FromGunRay, out FromGunRayhit, 10000, -513))
            {
                DrawLaser(FromGunRayhit.point);
                if (FromGunRayhit.transform.gameObject.CompareTag("Enemy"))
                    FromGunRayhit.transform.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(LaserDamage * Time.deltaTime);
            }
            else
            {
                DrawLaser(FromCentreRayhit.point);
                if (FromCentreRayhit.transform.gameObject.CompareTag("Enemy"))
                    FromCentreRayhit.transform.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(LaserDamage * Time.deltaTime);
            }
        }
        else
        {
            if (Physics.Raycast(FromGunRay, out FromGunRayhit, 10000))
                DrawLaser(FromGunRayhit.point);
            else
                DrawLaser(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10000)));
        }
        CurrentEnergy = CurrentEnergy - Time.deltaTime * EnergyDrain;
        if (CurrentEnergy < 0)
            CurrentEnergy = 0;
    }

    void Update()
    {
        Recharge();
        if (Input.GetMouseButton(0))
            if ((IsFiring && CurrentEnergy > 0) || (IsFiring == false && CurrentEnergy > 5))
                Fire();
            else
                DisableLaser();
        else
            DisableLaser();
    }

    void OnValidate()
    {
        lineRenderer = laserStart.GetComponent<LineRenderer>();
        lineRenderer.startWidth = LaserWidth;
        lineRenderer.endWidth = LaserWidth;
        lineRenderer.startColor = LaserColor;
        lineRenderer.endColor = LaserColor;
        lineRenderer.numPositions = 2;
    }


    static bool soundIsFading;
    public static IEnumerator FadeOutSound(AudioSource audioSource, float FadeTime)
    {
        soundIsFading = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        soundIsFading = false;
    }
}
