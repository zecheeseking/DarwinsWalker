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
[ExecuteInEditMode]
public class HermiteSpline : MonoBehaviour
{
    public int InterpolationSteps = 5;
    public Color Color;

    [SerializeField]
    private List<HermiteSplineControlPoint> _splineControlPoints = new List<HermiteSplineControlPoint>();
    private List<Vector3> _splinePoints = new List<Vector3>();
    private LineRenderer _lineRenderer;

    // Use this for initialization
    private void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    public void SetControlPointAt(int i, Vector3 pos, Vector3 tanPos)
    {
        _splineControlPoints[i] = new HermiteSplineControlPoint(pos, tanPos - pos);
    }

    public int GetControlPointCount()
    {
        return _splineControlPoints.Count;
    }

    public HermiteSplineControlPoint GetSplinePointAt(int i)
    {
        return _splineControlPoints[i];
    }

    public void AddControlPoint(Vector3 pos, Vector3 tan)
    {
        _splineControlPoints.Add(new HermiteSplineControlPoint(pos, tan));
    }
    public void RemoveControlPoint()
    {
        _splineControlPoints.RemoveAt(_splineControlPoints.Count - 1);
    }

    public void Update()
    {
        if (!_lineRenderer)
            return;

        if (_splineControlPoints.Count != 0)
        {
            RefreshSplinePoints();
            _lineRenderer.SetVertexCount(_splinePoints.Count);
            _lineRenderer.SetPositions(_splinePoints.ToArray());
        }
    }

    public Vector3 CalculateSplinePoint(float t, HermiteSplineControlPoint p0, HermiteSplineControlPoint p1)
    {
        Vector3 sp = (2.0f * Mathf.Pow(t, 3.0f) - 3.0f * Mathf.Pow(t, 2.0f) + 1) * p0.Position +
            (Mathf.Pow(t, 3.0f) - 2.0f * Mathf.Pow(t, 2.0f) + t) * p0.Tangent +
            (-2.0f * Mathf.Pow(t, 3.0f) + 3.0f * Mathf.Pow(t, 2.0f)) * p1.Position +
            (Mathf.Pow(t, 3.0f) - Mathf.Pow(t, 2.0f)) * p1.Tangent;

        return sp;
    }

    public void RefreshSplinePoints()
    {
        _splinePoints.Clear();

        _splinePoints.Add(_splineControlPoints[0].Position);

        for (var i = 1; i < _splineControlPoints.Count; ++i)
        {
            for (var ii = 1; ii < InterpolationSteps - 1; ++ii)
            {
                var t = (float)ii / (InterpolationSteps - 1);
                var p = CalculateSplinePoint(t, _splineControlPoints[i - 1], _splineControlPoints[i]);
                _splinePoints.Add(p);
            }
            _splinePoints.Add(_splineControlPoints[i].Position);
        }
    }

    public void GenerateRandomSpline(int controlPoints)
    {
        _splineControlPoints.Clear();
        for(int i = 0; i < controlPoints - 1; ++i)
        {
            HermiteSplineControlPoint tmp = new HermiteSplineControlPoint();
            if (i == 0)
                tmp.Position = new Vector3(0, UnityEngine.Random.Range(-5.0f, 5.0f), 0);
            else
                tmp.Position = new Vector3(_splineControlPoints[i - 1].Position.x + UnityEngine.Random.Range(2.5f, 8.0f), UnityEngine.Random.Range(-5.0f, 5.0f), 0);

            tmp.Tangent = tmp.Position;

            _splineControlPoints.Add(tmp);
        }

        HermiteSplineControlPoint tmpEnd = new HermiteSplineControlPoint();
        tmpEnd.Position = new Vector3(_splineControlPoints[_splineControlPoints.Count - 1].Position.x + UnityEngine.Random.Range(2.5f, 8.0f), 
            UnityEngine.Random.Range(-5.0f, 5.0f), 0);
        tmpEnd.Tangent = tmpEnd.Position;

        _splineControlPoints.Add(tmpEnd);
    }

    public void SampleYCoordinate(float scalar)
    {
        
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 lastPos = Vector3.zero;
    //    int cpCount = 0;

    //    for(int i = 0; i < _splinePoints.Count; ++i)
    //    {
    //        if (i % (InterpolationSteps - 1) == 0)
    //        {
    //            Gizmos.color = Color.white;
    //            Gizmos.DrawLine(_splinePoints[i], splineControlPoints[cpCount].tangent);
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawWireSphere(_splinePoints[i], .3f);
    //            cpCount++;
    //        }

    //        if(lastPos != Vector3.zero)
    //        {
    //            Gizmos.color = Color.white;
    //            Gizmos.DrawLine(lastPos, _splinePoints[i]);
    //        }
    //        lastPos = _splinePoints[i];
    //    }
    //    if(splineControlPoints.Count > 1)
    //        Gizmos.DrawLine(splineControlPoints[splineControlPoints.Count - 1].position, lastPos);
    //}
}
