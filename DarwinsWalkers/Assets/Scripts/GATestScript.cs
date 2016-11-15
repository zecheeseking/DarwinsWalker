using UnityEngine;
using System.Collections.Generic;

public class GATestScript : MonoBehaviour {

    GeneticAlgorithm _ga;

    private List<Individual> _population = new List<Individual>();
    public int PopulationSize = 10;
    public GameObject _target;

	// Use this for initialization
	void Awake () {

        for(int i = 0; i < PopulationSize; ++i)
        {
            Individual indiv = new Individual("HSpline " + i, _target);

            indiv.HermiteSpline.transform.position = Vector3.up * 10.0f * i;

            _population.Add(indiv);
        }

        foreach (Individual i in _population)
        {
            i.HermiteSpline.GetComponent<HermiteSpline>().RefreshSplinePoints();
        }

        _ga = new GeneticAlgorithm(_population, GeneticAlgorithm.SelectionType.Roulette);
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            _ga.Evolve();
        }
	}
}