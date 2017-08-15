using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermiteSplineMonoBehaviour : MonoBehaviour
{

    public HermiteSpline Spline;

    public void SetSpline(HermiteSpline spline)
    {
        Spline = spline;
        RefreshSplinePoints();
    }

	// Use this for initialization
	void Awake ()
    {
	}

    void RefreshSplinePoints()
    {
        var lineRender = GetComponent<LineRenderer>();
        int lineSteps = 25;

        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i <= lineSteps; i++)
        {
            positions.Add(GetPoint(i / (float)lineSteps));
        }

        lineRender.numPositions = positions.Count;
        lineRender.SetPositions(positions.ToArray());
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
