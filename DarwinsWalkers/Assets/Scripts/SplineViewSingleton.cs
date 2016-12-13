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
    private HermiteSpline _startSpline;
    private HermiteSpline _cyclicSpline;
    private HermiteSpline _endSpline;

    void Awake()
    {
        _display = gameObject.GetComponentInChildren<Camera>();

        var splines = gameObject.GetComponentsInChildren<HermiteSpline>();

        _startSpline = splines[0];
        _cyclicSpline = splines[1];
        _endSpline = splines[2];

        _display.gameObject.SetActive(false);

        _instance = this;
    }

    // Use this for initialization

    public void SetDisplaySplines(HermiteSpline startSpline, HermiteSpline cyclicSpline, HermiteSpline endSpline)
    {
        if(!_display.gameObject.activeSelf)
            _display.gameObject.SetActive(true);

        _startSpline.CopyFromSpline(startSpline);
        _cyclicSpline.CopyFromSpline(cyclicSpline);
        _endSpline.CopyFromSpline(endSpline);
    }

    // Update is called once per frame
    void Update () {
	    _startSpline.VisualizeSpline();
        _cyclicSpline.VisualizeSpline();
        _endSpline.VisualizeSpline();
    }
}
