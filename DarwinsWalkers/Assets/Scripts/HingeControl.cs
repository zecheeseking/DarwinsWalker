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

    private bool reversed = false;
    private bool hasInitialized = false;

    // Use this for initialization
    void Awake ()
	{
	    _hinge = gameObject.GetComponent<HingeJoint2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(!reversed)
            _timer += Time.deltaTime;
        else
            _timer -= Time.deltaTime;

        if (_timer > MaxiumumTime)
        {
            if (!hasInitialized)
                hasInitialized = true;
            else
                reversed = !reversed;
            _timer = 0.0f;
        }
        else if(_timer < 0)
        {
            reversed = !reversed;
            _timer = 0.0f;
        }

        var scalar = _timer / MaxiumumTime;

        var motor = _hinge.motor;

	    if (!hasInitialized)
            motor.motorSpeed = startSpline.SampleYCoordinate(scalar) * MaximumForce;
	    else
            motor.motorSpeed = cyclicSpline.SampleYCoordinate(scalar) * MaximumForce;
	    motor.maxMotorTorque = MaximumTorque;

        _hinge.motor = motor;
    }
}
