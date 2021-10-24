using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTurretController : MonoBehaviour
{

    public ParticleSystem fireFX;
    public float FireRate = 1f;
    public Transform Shootpoint;
    public float Force = 150f;
    float nextTimeToFire = 0;
    BulletPool bulletPool;
    GameManager gameManager;
    bool isShooting = false;
    public int ammo { get; private set; }
    public int maxAmmo { get; private set; }

    void Start()
    {
        maxAmmo = 20;
        ammo = maxAmmo;
        bulletPool = BulletPool.Instance;
        gameManager = GameManager.Instance;
    }

    public void startFiring(bool value)
    {
        isShooting = value;
    }

    void Update()
    {
        if(isShooting)
        {
            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1 / FireRate;
                shoot();
            }
        }
    }

    void shoot()
    {
        if(ammo != 0)
        {
            ammo--;
            gameManager.ammo.text = "ammo : " + ammo;
            Vector3 direction = transform.forward.normalized;
            GameObject BulletIns = bulletPool.spawnFromPool("bullets", Shootpoint, direction);
            BulletIns.GetComponent<Rigidbody>().AddForce(direction * Force);
            fireFX.Play(false);
        }
        
    }

    public void increaseAmmo(int size)
    {
        if(size == 1)
        {
            if (ammo <= 15)
            {
                ammo += 5;
            }
            else
            {
                ammo = 20;
            }
        }
        else
        {
            if (ammo <= 10)
            {
                ammo += 10;
            }
            else
            {
                ammo = 20;
            }
        }
        gameManager.ammo.text = "ammo : " + ammo;
    }
}
