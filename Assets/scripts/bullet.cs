using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    BulletPool bulletPool;

    void Start()
    {
        bulletPool = BulletPool.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.name == "Plane" || collision.collider.name.Contains("FattyCannon"))
        {
            bulletPool.destroyPoolBullet("bullets", gameObject);
        }
        if(collision.collider.name.Contains("FattyCannon"))
        {
            collision.collider.GetComponent<TurretController>().turretHit();
        }
    }
}
