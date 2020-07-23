using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator : MonoBehaviour
{
    private Vector3 lerpFrom = Vector3.zero;
    private Vector3 lerpTo = Vector3.zero;
    private float lerpTime = 0;
    private bool shouldLerp = false;
    private float timeStarted;
    private AnimationCurve yDisplacementCurve;
    public bool isLerping { get; private set; }

    public void startLerp(Vector3 from, Vector3 to, AnimationCurve displacement, float time = 1)
    {
        shouldLerp = true;
        lerpFrom = from;
        lerpTo = to;
        lerpTime = time;
        timeStarted = Time.time;
        yDisplacementCurve = displacement;
        isLerping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLerp)
        {
            float timeSinceStarted = Time.time - timeStarted;
            float percent = timeSinceStarted / lerpTime;
            Vector3 verticalDisplacement = yDisplacementCurve != null ? Vector3.up * yDisplacementCurve.Evaluate(percent) * 0.3f : Vector3.zero;

            transform.position = Vector3.Lerp(lerpFrom, lerpTo, percent) + verticalDisplacement;

            if (percent >= 1)
            {
                isLerping = false;
                shouldLerp = false;
                transform.position = lerpTo;
            }
        }
    }
}
