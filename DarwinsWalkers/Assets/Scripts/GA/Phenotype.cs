using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Phenotype : MonoBehaviour
{
    private static GameObject bipedPrefab;

    private Transform HipBone;

    private float timer = 0.0f;

    private bool terminate = false;

    private Genotype genotype = null;
    private float maxX = 0.0f;

    private float internalTimer = 0.0f;
    private float recordProgressInterval = 0.5f;
    private List<float> recordedYHeights = new List<float>();

    void SetGenotype(Genotype genotype)
    {
        this.genotype = genotype;
        HipBone = transform;

        SetSplineControllerSplines(HipBone.FindChild("LThigh").GetComponent<HingeControl>(), Genotype.EGenotypeIndex.LHip);
        SetSplineControllerSplines(HipBone.FindChild("LShin").GetComponent<HingeControl>(), Genotype.EGenotypeIndex.LKnee);
        SetSplineControllerSplines(HipBone.FindChild("LFoot").GetComponent<HingeControl>(), Genotype.EGenotypeIndex.LAnkle);
        SetSplineControllerSplines(HipBone.FindChild("RThigh").GetComponent<HingeControl>(), Genotype.EGenotypeIndex.RHip);
        SetSplineControllerSplines(HipBone.FindChild("RShin").GetComponent<HingeControl>(), Genotype.EGenotypeIndex.RKnee);
        SetSplineControllerSplines(HipBone.FindChild("RFoot").GetComponent<HingeControl>(), Genotype.EGenotypeIndex.RAnkle);
    }

    void SetSplineControllerSplines(HingeControl hingeControl, Genotype.EGenotypeIndex index)
    {
        var splines = genotype.GetSplineController(index);
        float[] initializeSplineCps = splines.Take(splines.Length / 2).ToArray();
        float[] cyclicSplineCps = splines.Skip(splines.Length / 2).ToArray();

        hingeControl.SetSplineControllers(new HermiteSpline(initializeSplineCps), new HermiteSpline(cyclicSplineCps));
    }

    // Update is called once per frame
    void Update ()
	{
        if(HipBone.position.x > maxX)
            maxX = HipBone.position.x;

	    internalTimer += Time.deltaTime;

	    if (internalTimer > recordProgressInterval)
	    {
	        recordedYHeights.Add(HipBone.position.y);
	    }

        if (Terminate())
        {
            //Calculate final fitness
            genotype.Fitness = CalculateFinalFitness();
            //Register to GA
            GeneticAlgorithm.Instance.RegisterResults(genotype);
            //Cleanup object
            Destroy(gameObject);
        }
    }

    private float CalculateFinalFitness()
    {
        float sumY = 0.0f;
        foreach (float y in recordedYHeights)
            sumY += y;

        //By dividing the sum by the same number for all phenotypes, we ensure that those that terminated early do not get a great advantage 
        //due to having less recorded data than those who lasted the entire round. 
        sumY =  sumY / (GeneticAlgorithm.Instance.GenerationTimeLimit / recordProgressInterval);

         //This exists to greatly encourage phenotypes that maintain a higher Y average.
        return sumY * 1.7f + maxX;
    }

    bool Terminate()
    {
        timer += Time.deltaTime;

        if (timer > GeneticAlgorithm.Instance.GenerationTimeLimit)
            terminate = true;

        return terminate;
    }

    public void TerminateEarly()
    {
        terminate = true;
    }

    public static void CreatePhenotype(Genotype genotype)
    {
        if (bipedPrefab == null)
            bipedPrefab = Resources.Load<GameObject>("BipedPrefab");

        GameObject phenotype = Instantiate(bipedPrefab, Vector3.up * 4.5f, Quaternion.identity);
        phenotype.name = "Phenotype";
        phenotype.AddComponent<Phenotype>();
        phenotype.GetComponent<Phenotype>().SetGenotype(genotype);
    }
}
