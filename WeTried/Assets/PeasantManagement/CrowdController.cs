using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour {

    // Using this class to control the motion, selection and passing on of peasants on from the crowd to the peasant manager

    [Range(0, 100)]
    public int nPeasants;
    public GameObject peasantPrefab;
    public List<GameObject> peasants;

    public float CrowdSizeScale;

    Transform crowdPos;

    void SpawnPeasant()
    {
        GameObject peasant = Instantiate(peasantPrefab, NoisyTransform(CrowdSizeScale),Quaternion.identity,transform);

        peasants.Add(peasant);
    }

    void SpawnNPeasants(int n)
    {
        for (int i = 0; i < n; i++)
        {
            SpawnPeasant();
        }
    }

    private void Awake()
    {
        crowdPos = gameObject.transform;
    }

    private void Start()
    {
        SpawnNPeasants(nPeasants);
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
}
