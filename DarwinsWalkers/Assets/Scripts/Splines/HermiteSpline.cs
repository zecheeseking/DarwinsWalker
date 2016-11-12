using System;
using System.Collections.Generic;
using UnityEngine;


public struct HermiteSplineParameters
{
    public Vector3 StartPoint;//p0
    public Vector3 StartControlPoint; //p1
    public Vector3 EndPoint; //p3
    public Vector3 EndControlPoint; //p2
}

[Serializable]
public struct HermiteSplineControlPoint
{
    public Vector3 position;
    public Vector3 tangent;
}

public class HermiteSpline : MonoBehaviour
{
    public HermiteSplineParameters Parameters;
    public int InterpolationSteps = 5;

    [SerializeField]
    public List<HermiteSplineControlPoint> splineControlPoints = new List<HermiteSplineControlPoint>();
    private List<Vector3> _splinePoints = new List<Vector3>();

    // Use this for initialization
    private void Start()
    {
        RefreshSplinePoints();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public Vector3 CalculateSplinePoint(float t, HermiteSplineControlPoint p0, HermiteSplineControlPoint p1)
    {
        Vector3 sp = (2.0f * Mathf.Pow(t, 3.0f) - 3.0f * Mathf.Pow(t, 2.0f) + 1) * p0.position +
            (Mathf.Pow(t, 3.0f) - 2.0f * Mathf.Pow(t, 2.0f) + t) * p0.tangent +
            (-2.0f * Mathf.Pow(t, 3.0f) + 3.0f * Mathf.Pow(t, 2.0f)) * p1.position +
            (Mathf.Pow(t, 3.0f) - Mathf.Pow(t, 2.0f)) * p1.tangent;

        return sp;
    }

    public void AddControlPoint()
    {
        HermiteSplineControlPoint hcp = new HermiteSplineControlPoint();
        hcp.position = splineControlPoints[splineControlPoints.Count - 1].position + Vector3.right * 10.0f;
        hcp.tangent = hcp.position + Vector3.up * 4.0f;
        splineControlPoints.Add(hcp);
    }

    public void RefreshSplinePoints()
    {
        var deltaT = 1.0f / (InterpolationSteps - 1);
        _splinePoints.Clear();

        _splinePoints.Add(splineControlPoints[0].position);

        for (var i = 1; i < splineControlPoints.Count; ++i)
        {
            for (var ii = 1; ii < InterpolationSteps - 1; ++ii)
            {
                var t = (float)ii / (InterpolationSteps - 1);
                var p = CalculateSplinePoint(t, splineControlPoints[i - 1], splineControlPoints[i]);
                _splinePoints.Add(p);
            }
            _splinePoints.Add(splineControlPoints[i].position);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 lastPos = Vector3.zero;
        int cpCount = 0;

        for(int i = 0; i < _splinePoints.Count; ++i)
        {
            if (i % (InterpolationSteps - 1) == 0)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(_splinePoints[i], splineControlPoints[cpCount].tangent);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_splinePoints[i], .3f);
                cpCount++;
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_splinePoints[i], .2f);
            }

            if(lastPos != Vector3.zero)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lastPos, _splinePoints[i]);
            }
            lastPos = _splinePoints[i];
        }
        //Gizmos.DrawLine(Parameters.EndControlPoint, lastPos);
    }
}
