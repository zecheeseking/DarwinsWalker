using UnityEngine;
using System.Collections;
using System;

public class HermiteSplineIndividual : Individual {

    public HermiteSplineIndividual(string name)
    {
        _organism = new GameObject();
        _organism.AddComponent<HermiteSpline>();
        _organism.GetComponent<HermiteSpline>().GenerateRandomSpline(4);
    }

    public void SetParent(Transform parent)
    {
        _organism.transform.parent = parent;
    }

    public void AddSplineClone(HermiteSpline spline)
    {
    }

    public override float Fitness
    {
        get
        {
            return _fitness;
        }

        set
        {
            _fitness = value;
        }
    }

    public override void Mutate()
    {
        throw new NotImplementedException();
    }

    public override void SetGene()
    {
        throw new NotImplementedException();
    }

    public override void UpdateAttributesFromGene()
    {
        throw new NotImplementedException();
    }
}
