using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public float GenerationTimeLimit = 30.0f;

    [SerializeField]
    private float crossoverRate = 0.1f;
    public float CrossoverRate { get { return crossoverRate; } set { crossoverRate = value; } }
    [SerializeField]
    private float mutationRate = 0.03f;
    public float MutationRate { get { return mutationRate; } set { mutationRate = value; } }

    [SerializeField] private int PopulationSize = 10;
    private int currentGeneration = 0;

    private List<Genotype> Population = new List<Genotype>();
    private List<Genotype> results = new List<Genotype>();

    public TextMeshProUGUI crossoverRateInputField;
    public TextMeshProUGUI mutationRateInputField;

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
	    try
	    {
	        float xRate = Single.Parse(crossoverRateInputField.text);
	        CrossoverRate = Mathf.Clamp01(xRate);
	        crossoverRateInputField.text = CrossoverRate.ToString();
        }
        catch (FormatException ex)
	    {
	        crossoverRateInputField.text = CrossoverRate.ToString();
	    }

	    try
	    {
	        float mutRate = Single.Parse(mutationRateInputField.text);
	        MutationRate = Mathf.Clamp01(mutRate);
	        mutationRateInputField.text = MutationRate.ToString();
        }
        catch (FormatException ex)
	    {
	        mutationRateInputField.text = MutationRate.ToString();
	    }

        Debug.Log("Crossover rate: " + crossoverRate + ", Mutation rate: " + mutationRate);

        if (results.Count == Population.Count)
        {
            Evolve();
        }
	}

    public void LoadCurrentGenotypes()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/SavedPopulation.json");
        SaveObject loadedGenotypes = JsonUtility.FromJson<SaveObject>(json);
        Debug.Log("Loading...");
    }

    public void SaveCurrentGenotypes()
    {
        SaveObject savedGenotypes = new SaveObject();
        foreach (Genotype g in Population)
        {
            savedGenotypes.genotypes.Add(new SaveObject.SavedGenotype(
                g.genotype[(int)Genotype.EGenotypeIndex.LHip].ToList(),
                g.genotype[(int)Genotype.EGenotypeIndex.LKnee].ToList(),
                g.genotype[(int)Genotype.EGenotypeIndex.LAnkle].ToList(),
                g.genotype[(int)Genotype.EGenotypeIndex.RHip].ToList(),
                g.genotype[(int)Genotype.EGenotypeIndex.RKnee].ToList(),
                g.genotype[(int)Genotype.EGenotypeIndex.RAnkle].ToList()));
        }

        string json = JsonUtility.ToJson(savedGenotypes);
        File.WriteAllText(Application.persistentDataPath + "/SavedPopulation.json", json);
    }

    public void RegisterResults(Genotype genotype)
    {
        results.Add(genotype);
    }

    public void Evolve()
    {
        //Evolve pop
        var newPopulation = new List<Genotype>();


        //Breed using routlette wheel selection
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

        //Maintain fittest from previous population.
        newPopulation[0] = FittestGenotype();

        //Lower sixth of the population is randomly generated to maintain diversity.
        for (int i = newPopulation.Count - newPopulation.Count / 6; i < newPopulation.Count; i++)
        {
            newPopulation[i] = new Genotype();
        }

        Population = newPopulation;

        foreach (Genotype g in Population)
            Phenotype.CreatePhenotype(g);

        currentGeneration++;
        generationLabel.text = "Generation " + currentGeneration;
        results.Clear();
    }

    private Genotype FittestGenotype()
    {
        Genotype fittest = Population[0];

        for (int i = 1; i < Population.Count; i++)
        {
            if (Population[i].Fitness > fittest.Fitness)
                fittest = Population[i];
        }

        return fittest;
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

            offspring1.SetRawGenotype(genotypeA);
            offspring2.SetRawGenotype(genotypeA);
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

//Because Unity can't handle saving the data as just a list, I've created this to subclass the genotypes.
public class SaveObject
{
    [Serializable]
    public struct SavedGenotype
    {
        public List<float> LThigh;
        public List<float> LShin;
        public List<float> LAnkle;
        public List<float> RThigh;
        public List<float> RShin;
        public List<float> RAnkle;

        public SavedGenotype(List<float> LThigh, List<float> LShin, List<float> LAnkle,
            List<float> RThigh, List<float> RShin, List<float> RAnkle)
        {
            this.LThigh = LThigh;
            this.LShin = LShin;
            this.LAnkle = LAnkle;
            this.RThigh = RThigh;
            this.RShin = RShin;
            this.RAnkle = RAnkle;
        }
    }

    public List<SavedGenotype> genotypes = new List<SavedGenotype>();
}