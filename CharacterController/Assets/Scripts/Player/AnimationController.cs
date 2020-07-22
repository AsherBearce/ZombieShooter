using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject leftFootTarget;
    public GameObject rightFootTarget;
    public GameObject leftFootIdeal;
    public GameObject rightFootIdeal;

    [Header("Animation Parameters")]
    public float torsoAngle = 0;
    [Range(0, 1)]
    public float headlookAmount = 0.75f;
    [Range(0, 1)]
    public float armOffsetLimit;
    [Range(0, 1)]
    public float legDistanceLimit = 1;
    public float strideLength;
    public AnimationCurve stepCurve;
    public float stepHeight;

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
        distFromHand = Mathf.Clamp(distFromHand, -armOffsetLimit / 2, 0);
        

        arm.transform.Rotate(axis, angle);
        arm.transform.position -= distFromHand * (arm.transform.position - worldSpaceTarget).normalized;
    }

    public void trackLeg(GameObject leg, Vector3 worldSpaceTarget, Quaternion startRotation, Vector3 startPosition)
    {
        leg.transform.localPosition = startPosition;
        leg.transform.localRotation = startRotation;

        //This needs fixing too ;-;

        if (legDistanceLimit > 0)
        {
            float distance = (leg.transform.position.y - worldSpaceTarget.y) - limbLength / 4;
            distance = Mathf.Clamp(distance, -legDistanceLimit / 2, 0);

            Vector3 legOffset = -distance * (torso.transform.forward + torso.transform.up);
            leg.transform.position += legOffset;
        }

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

            //trackArm(rightArm, trackPoint.transform.position, rightArmRotation, rightArmPosition);
            //trackArm(leftArm, trackPoint.transform.position, leftArmRotation, leftArmPosition);

            //trackLeg(leftLeg, trackPoint.transform.position, leftLegRotation, leftLegPosition);

            rightFootIdeal.transform.position = rigBase.transform.position + rigBase.transform.forward * 0.125f;
            float rDist = (rightFootIdeal.transform.position - rightFootTarget.transform.position).magnitude;
            float strideMultiplier = movementController.getMovementVelocity().magnitude > 0.001f ? 1f : 0.1f;

            //Add animation for this later 
            if (!movementController.grounded())
            {
                rightFootTarget.transform.position = rightFootIdeal.transform.position;
            }

            if (rDist > strideMultiplier * strideLength && !rightFootTarget.GetComponent<Interpolator>().isLerping)
            {
                Vector3 to = rightFootIdeal.transform.position + movementController.getMovementVelocity();
                rightFootTarget.GetComponent<Interpolator>().startLerp(rightFootTarget.transform.position, to, stepCurve, 0.1f);
            }

            trackLeg(rightLeg, rightFootTarget.transform.position, rightLegRotation, rightLegPosition);
        }
    }
}
