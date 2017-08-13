using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Genotype
{
    public const float MIN_XY_FLOAT = -5.0f;
    public const float MAX_XY_FLOAT = 5.0f;

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
    //Each Control point is 4 numbers. Use the Enum to calculate the offset, each spline fixed to 4 control points, 
    //will have 5 due to start and end being the same.
    private List<float[]> genotype = new List<float[]>();
    public List<float[]> GetRawGenotype(){ return genotype; }
    public float Fitness { get; set; }

    public float[] GetSplineController(EGenotypeIndex index)
    {
        return genotype[(int)index];
    }

    public Genotype()
    {
        var types = Enum.GetValues(typeof(EGenotypeIndex));

        foreach (EGenotypeIndex type in types)
        {
            int startIndex = genotype.Count;
            for (int i = 0; i <= 4 * 4; i++)
            {
                float[] splineCps = new float[4 * 5];

                for (int ii = 0; ii < splineCps.Length - 4; ii++)
                    splineCps[ii] = Random.Range(MIN_XY_FLOAT, MAX_XY_FLOAT);

                splineCps[splineCps.Length - 4] = splineCps[0];
                splineCps[splineCps.Length - 3] = splineCps[1];
                splineCps[splineCps.Length - 2] = splineCps[2];
                splineCps[splineCps.Length - 1] = splineCps[3];

                genotype.Add(splineCps);
            }
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