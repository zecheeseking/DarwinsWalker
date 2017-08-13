using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteSplineMonoBehaviour : MonoBehaviour
{

    public HermiteSpline Spline;

	// Use this for initialization
	void Awake ()
    {
        Spline = new HermiteSpline(new [] {0f,0f,1f,0f,5f,5f,6f,5f});
	}

    public void AddControlPoint()
    {
        Spline.AddControlPoint();
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Spline.GetPoint(t));
    }
}
