using UnityEngine;

public class EarlyTerminate : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Hip")
        {
            col.gameObject.GetComponent<Phenotype>().TerminateEarly();
        }
    }
}
