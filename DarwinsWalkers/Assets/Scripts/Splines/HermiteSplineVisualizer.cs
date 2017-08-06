using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;


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

public class HermiteSplineVisualizer : MonoBehaviour
{
    public Color Color;
    private LineRenderer lineRenderer;

    public int InterpolationSteps = 10;

    private HermiteSpline spline;
    public HermiteSpline Spline
    {
        get { return spline; }
        set
        {
            spline = value;
            if (spline != null)
            {
                List<Vector3> points = new List<Vector3>();
                float t = 0.0f;
                for (int i = 0; i < InterpolationSteps; i++)
                {
                    t = 1.0f / InterpolationSteps * i;
                    points.Add(spline.CalculateSplinePoint(t));
                }

                lineRenderer.SetPositions(points.ToArray());
            }
        }
    }

    void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        if (lineRenderer)
        {
            lineRenderer.startColor = Color;
            lineRenderer.endColor = Color;
        }
    }
}

public class HermiteSpline
{
    [SerializeField]
    private List<HermiteSplineControlPoint> splineControlPoints = new List<HermiteSplineControlPoint>();

    public HermiteSpline(float[] splineCp)
    {
        Debug.LogWarning("STILL GOTTA SETUP THIS ONCE GENOTYPE LAYOUT IS FINALIZED.");
        splineControlPoints.Add(new HermiteSplineControlPoint(Vector3.zero, Vector3.forward));
        splineControlPoints.Add(new HermiteSplineControlPoint(Vector3.forward, Vector3.forward));
    }

    public void SetControlPointAt(int i, Vector3 pos, Vector3 tanPos)
    {
        splineControlPoints[i] = new HermiteSplineControlPoint(pos, tanPos - pos);
    }

    public int GetControlPointCount()
    {
        return splineControlPoints.Count;
    }

    public HermiteSplineControlPoint GetSplinePointAt(int i)
    {
        return splineControlPoints[i];
    }

    public void AddControlPoint(Vector3 pos, Vector3 tan)
    {
        splineControlPoints.Add(new HermiteSplineControlPoint(pos, tan));
    }

    public Vector3 CalculateSplinePoint(float t)
    {
        Vector3 sp = (2.0f * Mathf.Pow(t, 3.0f) - 3.0f * Mathf.Pow(t, 2.0f) + 1) * splineControlPoints[0].Position +
            (Mathf.Pow(t, 3.0f) - 2.0f * Mathf.Pow(t, 2.0f) + t) * splineControlPoints[0].Tangent +
            (-2.0f * Mathf.Pow(t, 3.0f) + 3.0f * Mathf.Pow(t, 2.0f)) * splineControlPoints[1].Position +
            (Mathf.Pow(t, 3.0f) - Mathf.Pow(t, 2.0f)) * splineControlPoints[1].Tangent;

        return sp;
    }

    //public void RefreshSplinePoints()
    //{
        //_splinePoints.Clear();

        //_splinePoints.Add(_splineControlPoints[0].Position);

        //for (var i = 1; i < _splineControlPoints.Count; ++i)
        //{
        //    for (var ii = 1; ii < InterpolationSteps - 1; ++ii)
        //    {
        //        var t = (float)ii / (InterpolationSteps - 1);
        //        var p = CalculateSplinePoint(t, _splineControlPoints[i - 1], _splineControlPoints[i]);
        //        _splinePoints.Add(p);
        //    }
        //    _splinePoints.Add(_splineControlPoints[i].Position);
        //}
    //}

    //public void RandomSpline(int controlPoints)
    //{
    //    _splineControlPoints.Clear();
    //    for(int i = 0; i < controlPoints - 1; ++i)
    //    {
    //        HermiteSplineControlPoint tmp = new HermiteSplineControlPoint();
    //        if (i == 0)
    //            tmp.Position = new Vector3(0, UnityEngine.Random.Range(-15.0f, 15.0f), 0);
    //        else
    //            tmp.Position = new Vector3(_splineControlPoints[i - 1].Position.x + UnityEngine.Random.Range(2.5f, 8.0f), UnityEngine.Random.Range(-5.0f, 5.0f), 0);

    //        tmp.Tangent = tmp.Position;

    //        _splineControlPoints.Add(tmp);
    //    }

    //    HermiteSplineControlPoint tmpEnd = new HermiteSplineControlPoint();
    //    tmpEnd.Position = new Vector3(_splineControlPoints[_splineControlPoints.Count - 1].Position.x + UnityEngine.Random.Range(2.5f, 8.0f), 
    //        UnityEngine.Random.Range(-5.0f, 5.0f), 0);
    //    tmpEnd.Tangent = tmpEnd.Position;

    //    _splineControlPoints.Add(tmpEnd);
    //}

    public float SampleYCoordinate(float scalar)
    {
        var point = CalculateSplinePoint(scalar);
        return point.y;
    }
}