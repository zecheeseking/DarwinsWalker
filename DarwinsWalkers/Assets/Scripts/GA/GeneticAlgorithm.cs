using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    [SerializeField] private int PopulationSize = 10;

    private List<Genotype> Population = new List<Genotype>();

	// Use this for initialization
	void Awake ()
    {
        //Construct population. Involves generating initial genotypes and constructing Phenotypes from the genotypes.
        for (int i = 0; i < PopulationSize; i++)
            Population.Add(new Genotype());

        foreach(Genotype g in Population)
            Phenotype.CreatePhenotype(g);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
