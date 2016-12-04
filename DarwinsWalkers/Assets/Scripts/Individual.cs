using UnityEngine;
using System.Collections.Generic;

public class Individual
{

    private float _fitness;
    public GameObject HermiteSpline;
    private List<float[]> _genotype = new List<float[]>();

    public List<float[]> Genotype
    {
        get { return _genotype; }
        private set { }
    }

    public Individual()
    {

    }

    public Individual(string name, GameObject splinePrefab, bool randomize = true)
    {
        HermiteSpline = GameObject.Instantiate(splinePrefab);
        HermiteSpline.name = name;
        if (randomize)
            HermiteSpline.GetComponent<HermiteSpline>().GenerateRandomSpline(4);

        var spline = HermiteSpline.GetComponent<HermiteSpline>();

        int splineCPCount = spline.GetControlPointCount();
        for (int i = 0; i < splineCPCount; ++i)
        {
            var hcp = spline.GetSplinePointAt(i);

            float[] cp = { hcp.Position.x, hcp.Position.y, hcp.Tangent.x, hcp.Tangent.y };
            _genotype.Add(cp);
        }
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

    public void Mutate(float mutationRate)
    {
        if (Random.Range(0.0f, 100.0f) < mutationRate)
        {
            int iMutate = Random.Range(0, _genotype.Count);

            if (iMutate == 0)
            {
                _genotype[iMutate][0] = 0.0f;
                _genotype[iMutate][1] = UnityEngine.Random.Range(-5.0f, 5.0f);
                _genotype[iMutate][2] = _genotype[iMutate][0];
                _genotype[iMutate][3] = _genotype[iMutate][1];
            }
            else
            {
                _genotype[iMutate][0] = _genotype[iMutate - 1][0] + UnityEngine.Random.Range(2.5f, 8.0f);
                _genotype[iMutate][1] = UnityEngine.Random.Range(-5.0f, 5.0f);
                _genotype[iMutate][2] = _genotype[iMutate][0];
                _genotype[iMutate][3] = _genotype[iMutate][1];
            }
        }
    }

    public void UpdateAttributesFromGene()
    {
        //HermiteSpline hSpline = this.HermiteSpline.GetComponent<HermiteSpline>();
        //hSpline.splineControlPoints.Clear();

        //for(int x = 0; x < _genotype.Count; ++x)
        //{
        //    HermiteSplineControlPoint hcp;
        //    hcp.position = new Vector3(_genotype[x][0], _genotype[x][1], 0);
        //    hcp.tangent = new Vector3(_genotype[x][2], _genotype[x][3], 0);
        //    hSpline.splineControlPoints.Add(hcp);
        //}
    }
}
