using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Phenotype : MonoBehaviour
{
    private static GameObject bipedPrefab;

    private Transform HipBone;

    private float timer = 0.0f;
    private static float maxTime = 5.0f;

    private bool terminate = false;

    private Genotype genotype = null;
    private float fitness = 0.0f;

    void SetGenotype(Genotype genotype)
    {
        this.genotype = genotype;
        HipBone = transform;

        HipBone.FindChild("LThigh").GetComponent<HingeControl>().ForceSplines = 
            new []{ new HermiteSpline(genotype.GetSplineController(Genotype.EGenotypeIndex.LHip))};
        HipBone.FindChild("LShin").GetComponent<HingeControl>().ForceSplines =
            new []{ new HermiteSpline(genotype.GetSplineController(Genotype.EGenotypeIndex.LKnee))};
        HipBone.FindChild("LFoot").GetComponent<HingeControl>().ForceSplines =
            new []{ new HermiteSpline(genotype.GetSplineController(Genotype.EGenotypeIndex.LAnkle))};
        HipBone.FindChild("RThigh").GetComponent<HingeControl>().ForceSplines =
            new []{ new HermiteSpline(genotype.GetSplineController(Genotype.EGenotypeIndex.RHip))};
        HipBone.FindChild("RShin").GetComponent<HingeControl>().ForceSplines =
            new []{ new HermiteSpline(genotype.GetSplineController(Genotype.EGenotypeIndex.RKnee))};
        HipBone.FindChild("RFoot").GetComponent<HingeControl>().ForceSplines =
            new []{ new HermiteSpline(genotype.GetSplineController(Genotype.EGenotypeIndex.RAnkle))};
    }

    // Update is called once per frame
    void Update ()
	{
	    fitness = HipBone.position.x;

        if (Terminate())
        {
            //Record fitness
            genotype.Fitness = fitness;
            //Register to GA
            GeneticAlgorithm.Instance.RegisterResults(genotype);
            //Cleanup object
            Destroy(gameObject);
        }
    }

    bool Terminate()
    {
        timer += Time.deltaTime;

        if (timer > maxTime)
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

        GameObject phenotype = Instantiate(bipedPrefab, Vector3.up * 5.0f, Quaternion.identity);
        phenotype.name = "Phenotype";
        phenotype.AddComponent<Phenotype>();
        phenotype.GetComponent<Phenotype>().SetGenotype(genotype);
    }
}
