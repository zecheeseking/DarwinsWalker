using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HermiteSplineControlPoint
{
    public HermiteSplineControlPoint(Vector3 pos, Vector3 tan)
    {
        this.Position = pos;
        this.Tangent = tan;
    }

    public Vector3 Position;
    public Vector3 Tangent;
}

public class HermiteSpline
{
    [SerializeField]
    private List<HermiteSplineControlPoint> splineControlPoints = new List<HermiteSplineControlPoint>();

    public int CurveCount
    {
        get
        {
            return splineControlPoints.Count - 1;
        }
    }

    public HermiteSpline(float[] splineCp)
    {
        for (int i = 0; i < splineCp.Length; i += 4)
        {
            splineControlPoints.Add(new HermiteSplineControlPoint(
                new Vector3(splineCp[i], splineCp[i + 1]),
                new Vector3(splineCp[i + 2], splineCp[i + 3])));
        }
    }

    public void SetControlPointAt(int i, Vector3 pos, Vector3 tanPos)
    {
        splineControlPoints[i] = new HermiteSplineControlPoint(pos, tanPos);
    }

    public int GetControlPointCount()
    {
        return splineControlPoints.Count;
    }

    public HermiteSplineControlPoint GetSplinePointAt(int i)
    {
        return splineControlPoints[i];
    }

    public void AddControlPoint()
    {
        var cp = splineControlPoints[splineControlPoints.Count - 1];
        splineControlPoints.Add(new HermiteSplineControlPoint(cp.Position + new Vector3(5,0,0), cp.Position + new Vector3(5, 0, 0) + new Vector3(1f,0,0)));
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = splineControlPoints.Count - 2;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
        }

        return Hermite.GetPoint(t, 
            splineControlPoints[i].Position, 
            splineControlPoints[i].Tangent, 
            splineControlPoints[i + 1].Position, 
            splineControlPoints[i + 1].Tangent);
    }

    public float SampleYCoordinate(float scalar)
    {
        var point = GetPoint(scalar);
        return point.y;
    }
}

public static class Hermite
{
    public static Vector3 GetPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (2.0f * Mathf.Pow(t, 3.0f) - 3.0f * Mathf.Pow(t, 2.0f) + 1) * p0 +
            (Mathf.Pow(t, 3.0f) - 2.0f * Mathf.Pow(t, 2.0f) + t) * p1 +
            (-2.0f * Mathf.Pow(t, 3.0f) + 3.0f * Mathf.Pow(t, 2.0f)) * p2 +
            (Mathf.Pow(t, 3.0f) - Mathf.Pow(t, 2.0f)) * p3;
    }
}