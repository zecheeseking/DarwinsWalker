using UnityEngine;
using System.Collections;

public class HingeControl : MonoBehaviour
{

    private HingeJoint2D _hinge;

    public HermiteSpline TargetForceSpline;

    private float _timer = 0.0f;
    public float MaxiumumTime = 1.0f;

    // Use this for initialization
    void Awake ()
	{
	    _hinge = gameObject.GetComponent<HingeJoint2D>();

	    if (!_hinge) return;
	}
	
	// Update is called once per frame
	void Update ()
	{

	    _timer += Time.deltaTime;

	    if (_timer > MaxiumumTime)
	    {
	        Debug.Log("RESET");
	        _timer = 0.0f;
	    }

	    var scalar = _timer/MaxiumumTime;



	    //if (Input.GetKey(KeyCode.W))
	    //{
	    //    var motor = _hinge.motor;
	    //    motor.motorSpeed = _hinge.motor.motorSpeed + 5.0f;
	    //    motor.maxMotorTorque = 100.0f;

	    //    _hinge.motor = motor;
	    //}
	    //   else if (Input.GetKey(KeyCode.S))
	    //{
	    //       var motor = _hinge.motor;
	    //       motor.motorSpeed = _hinge.motor.motorSpeed + -5.0f;
	    //       motor.maxMotorTorque = 100.0f;

	    //       _hinge.motor = motor;
	    //   }
	}
}
