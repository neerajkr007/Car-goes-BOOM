using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretController : MonoBehaviour
{

    public Transform target;
    public float range = 10f;
    public ParticleSystem fireFX;

    private GameObject gun;
    [SerializeField]
    bool detected = false;
    Vector3 direction;

    public float FireRate = 1f;
    float nextTimeToFire = 0;
    public Transform Shootpoint;
    public float Force = 150f;
    public Slider healthSlider;
    GameManager gameManager;

    [SerializeField]
    float health = 100f;

    BulletPool bulletPool;

    void Start()
    {
        gameManager = GameManager.Instance;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        gun = transform.GetChild(0).gameObject;
        bulletPool = BulletPool.Instance;
        healthSlider.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void Update()
    {
        Vector3 lookAtPosition = target.transform.position;
        lookAtPosition.y = healthSlider.transform.position.y;
        healthSlider.transform.parent.LookAt(lookAtPosition);
        Vector3 targetPos = target.position;
        direction = targetPos - transform.position;
        direction.y = 0.7f;
        //print(transform.position + "  " + direction + "  " + target.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            //print("ray sent " + range);
            //Debug.DrawRay(transform.position, direction, Color.green);
            if (hit.collider.CompareTag("Player"))
            {
                detected = true;
            }
            else
            {
                detected = false;
            }
        }
        else
        {
            detected = false;
        }
        if (detected)
        {
            //print("detected");
            gun.transform.forward = direction;
            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1 / FireRate;
                shoot();
            }
        }
    }

    void shoot()
    {
        GameObject BulletIns = bulletPool.spawnFromPool("bullets", Shootpoint, direction);
        BulletIns.GetComponent<Rigidbody>().AddForce(direction * Force);
        fireFX.Play(false);
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void turretHit()
    {
        if(health > 0)
        {
            healthSlider.value -= 0.15f;
            health -= 15f;
        }
        else
        {
            transform.parent = transform.parent.parent.GetChild(transform.parent.GetSiblingIndex() + 1);
            gameObject.SetActive(false);
        }
    }
}
