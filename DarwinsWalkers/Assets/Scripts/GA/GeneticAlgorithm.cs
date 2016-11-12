using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneticAlgorithm{

    public enum SelectionType
    {
        Roulette,
        BreedingPool
    }

    private List<Individual> _population;

    private float _mutationRate = 0.1f;
    private int _currentGeneration = 0;
    public int Generation { get { return _currentGeneration; } }
    private delegate Individual PopulationSelection(List<Individual> pop);
    private PopulationSelection _selection;

    public GeneticAlgorithm(List<Individual> _population, SelectionType selectionType)
    {

        _selection = AssignSelection(selectionType);
    }

    public void Evolve()
    {
        Debug.Log("To do: Evolve stuff");
    }

    private PopulationSelection AssignSelection(SelectionType selectionType)
    {
        if (selectionType == SelectionType.Roulette)
            return RouletteWheelSelection;
        else if (selectionType == SelectionType.BreedingPool)
            return BreedingPoolSelection;

        return null;
    }


    private Individual RouletteWheelSelection(List<Individual> pop)
    {
        return null;
    }

    private float TotalPopulationFitness(List<Individual> pop)
    {
        float f = 0.0f;

        foreach (Individual i in pop)
            f += i.Fitness;

        return f;
    }

    private Individual BreedingPoolSelection(List<Individual> pop)
    {
        return null;
    }
}
