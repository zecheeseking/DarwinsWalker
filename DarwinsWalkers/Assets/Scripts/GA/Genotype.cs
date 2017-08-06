using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genotype
{
    public const float MIN_FLOAT = -5.0f;
    public const float MAX_FLOAT = 5.0f;

    public enum EGenotypeIndex
    {
        LHip = 0,
        RHip = 1,
        LKnee = 2,
        RKnee = 3,
        LAnkle = 4,
        RAnkle = 5
    }
    //An array of 3 floats for each spline
    //Set amount of joints always, LHip, RHip, LKnee, RKnee, LAnkle, RAnkle
    private List<float[]> genotype = new List<float[]>();
    public List<float[]> GetRawGenotype(){ return genotype; }
    public float Fitness { get; set; }

    public float[] GetSplineController(EGenotypeIndex index)
    {
        return genotype[(int)index];
    }

    public Genotype()
    {
        for (int i = 0; i <= (int) EGenotypeIndex.RAnkle; i++)
        {
            float a = Random.Range(MIN_FLOAT, MAX_FLOAT);
            float b = Random.Range(MIN_FLOAT, MAX_FLOAT);
            float c = Random.Range(MIN_FLOAT, MAX_FLOAT);

            genotype.Add(new [] {a,b,c});
        }
    }

    public void Mutate(float mutationRate)
    {
        if (Random.Range(0.0f, 100.0f) < mutationRate)
        {
            int iMutate = Random.Range(0, genotype.Count);

            if (iMutate == 0 || iMutate == genotype.Count - 1)
            {
                //Have to set both these to the same value to ensure that the splines will cycle properly.
            }
            else
            {
                //Just random it.
            }
        }
    }
}