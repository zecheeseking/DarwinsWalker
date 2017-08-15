using UnityEngine;
using System.Collections;

public class SplineViewSingleton : MonoBehaviour
{

    private static SplineViewSingleton instance = null;

    public static SplineViewSingleton Instance
    {
        get
        {
            return instance;
        }
    }

    public Camera display;
    public HermiteSplineMonoBehaviour startSpline;
    public HermiteSplineMonoBehaviour cyclicSpline;

    void Awake()
    {
        instance = this;
        display.gameObject.SetActive(false);
    }

    // Use this for initialization

    public void SetDisplaySplines(HermiteSpline startSpline, HermiteSpline cyclicSpline)
    {
        if(!display.gameObject.activeSelf)
            display.gameObject.SetActive(true);

        this.startSpline.Spline = startSpline;
        this.cyclicSpline.Spline = cyclicSpline;
        //_endSpline.Spline = endSpline;
    }
}
