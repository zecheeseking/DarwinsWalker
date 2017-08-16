using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;
using Random = UnityEngine.Random;

public class GeneticAlgorithm : MonoBehaviour
{
    //[DllImport("user32.dll")]
    //private static extern void OpenFileDialog();
    //private static extern void SaveFileDialog();

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

    private bool autosaveToggle = false;
    public void SetAutosaveToggle() { autosaveToggle = !autosaveToggle; }
    private int autosaveGenerationInterval = 5;

    [SerializeField] private int PopulationSize = 10;
    private int currentGeneration = 0;

    private List<Genotype> Population = new List<Genotype>();
    private List<Genotype> results = new List<Genotype>();

    //UI Stuff
    public TextMeshProUGUI crossoverRateInputField;
    public TextMeshProUGUI mutationRateInputField;
    public TextMeshProUGUI autosaveGenerationInputField;
    public TextMeshProUGUI sessionNameInputField;
    public Slider timeScaleSliderField;
    public TextMeshProUGUI timeScaleLabel;
    public TextMeshProUGUI generationTimerLabel;

    #region UnityCallbacks
    // Use this for initialization
    void Awake ()
    {
        //Construct population. Involves generating initial genotypes and constructing Phenotypes from the genotypes.
        for (int i = 0; i < PopulationSize; i++)
            Population.Add(new Genotype());

        foreach(Genotype g in Population)
            Phenotype.CreatePhenotype(g);
	}


    private float internalTimer = 0.0f;
	// Update is called once per frame
	void Update ()
	{
	    internalTimer += Time.deltaTime;
	    generationTimerLabel.text = internalTimer.ToString("F");

        if (results.Count == Population.Count)
        {
            Evolve();
        }
	}
    #endregion

    #region UIHooks
    public void SetCrossoverRate(string s)
    {
        try
        {
            float xRate = Single.Parse(s);
            CrossoverRate = Mathf.Clamp01(xRate);
        }
        catch (FormatException ex)
        {
            crossoverRateInputField.text = CrossoverRate.ToString("F");
        }
    }

    public void SetMutationRate(string s)
    {
        try
        {
            float mutRate = Single.Parse(s);
            MutationRate = Mathf.Clamp01(mutRate);
        }
        catch (FormatException ex)
        {
            mutationRateInputField.text = MutationRate.ToString("F");
        }
    }

    public void SetAutosaveGenerationInterval(string s)
    {
        try
        {
            autosaveGenerationInterval = Int32.Parse(s);
        }
        catch (FormatException ex)
        {
            autosaveGenerationInputField.text = autosaveGenerationInterval.ToString();
        }
    }

    public void SetTimeScale(Single s)
    {
        Debug.Log("CALLED");
        Time.timeScale = s;
        timeScaleLabel.text = "Timescale: x" + Time.timeScale.ToString("F");
    }

    public void LoadCurrentGenotypes()
    {
        OpenFileDialog ofd = new OpenFileDialog();

        ofd.Filter = "All files (*.*)|*.*|JSON files(*.json)|*.json";
        ofd.InitialDirectory = Application.persistentDataPath;

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            Debug.Log(ofd.FileName);
            string json = File.ReadAllText(ofd.FileName);
            SaveObject loadedGenotypes = JsonUtility.FromJson<SaveObject>(json);
            foreach(var geno in loadedGenotypes.genotypes)
                Debug.Log(geno.RAnkle);

            Population.Clear();
            foreach (var geno in loadedGenotypes.genotypes)
            {
                Population.Add(new Genotype(geno));
            }
            currentGeneration = loadedGenotypes.generation;
            generationLabel.text = "Generation " + currentGeneration;

            //Reset generation.
            ResetGeneration();
        }
    }

    public void SaveCurrentGenotypes()
    {
        SaveFileDialog sfd = new SaveFileDialog();

        sfd.Filter = "All files (*.*)|*.*|JSON files(*.json)|*.json";
        sfd.InitialDirectory = Application.persistentDataPath;

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            string json = SavePopulation();
            File.WriteAllText(sfd.FileName, json);
        }
    }
    #endregion

    string SavePopulation()
    {
        SaveObject savedGenotypes = new SaveObject();
        savedGenotypes.generation = currentGeneration;
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

        return JsonUtility.ToJson(savedGenotypes);
    }

    void ResetGeneration()
    {
        //Destroy all current Phenotypes
        var livePhenotypes = GameObject.FindObjectsOfType<Phenotype>();
        foreach(var pheno in livePhenotypes)
            Destroy(pheno.gameObject); 
        //Clear any results
        results.Clear();
        //Recreate phenotypes from population
        foreach(var geno in Population)
            Phenotype.CreatePhenotype(geno);
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

            offspring1.FixEndPoints();
            offspring2.FixEndPoints();

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
        if (autosaveToggle && currentGeneration % autosaveGenerationInterval == 0)
        {
            string json = SavePopulation();
            FileInfo file = new FileInfo(Application.persistentDataPath + "/" + sessionNameInputField.text + "/"
                                         + "autosavePopulation" + currentGeneration / autosaveGenerationInterval + ".json");
            file.Directory.Create();
            File.WriteAllText(file.FullName, json);
        }

        results.Clear();
        internalTimer = 0.0f;
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

        if (Random.Range(0.0f, 1.0f) < crossoverRate)
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
[Serializable]
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

    public int generation = 0;
    public List<SavedGenotype> genotypes = new List<SavedGenotype>();
}