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
    public GameObject leftArm;

    [Header("Limb Track Objects")]
    public GameObject trackPoint;

    [Header("Animation Parameters")]
    public float torsoAngle = 0;
    [Range(0, 1)]
    public float headlookAmount = 0.75f;
    [Range(0, 1)]
    public float armOffsetLimit;

    private const float limbLength = 2;
    private MouseLook mouseLook;
    //Right arm
    private Quaternion rightArmRotation;
    private Vector3 rightArmPosition;
    //Left arm
    private Quaternion leftArmRotation;
    private Vector3 leftArmPosition;

    private void Start()
    {
        mouseLook = transform.GetComponent<MouseLook>();
        rightArmRotation = rightArm.transform.localRotation;
        rightArmPosition = rightArm.transform.localPosition;
        leftArmRotation = leftArm.transform.localRotation;
        leftArmPosition = leftArm.transform.localPosition;
    }

    public void trackArm(GameObject arm, Vector3 worldSpaceTarget, Quaternion startRotation, Vector3 startPosition)
    {
        arm.transform.localPosition = startPosition;
        arm.transform.localRotation = startRotation;

        Vector3 startFrom = arm.transform.InverseTransformDirection(arm.transform.up);

        Vector3 localSpace = arm.transform.InverseTransformPoint(worldSpaceTarget);
        Vector3 axis = Vector3.Cross(localSpace, -startFrom);
        float angle = Vector3.Angle(localSpace, startFrom);

        float distFromHand = (arm.transform.position - worldSpaceTarget).magnitude - limbLength / 4;
        distFromHand = Mathf.Clamp(distFromHand, -armOffsetLimit / 2, armOffsetLimit / 2);
        

        arm.transform.Rotate(axis, angle);
        arm.transform.position -= distFromHand * (arm.transform.position - worldSpaceTarget).normalized;
    }

    public void trackLeg(GameObject leg, Vector3 worldSpaceTarget, Quaternion startRotation, Vector3 startPosition)
    {
        leg.transform.localPosition = startPosition;
        leg.transform.localRotation = startRotation;

        Vector3 startFrom = leg.transform.InverseTransformDirection(leg.transform.up);

        Vector3 localSpace = leg.transform.InverseTransformPoint(worldSpaceTarget);
        Vector3 axis = Vector3.Cross(localSpace, -startFrom);
        float angle = Vector3.Angle(localSpace, startFrom);

        leg.transform.Rotate(axis, angle);
    }

    // Update is called once per frame
    void Update()
    {
        if (head != null)
        {
            head.transform.localRotation = Quaternion.Euler(0, torsoAngle, 0) * Quaternion.Euler(mouseLook.pitch * headlookAmount, 0, 0);
            torso.transform.localRotation = Quaternion.Euler(mouseLook.pitch * ( 1 - headlookAmount) + 180f, -90f, 0) * Quaternion.Euler(0, -torsoAngle, 0);
            hips.transform.localRotation = Quaternion.Euler(0, torsoAngle, 0);


            trackArm(rightArm, trackPoint.transform.position, rightArmRotation, rightArmPosition);
            //trackArm(leftArm, trackPoint.transform.position, leftArmRotation, leftArmPosition);
        }
    }
}
