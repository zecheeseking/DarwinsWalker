using UnityEngine;
using System.Collections;

public class HingeControl : MonoBehaviour
{

    private HingeJoint2D _hinge;

    public HermiteSpline[] ForceSplines = new HermiteSpline[2];

    private float _timer = 0.0f;
    public float MaxiumumTime = 5.0f;

    private bool reversed = false;

    // Use this for initialization
    void Awake ()
	{
	    _hinge = gameObject.GetComponent<HingeJoint2D>();

        //Fill with dummy data to test out viewing splines
	    //for (int i = 0; i < 3; ++i)
	    //{
	    //    ForceSplines[i] = gameObject.AddComponent<HermiteSpline>();
        //    ForceSplines[i].GenerateRandomSpline(2);
	    //}
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
            reversed = !reversed;
            _timer = MaxiumumTime;
        }
        else if(_timer < 0)
        {
            reversed = !reversed;
            _timer = 0.0f;
        }

        var scalar = _timer / MaxiumumTime;
        //Debug.Log(scalar);

        var motor = _hinge.motor;
        motor.motorSpeed = ForceSplines[1].SampleYCoordinate(scalar);
        motor.maxMotorTorque = 15.0f;

        _hinge.motor = motor;
    }

    private void Visualize()
    {
        
    }
}
