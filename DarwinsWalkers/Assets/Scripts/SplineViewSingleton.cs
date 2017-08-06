using UnityEngine;
using System.Collections;

public class SplineViewSingleton : MonoBehaviour
{

    private static SplineViewSingleton _instance = null;

    public static SplineViewSingleton Instance
    {
        get
        {
            return _instance;
        }
    }

    private static Camera _display;
    private HermiteSplineVisualizer _startSpline;
    private HermiteSplineVisualizer _cyclicSpline;
    private HermiteSplineVisualizer _endSpline;

    void Awake()
    {
        _display = gameObject.GetComponentInChildren<Camera>();

        var splines = gameObject.GetComponentsInChildren<HermiteSpline>();

        _startSpline.Spline = splines[0];
        _cyclicSpline.Spline = splines[1];
        _endSpline.Spline = splines[2];

        _display.gameObject.SetActive(false);

        _instance = this;
    }

    // Use this for initialization

    public void SetDisplaySplines(HermiteSpline startSpline, HermiteSpline cyclicSpline, HermiteSpline endSpline)
    {
        if(!_display.gameObject.activeSelf)
            _display.gameObject.SetActive(true);

        _startSpline.Spline = startSpline;
        _cyclicSpline.Spline = cyclicSpline;
        _endSpline.Spline = endSpline;
    }
}
