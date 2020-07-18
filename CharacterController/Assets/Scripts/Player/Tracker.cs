using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform trackTarget;

    void Update()
    {
        transform.rotation = Quaternion.identity;

        Vector3 difference = transform.position - trackTarget.position;
        Vector3 rotationAxis = Vector3.Cross(-difference, Vector3.down).normalized;
        float angle = Vector3.Angle(difference, Vector3.down);

        transform.Rotate(rotationAxis, angle, Space.World);
    }
}
