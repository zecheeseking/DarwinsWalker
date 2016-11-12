using UnityEngine;
using System.Collections;

public abstract class Individual {

    public string _gene;
    public GameObject _organism;
    public int _fitness;

    abstract public void SetGene();
    abstract public float Fitness { get; set; }
    abstract public void UpdateAttributesFromGene();
    abstract public void Mutate();
}
