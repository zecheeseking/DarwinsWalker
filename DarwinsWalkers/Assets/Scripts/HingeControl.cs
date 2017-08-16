using UnityEngine;
using System.Collections;

public class HingeControl : MonoBehaviour
{
    private HingeJoint2D _hinge;
    private HermiteSpline startSpline;
    public HermiteSpline GetStartSpline() { return startSpline; }
    private HermiteSpline cyclicSpline;
    public HermiteSpline GetCyclicSpline() { return cyclicSpline; }

    public void SetSplineControllers(HermiteSpline startSpline, HermiteSpline cyclicSpline)
    {
        this.startSpline = startSpline;
        this.cyclicSpline = cyclicSpline;
    }

   // public HermiteSpline[] ForceSplines = new HermiteSpline[2];

    private float _timer = 0.0f;
    public float MaxiumumTime = 5.0f;
    public float MaximumForce = 10.0f;
    public float MaximumTorque = 10.0f;

    private bool hasInitialized = false;

    // Use this for initialization
    void Awake ()
	{
	    _hinge = gameObject.GetComponent<HingeJoint2D>();
	    var motor = _hinge.motor;
	    motor.maxMotorTorque = MaximumTorque;
        _hinge.motor = motor;
    }

    // Update is called once per frame
    void Update ()
	{
        _timer += Time.deltaTime;

        if (_timer > MaxiumumTime)
        {
            if (!hasInitialized)
                hasInitialized = true;
            _timer = 0.0f;
        }

        var scalar = _timer / MaxiumumTime;

        var motor = _hinge.motor;

	    if (!hasInitialized)
            motor.motorSpeed = startSpline.SampleYCoordinate(scalar) * MaximumForce;
	    else
            motor.motorSpeed = cyclicSpline.SampleYCoordinate(scalar) * MaximumForce;

        _hinge.motor = motor;
    }
}
