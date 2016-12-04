using UnityEngine;
using System.Collections;
using Assets.Scripts.IK;

public class IKController : MonoBehaviour {

    public Transform[] Bones;
    private CyclicCoordinateDescent _ccdSolver;



    // Use this for initialization
    void Awake ()
	{
        _ccdSolver = new CyclicCoordinateDescent();
	}

    // Update is called once per frame
    void Update ()
    {
        Vector3 target = new Vector3();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            target = new Vector3(hit.point.x, hit.point.y, 0.0f);
        }

        Debug.Log(target.ToString());

        _ccdSolver.Solve(Bones, target);
    }

    private void OnDrawGizmos()
    {
    }
}