﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour
{
	void Update ()
	{
        Vector2 avgPos = Vector2.zero;

	    var phenotypes = FindObjectsOfType<Phenotype>();
	    if (phenotypes.Length > 0)
	    {
            foreach (Phenotype p in phenotypes)
                avgPos += new Vector2(p.transform.position.x, p.transform.position.y);

            avgPos.x /= phenotypes.Length;
            avgPos.y /= phenotypes.Length;
	    }

        Camera.main.transform.position = new Vector3(avgPos.x, 3.04f, -10);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hit.rigidbody.gameObject.GetComponent<JointSelection>().RaycastHit();
            }
        }
    }
}
