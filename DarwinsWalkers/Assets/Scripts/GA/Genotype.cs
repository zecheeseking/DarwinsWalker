using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Genotype
{
    public const float MIN_XY_FLOAT = -1.0f;
    public const float MAX_XY_FLOAT = 1.0f;

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
    [SerializeField]
    public List<float[]> genotype = new List<float[]>();
    public List<float[]> GetRawGenotype(){ return genotype; }
    public void SetRawGenotype(List<float[]> newGenotype){ genotype = newGenotype; }

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

    public Genotype(SaveObject.SavedGenotype savedGenotype)
    {
        for (int i = 0; i <= (int) EGenotypeIndex.RAnkle; i++)
        {
            genotype.Add(null);
        }

        genotype[(int)EGenotypeIndex.LHip] = savedGenotype.LThigh.ToArray();
        genotype[(int)EGenotypeIndex.LKnee] = savedGenotype.LShin.ToArray();
        genotype[(int)EGenotypeIndex.LAnkle] = savedGenotype.LAnkle.ToArray();
        genotype[(int)EGenotypeIndex.RHip] = savedGenotype.RThigh.ToArray();
        genotype[(int)EGenotypeIndex.RKnee] = savedGenotype.RShin.ToArray();
        genotype[(int)EGenotypeIndex.RAnkle] = savedGenotype.RAnkle.ToArray();
    }

    public void Mutate(float mutationRate)
    {

        foreach (float[] splines in genotype)
        {
            for (int ii = 0; ii < splines.Length - 4; ii++)
            {
                float mutate = Random.Range(0.0f, 1.0f);
                if (mutate < mutationRate)
                {
                    float f = Random.Range(-1.0f, 1.0f);

                    //To ensure end control points are the same.
                    if (ii < 4)
                    {
                        splines[ii] = f;
                        splines[splines.Length - 4 + ii] = f;
                    }
                    else
                    {
                        splines[ii] = f;
                    }
                }
            }
        }
    }
}