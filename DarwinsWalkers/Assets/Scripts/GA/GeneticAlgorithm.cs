using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public enum SelectionType
    {
        Roulette,
        BreedingPool
    }

    [SerializeField] private TextMeshProUGUI generationLabel;

    private static GeneticAlgorithm instance = null;
    public static GeneticAlgorithm Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GeneticAlgorithm>();

            return instance;
        }
    }

    [SerializeField]
    private float crossoverRate = 0.1f;
    [SerializeField]
    private float mutationRate = 0.1f;

    [SerializeField] private int PopulationSize = 10;
    private int currentGeneration = 0;

    private List<Genotype> Population = new List<Genotype>();
    private List<Genotype> results = new List<Genotype>();

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
        if (results.Count == Population.Count)
        {
            Evolve();
        }
	}

    public void RegisterResults(Genotype genotype)
    {
        results.Add(genotype);
    }

    public void Evolve()
    {
        //Evolve pop
        var newPopulation = new List<Genotype>();

        for (int i = 0; i < Population.Count / 2; ++i)
        {
            var parent1 = RouletteWheelSelection(Population);
            var parent2 = RouletteWheelSelection(Population);
            //Crossover
            Genotype offspring1 = parent1, offspring2 = parent2;
            OnePointCrossover(parent1, parent2, ref offspring1, ref offspring2);
            //Mutate offspring
            offspring1.Mutate(mutationRate);
            offspring2.Mutate(mutationRate);

            newPopulation.Add(offspring1);
            newPopulation.Add(offspring2);
        }

        Population = newPopulation;

        foreach (Genotype g in Population)
            Phenotype.CreatePhenotype(g);

        currentGeneration++;
        generationLabel.text = "Generation " + currentGeneration;
        results.Clear();
    }

    private void OnePointCrossover(Genotype parent1, Genotype parent2, ref Genotype offspring1, ref Genotype offspring2)
    {
        var genotypeParent1 = parent1.GetRawGenotype();
        var genotypeParent2 = parent2.GetRawGenotype();
        int crossoverPoint = genotypeParent1.Count;

        if (Random.Range(0.0f, 100.0f) < mutationRate)
        {
            crossoverPoint = Random.Range(0, genotypeParent1.Count);

            List<float[]> genotypeA = new List<float[]>();
            List<float[]> genotypeB = new List<float[]>();

            for (int i = 0; i < genotypeParent1.Count; ++i)
            {
                if (i < crossoverPoint)
                {
                    genotypeA.Add(genotypeParent1[i]);
                    genotypeB.Add(genotypeParent2[i]);
                }
                else
                {
                    genotypeA.Add(genotypeParent2[i]);
                    genotypeB.Add(genotypeParent1[i]);
                }
            }
        }
    }

    private Genotype RouletteWheelSelection(List<Genotype> pop)
    {
        float totalFitness = Mathf.Abs(TotalPopulationFitness(pop));

        float random = Random.Range(0, totalFitness);
        float s = 0.0f;

        foreach (Genotype ind in pop)
        {
            s += Mathf.Abs(ind.Fitness);

            if (s > random)
            {
                return ind;
            }
        }

        return null;
    }

    private float TotalPopulationFitness(List<Genotype> pop)
    {
        float f = 0.0f;

        foreach (Genotype i in pop)
            f += i.Fitness;

        return f;
    }
}
