using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: add custom timeline and quaternion interpolation system

[RequireComponent(typeof(MouseLook))]
[RequireComponent(typeof(PlayerMovement))]
public class AnimationController : MonoBehaviour
{
    [Header("Rig Objects")]
    public GameObject head;
    public GameObject torso;
    public GameObject rigBase;
    public GameObject hips;
    public GameObject rightArm;
    public GameObject leftArm;
    public GameObject rightLeg;
    public GameObject leftLeg;

    [Header("Limb Track Objects")]
    public GameObject leftArmTracker;
    public GameObject rightArmTracker;

    [Header("Animation Parameters")]
    public float torsoAngle = 0;
    [Range(0, 1)]
    public float headlookAmount = 0.75f;
    [Range(0, 1)]
    public float armOffsetLimit;

    private const float limbLength = 2;
    private MouseLook mouseLook;
    private PlayerMovement movementController;
    //Right arm
    private Quaternion rightArmRotation;
    private Vector3 rightArmPosition;
    //Left arm
    private Quaternion leftArmRotation;
    private Vector3 leftArmPosition;
    //Right leg
    private Quaternion rightLegRotation;
    private Vector3 rightLegPosition;
    //Right leg
    private Quaternion leftLegRotation;
    private Vector3 leftLegPosition;

    private void Start()
    {
        mouseLook = transform.GetComponent<MouseLook>();
        movementController = transform.GetComponent<PlayerMovement>();

        rightArmRotation = rightArm.transform.localRotation;
        rightArmPosition = rightArm.transform.localPosition;

        leftArmRotation = leftArm.transform.localRotation;
        leftArmPosition = leftArm.transform.localPosition;

        rightLegRotation = rightLeg.transform.localRotation;
        rightLegPosition = rightLeg.transform.localPosition;

        leftLegRotation = leftLeg.transform.localRotation;
        leftLegPosition = leftLeg.transform.localPosition;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 0.1f;
        }

        if (head != null)
        {
            head.transform.localRotation = Quaternion.Euler(0, torsoAngle, 0) * Quaternion.Euler(mouseLook.pitch * headlookAmount, 0, 0);
            torso.transform.localRotation = Quaternion.Euler(mouseLook.pitch * ( 1 - headlookAmount) + 180f, -90f, 0) * Quaternion.Euler(0, -torsoAngle, 0);
            hips.transform.localRotation = Quaternion.Euler(0, torsoAngle, 0);
            rightLeg.transform.localRotation = rightLegRotation;
            leftLeg.transform.localRotation = leftLegRotation;

            trackArm(rightArm, rightArmTracker.transform.position, rightArmRotation, rightArmPosition);
            trackArm(leftArm, leftArmTracker.transform.position, leftArmRotation, leftArmPosition);

            Vector3 axis = Vector3.Cross(movementController.getMovementVelocity(), Vector3.up);
            axis = hips.transform.InverseTransformDirection(axis);
            float targetAngle = 0;

            if (movementController.getMovementVelocity().magnitude < 0.9f)
            {
                targetAngle = Mathf.Lerp(targetAngle, 0, 0.5f);
            }
            else
            {
                targetAngle = 45f * Mathf.Cos(10 * Time.time);
            }
            rightLeg.transform.Rotate(axis, targetAngle);
            leftLeg.transform.Rotate(axis, -targetAngle);
        }
    }
}
