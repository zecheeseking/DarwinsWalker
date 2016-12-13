using UnityEngine;
using System.Collections;

public class HingeControl : MonoBehaviour
{

    private HingeJoint2D _hinge;

    public HermiteSpline[] ForceSplines = new HermiteSpline[3];

    private float _timer = 0.0f;
    public float MaxiumumTime = 5.0f;

    // Use this for initialization
    void Awake ()
	{
	    _hinge = gameObject.GetComponent<HingeJoint2D>();

        //Fill with dummy data to test out viewing splines
	    for (int i = 0; i < 3; ++i)
	    {
	        ForceSplines[i] = gameObject.AddComponent<HermiteSpline>();
            ForceSplines[i].GenerateRandomSpline(3);
	    }

	    if (!_hinge) return;
	}
	
	// Update is called once per frame
	void Update ()
	{
        _timer += Time.deltaTime;

        if (_timer > MaxiumumTime)
        {
            _timer = 0.0f;
        }

        var scalar = _timer / MaxiumumTime;

        var motor = _hinge.motor;
        motor.motorSpeed = _hinge.motor.motorSpeed + ForceSplines[1].SampleYCoordinate(scalar);
        motor.maxMotorTorque = 5.0f;

        _hinge.motor = motor;
    }

    private void Visualize()
    {
        
    }
}
