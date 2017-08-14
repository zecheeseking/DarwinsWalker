using UnityEngine;
using System.Collections;

public class HingeControl : MonoBehaviour
{

    private HingeJoint2D _hinge;

    public HermiteSpline[] ForceSplines = new HermiteSpline[2];

    private float _timer = 0.0f;
    public float MaxiumumTime = 5.0f;
    public float MaximumForce = 10.0f;
    public float MaximumTorque = 10.0f;

    private bool reversed = false;

    // Use this for initialization
    void Awake ()
	{
	    _hinge = gameObject.GetComponent<HingeJoint2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (ForceSplines[0] == null)
	        return;

        if(!reversed)
            _timer += Time.deltaTime;
        else
            _timer -= Time.deltaTime;

        if (_timer > MaxiumumTime)
        {
            reversed = !reversed;
            _timer = MaxiumumTime;
        }
        else if(_timer < 0)
        {
            reversed = !reversed;
            _timer = 0.0f;
        }

        var scalar = _timer / MaxiumumTime;

        var motor = _hinge.motor;
        motor.motorSpeed = ForceSplines[0].SampleYCoordinate(scalar) * MaximumForce;
        motor.maxMotorTorque = MaximumTorque;

        _hinge.motor = motor;
    }
}
