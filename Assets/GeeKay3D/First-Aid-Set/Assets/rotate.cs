using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField]
    float angle = 25f;
    void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, transform.position.y, 0), Time.deltaTime * angle);
    }
}
