using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesController : MonoBehaviour {

    public GameObject eyesTarget;
    public float eyeRadius;
    public GameObject leftPupil;
    public GameObject rightPupil;
    public GameObject centerOfLeftEye;
    public GameObject centerOfRightEye;
	
	void Update () {
        eyesFollowTarget();
    }

    void eyesFollowTarget()
    {
        leftEyeFollowTarget();
        rightEyeFollowTarget();
    }

    void leftEyeFollowTarget()
    {
        //distance from center of eyte to the target var distanceToTarget -> Vector3 = target.transform.position - eye.transform.position;
        Vector3 distanceToTarget = eyesTarget.transform.position - centerOfLeftEye.transform.position;
        //clamp distance so it never exceeds the size of the eyeball - > distanceToTarget = Vector3.ClampMagnitude(distanceToTarget(EyeRadius);
        distanceToTarget = Vector3.ClampMagnitude(distanceToTarget, eyeRadius);
        //place thje pupil at the desired position relative to the eyeball var finalPupilPosition : Vector3 = eye.transdform.position + disatnceToTarget; pupil.transform.posiiton - finalPuipilPosiiton;
        Vector3 finalLeftPupilPosition = centerOfLeftEye.transform.position + distanceToTarget;
        leftPupil.transform.position = finalLeftPupilPosition;
    }

    void rightEyeFollowTarget()
    {
        Vector3 distanceToTarget = eyesTarget.transform.position - centerOfRightEye.transform.position;
        //clamp distance so it never exceeds the size of the eyeball - > distanceToTarget = Vector3.ClampMagnitude(distanceToTarget(EyeRadius);
        distanceToTarget = Vector3.ClampMagnitude(distanceToTarget, eyeRadius);
        //place thje pupil at the desired position relative to the eyeball var finalPupilPosition : Vector3 = eye.transdform.position + disatnceToTarget; pupil.transform.posiiton - finalPuipilPosiiton;
        Vector3 finalRightPupilPosition = centerOfRightEye.transform.position + distanceToTarget;
        rightPupil.transform.position = finalRightPupilPosition;
    }
}
