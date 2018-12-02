//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class RandomLocSpawner : MonoBehaviour {

//    [Range(0, 100)]
//    public int nItems;
//    public GameObject[] itemPrefabs;

//    [Space]
//    public float spacing;
//    [Range(0.1f,10)]
//    public float widthToHeightRatio;
//    public bool isNormalDist;

//    List<Vector3> spawnLocations;
//    Transform crowdPos;

//    //void SpawnItem(int itemIndex)
//    //{
//    //    GameObject item = Instantiate(itemPrefabs[itemIndex], NoisyTransform(spacing, widthToHeightRatio), Quaternion.identity, transform);
//    //}

//    //void SpawnRandomItem()
//    //{
//    //    int randomIndex = Random.Range(0, itemPrefabs.Length);
//    //    GameObject item = Instantiate(itemPrefabs[randomIndex], NoisyTransform(spacing, widthToHeightRatio), Quaternion.identity, transform);
//    //}

//    void CreateSpawnLocation()
//    {
//        Vector3 newSpawnLocation = NoisyTransform(spacing, widthToHeightRatio);
//        spawnLocations.Add(newSpawnLocation);
//    }

//    void SpawnNRandomItems(int n)
//    {

//        for (int i = 0; i < n; i++)
//        {
//            SpawnRandomItem();
//        }
//    }

//    private void Awake()
//    {
//        crowdPos = gameObject.transform;
//    }

//    private void Start()
//    {
//        SpawnNRandomItems(nItems);
//    }

//    float RandNorm(float scale)
//    {
//        float u1 = 1 - Random.value;
//        float u2 = 1 - Random.value;

//        float randomNum = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
//        randomNum = scale * randomNum;
//        return randomNum;
//    }

//    Vector3 NoisyTransform(float scale, float wHRatio)
//    {
//        Vector3 randomNoise = new Vector3(1,1,1);

//        if (isNormalDist)
//            randomNoise = new Vector2(RandNorm(scale) * wHRatio, RandNorm(scale));
//        else
//            randomNoise = new Vector2(Random.Range(-wHRatio, wHRatio), Random.Range(-1f, 1f)) * scale;

//        Vector3 spawnPoint = crowdPos.position;
//        spawnPoint += randomNoise;
//        return spawnPoint;
//    }

//    void ReOrderSortOrder()
//    {
//        List<Transform> items = new List<Transform>();

//        //cards = cards.OrderBy(w => w.cardIndex).ToList();

//        foreach (Transform child in transform)
//        {
//            transform.GetComponentInChildren<Transform>();
//            items.Add(child);
//        }
//    }
//}
