using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct HermiteSplineControlPoint
{
    public Vector3 position;
    public Vector3 tangent;
}

public class HermiteSpline : MonoBehaviour
{
    public int InterpolationSteps = 5;
    public Color Color;

    [SerializeField]
    public List<HermiteSplineControlPoint> splineControlPoints = new List<HermiteSplineControlPoint>();
    private List<Vector3> _splinePoints = new List<Vector3>();
    private LineRenderer _lineRenderer;

    // Use this for initialization
    private void Awake()
    {
        RefreshSplinePoints();
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    public void Update()
    {
        if (!_lineRenderer)
            return;

        RefreshSplinePoints();
        _lineRenderer.SetVertexCount(_splinePoints.Count);
        _lineRenderer.SetPositions(_splinePoints.ToArray());
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
        var hcp = new HermiteSplineControlPoint();
        if(splineControlPoints.Count == 0)
            hcp.position = Vector3.zero;
        else
            hcp.position = splineControlPoints[splineControlPoints.Count - 1].position + Vector3.right * 3.0f;

        hcp.tangent = hcp.position + Vector3.right;
        splineControlPoints.Add(hcp);
    }

    public void RemoveControlPoint()
    {
        splineControlPoints.RemoveAt(splineControlPoints.Count - 1);
    }

    public void RefreshSplinePoints()
    {
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

    public void GenerateRandomSpline(int controlPoints)
    {
        splineControlPoints.Clear();
        for(int i = 0; i < controlPoints; ++i)
        {
            HermiteSplineControlPoint tmp = new HermiteSplineControlPoint();
            if (i == 0)
                tmp.position = new Vector3(0, UnityEngine.Random.Range(-5.0f, 5.0f), 0);
            else
                tmp.position = new Vector3(splineControlPoints[i - 1].position.x + UnityEngine.Random.Range(0.5f, 5.0f), UnityEngine.Random.Range(-5.0f, 5.0f), 0);

            tmp.tangent = tmp.position;

            splineControlPoints.Add(tmp);
        }

        HermiteSplineControlPoint tmpEnd = new HermiteSplineControlPoint();
        tmpEnd.position = new Vector3(splineControlPoints[splineControlPoints.Count - 1].position.x + UnityEngine.Random.Range(0.5f, 5.0f), 
            UnityEngine.Random.Range(-5.0f, 5.0f), 0);
        tmpEnd.tangent = tmpEnd.position;

        splineControlPoints.Add(tmpEnd);
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

            if(lastPos != Vector3.zero)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lastPos, _splinePoints[i]);
            }
            lastPos = _splinePoints[i];
        }
        if(splineControlPoints.Count > 1)
            Gizmos.DrawLine(splineControlPoints[splineControlPoints.Count - 1].position, lastPos);
    }
}
