using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarlyTerminate : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Hip")
        {
            col.gameObject.GetComponent<Phenotype>().TerminateEarly();
            Debug.Log("Terminate " + col.gameObject.name);
        }
    }
}
