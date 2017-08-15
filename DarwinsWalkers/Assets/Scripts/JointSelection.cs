using System;
using UnityEngine;
using System.Collections;

public class JointSelection : MonoBehaviour
{

    public GameObject Joint;

    void RaycastHit()
    {
        var hingeControl = Joint.GetComponent<HingeControl>();

        if (!hingeControl)
            return;

        //SplineViewSingleton.Instance.SetDisplaySplines(hingeControl.ForceSplines[0], 
        //    hingeControl.ForceSplines[1], 
        //    hingeControl.ForceSplines[2]);
    }
}
