//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class GeneticAlgorithm : MonoBehaviour
//{

//    public enum SelectionType
//    {
//        Roulette,
//        BreedingPool
//    }

//    private List<Individual> _population;

//    private float _crossoverRate = 0.1f;
//    public float CrossoverRate { get { return _crossoverRate; } set { _crossoverRate = value; } }

//    private float _mutationRate = 0.1f;
//    public float MutationRate { get { return _mutationRate; } set { _mutationRate = value; } }

//    private int _currentGeneration = 1;
//    public int Generation { get { return _currentGeneration; } }
//    private delegate Individual PopulationSelection(List<Individual> pop);
//    private PopulationSelection _selection;

//    private Individual _target;

//    public GeneticAlgorithm(List<Individual> population, SelectionType selectionType, Individual target)
//    {
//        _population = population;
//        _selection = AssignSelection(selectionType);
//        _target = target;
//    }

//    public void Evolve()
//    {
//        //Evaluate existing pop
//        EvaluatePopulation();

//        //Evolve pop
//        var newPopulation = new List<Individual>();

//        for(int i = 0; i < _population.Count / 2; ++i)
//        {
//            var parent1 = _selection(_population);
//            var parent2 = _selection(_population);
//            //Crossover
//            Individual offspring1 = parent1, offspring2 = parent2;
//            OnePointCrossover(parent1, parent2, ref offspring1, ref offspring2);
//            //Mutate offspring
//            offspring1.Mutate(_mutationRate);
//            offspring2.Mutate(_mutationRate);

//            offspring1.UpdateAttributesFromGene();
//            offspring2.UpdateAttributesFromGene();

//            newPopulation.Add(offspring1);
//            newPopulation.Add(offspring2);
//        }

//        _currentGeneration++;
//    }

//    private PopulationSelection AssignSelection(SelectionType selectionType)
//    {
//        if (selectionType == SelectionType.Roulette)
//            return RouletteWheelSelection;
//        else if (selectionType == SelectionType.BreedingPool)
//            return BreedingPoolSelection;

//        return null;
//    }

//    public void EvaluatePopulation()
//    {
//        var targetGenotype = _target.Genotype;

//        for(int i = 0; i < _population.Count; ++i)
//        {
//            float f = 0.0f;
//            var genotype = _population[i].Genotype;
//            //Sum difference between genotype numbers
//            for(int x = 0; x < genotype.Count; ++x)
//            {
//                for(int y = 0; y < genotype[x].Length; ++y)
//                {
//                    f += (targetGenotype[x][y] - genotype[x][y]);
//                }
//            }

//            _population[i].Fitness = _target.Fitness - f;
//        }
//    }

//    private Individual RouletteWheelSelection(List<Individual> pop)
//    {
//        float totalFitness = Mathf.Abs(TotalPopulationFitness(pop));

//        float random = Random.Range(0, totalFitness);
//        float s = 0.0f;

//        foreach(Individual ind in  pop)
//        {
//            s += Mathf.Abs(ind.Fitness);

//            if (s > random)
//            {
//                return ind;
//            }
//        }

//        return null;
//    }

//    private void OnePointCrossover(Individual parent1, Individual parent2, ref Individual offspring1, ref Individual offspring2)
//    {
//        var genotypeParent1 = parent1.Genotype;
//        var genotypeParent2 = parent2.Genotype;
//        int crossoverPoint = genotypeParent1.Count;

//        if(Random.Range(0.0f, 100.0f) < _mutationRate)
//        {
//            crossoverPoint = Random.Range(0, genotypeParent1.Count);

//            List<float[]> genotypeA = new List<float[]>();
//            List<float[]> genotypeB = new List<float[]>();

//            for (int i = 0; i < genotypeParent1.Count; ++i)
//            {
//                if(i < crossoverPoint)
//                {
//                    genotypeA.Add(genotypeParent1[i]);
//                    genotypeB.Add(genotypeParent2[i]);
//                }
//                else
//                {
//                    genotypeA.Add(genotypeParent2[i]);
//                    genotypeB.Add(genotypeParent1[i]);
//                }
//            }
//        }
//    }

//    private Individual BreedingPoolSelection(List<Individual> pop)
//    {
//        return null;
//    }

//    private float TotalPopulationFitness(List<Individual> pop)
//    {
//        float f = 0.0f;

//        foreach (Individual i in pop)
//            f += i.Fitness;

//        return f;
//    }
//}
