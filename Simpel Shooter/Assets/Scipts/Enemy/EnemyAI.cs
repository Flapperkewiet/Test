using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    // Refrences
    NavMeshAgent agent;
    GameObject player;
    EnemyHealth enemyHealth;
    AudioSource audioSource;
    public GameObject LeftGun;
    public GameObject RigthGun;

    [Header("Movement")]
    public float RotationSpeed;
    public float MovementSpeed;

    [Header("Shooting")]
    public GameObject BulletPrefab;
    public Transform[] BulletStarts;
    public float TimeBetweenBullets;
    public float ShootForce;
    float shootTimer;

    [Header("AI")]
    public float ViewRange;
    float ViewRangeSqrd;
    public float ViewAngle;
    public float AIUpdatesPerSecond;
    float updateRateTimer;
    public Transform Eyes;
    public LayerMask CanSeePlayerMask;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (Settings.Player == null)
            Settings.Player = GameObject.Find("Player");
        audioSource = GetComponent<AudioSource>();
        player = Settings.Player;
        enemyHealth = GetComponent<EnemyHealth>();
        agent.speed = MovementSpeed;
        ViewRangeSqrd = ViewRange * ViewRange;
    }

    public bool _seesPlayer;
    public bool SeesPlayer
    {
        get { return _seesPlayer; }
        set
        {
            if (value == true && _seesPlayer == false)
                OnSeesPlayer();
            _seesPlayer = value;
        }
    }


    void Update()
    {
        if (enemyHealth.IsDead)
            return;
        updateRateTimer += Time.deltaTime;
        if (updateRateTimer > 1.0f / AIUpdatesPerSecond)
        {
            updateRateTimer = 0;
            AIUpdate();
        }
        shootTimer += Time.deltaTime;
    }

    void OnSeesPlayer()
    {
        agent.SetDestination(transform.position);
    }


    void AIUpdate()
    {
        SeesPlayer = CanSeePlayer();
        if (SeesPlayer == false)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), Time.deltaTime * RotationSpeed);
            Vector3 rot = transform.rotation.eulerAngles;
            rot.x = 0;
            rot.z = 0;
            transform.rotation =  Quaternion.Euler(rot);
            Shoot();
        }
    }

    int shootIndex;
    void Shoot()
    {
        Transform BulletStart = BulletStarts[shootIndex];
        if (shootTimer > TimeBetweenBullets)
        {
            RaycastHit hit;
            Ray ray = new Ray(BulletStart.position, player.transform.position - BulletStart.position);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    if (shootIndex == 0)
                        RigthGun.GetComponent<Animation>().Play();
                    else if(shootIndex == 1)
                        LeftGun.GetComponent<Animation>().Play();

                    shootIndex++;
                    if (shootIndex >= BulletStarts.Length)
                        shootIndex = 0;

                    shootTimer = 0;
                    GameObject Bullet = Instantiate(BulletPrefab, BulletStart.transform.position, Quaternion.identity);
                    Bullet.GetComponent<Rigidbody>().AddForce((BulletStart.transform.forward).normalized * ShootForce);
                    audioSource.Play();
                }
            }
        }
    }

    void LookAtPlayer()
    {
        transform.LookAt(player.transform.position);
    }

    public bool CanSeePlayer()
    {
        if (FastDistanceSquared(player.transform.position, transform.position) > ViewRangeSqrd)
            return false;
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) > ViewAngle / 2)
            return false;
        RaycastHit hit;
        Ray ray = new Ray(Eyes.position, player.transform.position - Eyes.position);
        if (Physics.Raycast(ray, out hit, CanSeePlayerMask))
            if (hit.transform.gameObject.CompareTag("Player"))
                return true;
        return false;
    }

    public float FastDistanceSquared(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }
}
