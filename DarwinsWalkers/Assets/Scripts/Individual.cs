using UnityEngine;

public class Individual {

    private float _fitness;
    public GameObject HermiteSpline;

    public Individual(string name, GameObject splinePrefab)
    {
        HermiteSpline = GameObject.Instantiate(splinePrefab);
        HermiteSpline.name = name;
        HermiteSpline.GetComponent<HermiteSpline>().GenerateRandomSpline(4);
    }

    public float Fitness
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

    //public override void Mutate()
    //{
    //    throw new NotImplementedException();
    //}

    //public override void SetGene()
    //{
    //    throw new NotImplementedException();
    //}

    //public override void UpdateAttributesFromGene()
    //{
    //    throw new NotImplementedException();
    //}
}
