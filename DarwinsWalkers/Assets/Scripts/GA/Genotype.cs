using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Genotype
{
    public const float MIN_XY_FLOAT = -1.0f;
    public const float MAX_XY_FLOAT = 1.0f;

    private const float splineLength = 5.0f;

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
                float[] initializeSplineCps = new float[4 * 5];
                float[] splineCps = new float[4 * 5];

                for (int ii = 0; ii < splineCps.Length - 4; ii+=4)
                {
                    Vector2 positionInitSpline = new Vector2(splineLength / 4.0f * (ii / 4.0f), Random.Range(MIN_XY_FLOAT, MAX_XY_FLOAT));
                    Vector2 posCyclicSpline = new Vector2(positionInitSpline.x, Random.Range(MIN_XY_FLOAT, MAX_XY_FLOAT));
                    //Position
                    initializeSplineCps[ii] = splineCps[ii] = posCyclicSpline.x;
                    initializeSplineCps[ii + 1] = positionInitSpline.y;
                    splineCps[ii + 1] = posCyclicSpline.y;

                    //Limit tangent X to positive so that splines travel in same direction always.
                    Vector2 InitTan = positionInitSpline + new Vector2(Random.Range(0.1f, MAX_XY_FLOAT / 2), Random.Range(MIN_XY_FLOAT / 2, MAX_XY_FLOAT / 2));
                    Vector2 CyclicTan = positionInitSpline + new Vector2(Random.Range(0.1f, MAX_XY_FLOAT / 2), Random.Range(MIN_XY_FLOAT / 2, MAX_XY_FLOAT / 2));

                    initializeSplineCps[ii + 2] = InitTan.x;
                    splineCps[ii + 2] = CyclicTan.x;
                    initializeSplineCps[ii + 3] = InitTan.y;
                    splineCps[ii + 3] = CyclicTan.y;
                }

                initializeSplineCps[initializeSplineCps.Length - 4] = splineCps[splineCps.Length - 4] = splineLength;
                initializeSplineCps[initializeSplineCps.Length - 3] = splineCps[splineCps.Length - 3] = splineCps[1];
                initializeSplineCps[initializeSplineCps.Length - 2] = splineCps[splineCps.Length - 2] = splineCps[2];
                initializeSplineCps[initializeSplineCps.Length - 1] = splineCps[splineCps.Length - 1] = splineCps[3];

                List<float> completeGenotype = new List<float>(initializeSplineCps);
                foreach(float f in splineCps)
                    completeGenotype.Add(f);

                genotype.Add(completeGenotype.ToArray());
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
            for (int ii = 0; ii < splines.Length; ii++)
            {
                float mutate = Random.Range(0.0f, 1.0f);
                if (mutate < mutationRate)
                {
                    float f = Random.Range(-1.0f, 1.0f);
                    splines[ii] = f;
                }
            }

            //Ensure end point of first spline and beginning + end of cyclic splines are all the same.
            splines[splines.Length / 2 - 1] = splines[splines.Length / 2 + 3] = splines[splines.Length - 1];
            splines[splines.Length / 2 - 2] = splines[splines.Length / 2 + 2] = splines[splines.Length - 2];
            splines[splines.Length / 2 - 3] = splines[splines.Length / 2 + 1] = splines[splines.Length - 3];
            splines[splines.Length / 2 - 4] = splines[splines.Length / 2] = splines[splines.Length - 4];
        }
    }
}