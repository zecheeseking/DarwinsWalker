using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Individual {

    public float[] _gene;
    public GameObject _organism;
    public float _fitness;

    abstract public void SetGene();
    abstract public float Fitness { get; set; }
    abstract public void UpdateAttributesFromGene();
    abstract public void Mutate();
}
