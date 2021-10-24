using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCarModel : MonoBehaviour
{
    [SerializeField]
    float angle = 25f;
    void Update()
    {
        transform.RotateAround(transform.position, new Vector3(0, transform.GetChild(2).position.y, 0), Time.deltaTime * angle);
    }
}
