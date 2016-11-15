using UnityEngine;
using System.Collections.Generic;

public class GATestScript : MonoBehaviour {

    GeneticAlgorithm _ga;

    private List<GameObject> _population = new List<GameObject>();
    public int PopulationSize = 10;
    public GameObject _target;

	// Use this for initialization
	void Awake () {

        for(int i = 0; i < PopulationSize; ++i)
        {
            var clone = Instantiate(_target);
            clone.transform.position = Vector3.up * 10.0f * i;
            clone.GetComponent<HermiteSpline>().GenerateRandomSpline(4);

            _population.Add(clone);
            //HermiteSplineIndividual indiv = new HermiteSplineIndividual("HSpline " + i);
            //indiv.SetParent(this.transform);
            //_population.Add(indiv);
        }

        //_ga = new GeneticAlgorithm(_population, GeneticAlgorithm.SelectionType.Roulette);
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            //_ga.Evolve();
        }
	}
}