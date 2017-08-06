//using UnityEngine;
//using System.Collections.Generic;

//public class GATestScript : MonoBehaviour {

//    GeneticAlgorithm _ga;

//    public int PopulationSize = 10;
//    public GameObject _target;

//	// Use this for initialization
//	void Awake () {

//        List<Individual> _population = new List<Individual>();

//        for(int i = 0; i < PopulationSize; ++i)
//        {
//            Individual indiv = new Individual("HSpline " + i, _target);

//            indiv.HermiteSpline.transform.position = Vector3.up * 10.0f * i;

//            _population.Add(indiv);
//        }

//        foreach (Individual i in _population)
//        {
//            i.HermiteSpline.GetComponent<HermiteSpline>().RefreshSplinePoints();
//        }

//        Individual target = new Individual("Target", _target, false);
//        target.HermiteSpline.transform.position = Vector3.left * 25.0f;

//        _ga = new GeneticAlgorithm(_population, GeneticAlgorithm.SelectionType.Roulette, target);
//        _ga.CrossoverRate = 70.0f;
//        _ga.MutationRate = 30.0f;
//        Debug.Log("Generation: " + _ga.Generation);
//    }

//    // Update is called once per frame
//    void Update () {
//	    if(Input.GetKeyDown(KeyCode.Space))
//        {
//            _ga.Evolve();
//            Debug.Log("Generation: " + _ga.Generation);
//        }
//	}
//}