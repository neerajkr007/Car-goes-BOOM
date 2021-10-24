using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform car;
    private Vector3 offset = new Vector3(0f, 10f, 0f);
    public float cameraTiltOffset = 0f;

    float smoothnessFactor = 0.125f;
    void FixedUpdate()
    {
        Vector3 camPosi = car.position - car.transform.forward * offset.magnitude;
        Vector3 camRotation = car.eulerAngles;
        Vector3 lerpedPosi = Vector3.Lerp(transform.position, camPosi, smoothnessFactor);
        transform.position = new Vector3(lerpedPosi.x + cameraTiltOffset, offset.y, lerpedPosi.z);
        // camRotation.x = 50f;
        // transform.eulerAngles = camRotation;

        transform.LookAt(car);


    }

    private void LateUpdate()
    {
    }
}
