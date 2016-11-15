using UnityEngine;
using System.Collections.Generic;

public class GATestScript : MonoBehaviour {

    GeneticAlgorithm _ga;

    private List<Individual> _population = new List<Individual>();
    public int PopulationSize = 10;

	// Use this for initialization
	void Awake () {

        for(int i = 0; i < PopulationSize; ++i)
        {
            HermiteSplineIndividual indiv = new HermiteSplineIndividual("HSpline " + i);
            indiv.SetParent(this.transform);
            _population.Add(indiv);
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