using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseLook))]
public class AnimationController : MonoBehaviour
{
    [Header("Rig Objects")]
    public GameObject head;
    public GameObject torso;
    public GameObject rigBase;
    public GameObject hips;
    public GameObject rightArm;

    [Header("Limb Track Objects")]
    public GameObject trackPoint;

    [Header("Animation Parameters")]
    public float torsoAngle = 0;
    [Range(0, 1)]
    public float headlookAmount = 0.75f;
    private MouseLook mouseLook;
    private Quaternion rightArmRotation;
    private Vector3 rightArmPosition;

    private void Start()
    {
        mouseLook = transform.GetComponent<MouseLook>();
        rightArmRotation = rightArm.transform.localRotation;
    }

    public void trackArm(GameObject arm, Vector3 worldSpaceTarget, Quaternion startRotation)
    {
        //This needs adjustment
        arm.transform.localRotation = startRotation;
        Vector3 startFrom = -arm.transform.up;

        Vector3 localSpace = arm.transform.InverseTransformPoint(worldSpaceTarget);
        Vector3 axis = Vector3.Cross(localSpace, -startFrom);
        float angle = Vector3.Angle(localSpace, startFrom);

        arm.transform.Rotate(axis, angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (head != null)
        {
            head.transform.localRotation = Quaternion.Euler(0, torsoAngle, 0) * Quaternion.Euler(mouseLook.pitch * headlookAmount, 0, 0);
            torso.transform.localRotation = Quaternion.Euler(mouseLook.pitch * ( 1 - headlookAmount) + 180f, -90f, 0) * Quaternion.Euler(0, -torsoAngle, 0);
            hips.transform.localRotation = Quaternion.Euler(0, torsoAngle, 0);


            trackArm(rightArm, trackPoint.transform.position, rightArmRotation);
        }
    }
}
