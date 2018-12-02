using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdController : MonoBehaviour {

    // Using this class to control the motion, selection and passing on of peasants on from the crowd to the peasant manager

    public ResourceManagementCore rmc;
    [Space]
    [Range(0, 200)]
    public int nPeasants = 100;
    public GameObject peasantPrefab;
    public List<Peasant> peasants;

    public float CrowdSizeScale;

    Transform crowdPos;

    [SerializeField]
    [Tooltip("Stat boost multiplier")]
    private float _statBoostMax = 5;

    [SerializeField]
    [Tooltip("Percentage chance of gaining a random affinity")]
    [Range(0.0f, 100.0f)]
    private float _affinityChancePercent = 10.0f;

    private void Awake()
    {
        crowdPos = gameObject.transform;
    }

    private void Start()
    {
        var spawnedPeasants = SpawnNPeasants(nPeasants);
        peasants = spawnedPeasants;
        rmc.CurrentGameState.ResourceState.UpdatePeastants(spawnedPeasants);
        Debug.Log("break point");
    }

    Peasant SpawnPeasant()
    {
        GameObject peasant = Instantiate(peasantPrefab, NoisyTransform(CrowdSizeScale),Quaternion.identity,transform);
        var p = peasant.GetComponent<Peasant>();

        int level = 0;
        var location = ResourceLocation.CrowdPit;
        float statBoost = GenerateRandomStatBoost(_statBoostMax);
        var affinity = AssignRandomAffinity(_affinityChancePercent);
        bool transit = false;

        p.Level = level;
        p.CurrentLocation = location;
        p.StatBoostBonus = statBoost;
        p.ResourceAffinity = affinity;
        p.IsInTrasit = transit;

        return p;
    }

    List<Peasant> SpawnNPeasants(int n)
    {
        var result = new List<Peasant>();

        for (int i = 0; i < n; i++)
        {
            var peasant = SpawnPeasant();
            result.Add(peasant);
        }

        return result;
    }

    float RandNorm(float scale)
    {
        float u1 = 1 - Random.value;
        float u2 = 1 - Random.value;

        float randomNum = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
        randomNum = scale * randomNum;
        return randomNum;
    }

    Vector3 NoisyTransform(float scale)
    {
        Vector3 randomNoise = new Vector2(RandNorm(scale) * 1.5f, RandNorm(scale));
        //Vector3 randomNoise = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1f, 1f)) * scale;
        Vector3 spawnPoint = crowdPos.position;
        spawnPoint += randomNoise;
        return spawnPoint;
    }

    private List<MaterialResourceType> AssignRandomAffinity(float affinityPercent)
    {
        var result = new List<MaterialResourceType>();

        if (RandomChance(affinityPercent))
        {
            result.Add(SelectRandomRersource());
        }

        return result;

    }

    private MaterialResourceType SelectRandomRersource()
    {
        int nResourceTypes = Enum.GetNames(typeof(MaterialResourceType)).Length;
        int randommIndex = (int)Random.Range(0.0f, nResourceTypes);
        return (MaterialResourceType)randommIndex;
    }

    private bool RandomChance(float chance)
    {
        var comparer = Random.Range(0.0f, 100.0f);

        return (comparer <= chance);
    }

    private float GenerateRandomStatBoost(float statBoostMax)
    {
        return Random.Range(0.0f, statBoostMax);
    }
}
