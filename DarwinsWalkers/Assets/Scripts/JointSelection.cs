using System;
using UnityEngine;
using System.Collections;

public class JointSelection : MonoBehaviour
{
    public GameObject Joint;

    public void RaycastHit()
    {
        var hingeControl = Joint.GetComponent<HingeControl>();

        if (!hingeControl)
            return;

        SplineViewSingleton.Instance.SetDisplaySplines(hingeControl.GetStartSpline(), 
            hingeControl.GetCyclicSpline());
    }
}
