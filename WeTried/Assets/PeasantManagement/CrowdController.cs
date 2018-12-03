using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CrowdController : MonoBehaviour {

    // Using this class to control the motion, selection and passing on of peasants on from the crowd to the peasant manager

    public ResourceManagementCore ResourceCore;
    public ResourceManagementUIManager UIManager;
    [Space]
    [Range(0, 200)]
    public int nPeasants = 100;
    public GameObject peasantPrefab;
    public List<Peasant> peasants;

    public float CrowdSizeScale;
    [Space]
    [Header("UI for number of peasants")]
    public Slider CrowdSlider;
    public GameObject NPeasantPanel;
    public Text CurrentValueLabel;
    public Text MaxValueLabel;

    [Tooltip("Centrepoint of where crowd is")]
    public Transform CrowdPos;

    [SerializeField]
    [Tooltip("Stat boost multiplier")]
    private float _statBoostMax = 5;

    [SerializeField]
    [Tooltip("Percentage chance of gaining a random affinity")]
    [Range(0.0f, 100.0f)]
    private float _affinityChancePercent = 10.0f;

    [SerializeField]
    private float _moveSpeed = 20;

    [SerializeField]
    private float _reachedLocationRadius = 5;

    [Header("Spawn Points to wire up")]
    public Transform CrowdPit;
    public Transform Mine;
    public Transform Farm;
    public Transform BlackSmith;
    public Transform Forest;
    public Transform Lake;
    public Transform SACRIFICE;

    [Header("Clickable Bulidings")]
    public ClickableBuildingController[] ClickableBuildings;

    private int _nPeasantsToMove = 1;

    private void Awake()
    {
        CrowdPos = gameObject.transform;
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();

    }

    private void Start()
    {
        if (PersistingData.storyProgression == 0)
        {
            var spawnedPeasants = SpawnNPeasants(nPeasants);
            peasants = spawnedPeasants;
            ResourceCore.CurrentGameState.ResourceState.UpdatePeastants(spawnedPeasants);
        }
        else
        {
            nPeasants = PersistingData.gs.GetNumberOfPeasants();
            var spawnedPeasants = SpawnNPeasants(nPeasants);
            peasants = spawnedPeasants;
            ResourceCore.CurrentGameState.ResourceState.UpdatePeastants(spawnedPeasants);
        }

        // TODO: Codesmell. This class shouldn't be worried about UI
        CrowdSlider.onValueChanged.AddListener(delegate { NumberOfPeasantsChanged(); });

        UpdatePeasantNumberPanel();
    }

    #region Implementation

    public Peasant SpawnPeasant(ResourceLocation location)
    {
        Vector3 spawnPoint = GetPosition(location);
        GameObject peasant = Instantiate(peasantPrefab, NoisyTransformPosition(spawnPoint, CrowdSizeScale), Quaternion.identity,transform);
        var p = peasant.GetComponent<Peasant>();

        int level = 0;
        float statBoost = GenerateRandomStatBoost(_statBoostMax);
        var affinity = AssignRandomAffinity(_affinityChancePercent);
        bool transit = false;

        p.Level = level;
        p.CurrentLocation = location;
        p.StatBoostBonus = statBoost;
        p.ResourceAffinity = affinity;
        p.IsInTrasit = transit;
        p.MoveSpeed = _moveSpeed;
        p.ReachedLocationRadius = _reachedLocationRadius;
        p.Controller = this;

        return p;
    }

    public List<Peasant> SpawnNPeasants(int n)
    {
        var result = new List<Peasant>();

        for (int i = 0; i < n; i++)
        {
            var peasant = SpawnPeasant(ResourceLocation.CrowdPit);
            result.Add(peasant);
        }

        return result;
    }

    public float RandNorm(float scale)
    {
        float u1 = 1 - Random.value;
        float u2 = 1 - Random.value;

        float randomNum = Mathf.Sqrt(-2 * Mathf.Log(u1)) * Mathf.Sin(2 * Mathf.PI * u2);
        randomNum = scale * randomNum;
        return randomNum;
    }

    public Vector3 NoisyTransformPosition(Vector3 centrePoint, float scale)
    {
        Vector3 positionalNoise = new Vector2(RandNorm(scale), RandNorm(scale));

        //Vector3 randomNoise = new Vector2(RandNorm(scale) * 1.5f, RandNorm(scale));
        //Vector3 randomNoise = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1f, 1f)) * scale;
        Vector3 newPosition = centrePoint;
        newPosition += positionalNoise;
        return newPosition;
    }
    
    public void MovePeasants(ResourceLocation newDestination)
    {
        // get peasants from origin
        int nPeasantsToMove = (int)CrowdSlider.value;
        var peasants = ResourceCore.GetPeasantsAt(ResourceLocation.CrowdPit);

        var peasantsToMove = peasants.Where(p => p.IsInTrasit == false).Take(nPeasantsToMove);

        // move them to new destination
        foreach (var peasant in peasantsToMove)
        {
            var destination = NoisyTransformPosition(GetPosition(newDestination), CrowdSizeScale);
            peasant.StartMoving(newDestination, destination);
        }

        // Update UI
        UpdatePeasantNumberPanel();
        UIManager.UpdateUI();
    }

    public void UpdatePeasantNumberPanel()
    {
        // get nubmer of peasants at origin
        int nPeasants = ResourceCore.GetPeasantsAt(ResourceLocation.CrowdPit).Count();

        // update UI
        CrowdSlider.maxValue = nPeasants;
        CrowdSlider.value = 1;
        CrowdSlider.minValue = 1;

        MaxValueLabel.text = nPeasants.ToString();
        CurrentValueLabel.text = 1.ToString();
    }

    public void NumberOfPeasantsChanged()
    {
        // get current value
        int newValue = (int)CrowdSlider.value;
        CurrentValueLabel.text = newValue.ToString();
        _nPeasantsToMove = newValue;

    }

    public void MoveAllPeasantsTo(ResourceLocation location)
    {
        var peasants = ResourceCore.CurrentGameState.GetPeasants();

        foreach (var peasant in peasants)
        {
            peasant.CurrentLocation = location;
            peasant.IsInTrasit = false;
            peasant.transform.position = NoisyTransformPosition(GetPosition(location), CrowdSizeScale);
        }
    }

    public void OnPeasantReachedDestination(ResourceLocation location)
    {
        UIManager.UpdateUI();
    }

    #endregion

    #region Helpers

    private void SubscribeToEvents()
    {
        ResourceCore.NewDayEvent.AddListener(OnNewDay);

        foreach (var building in ClickableBuildings)
        {
            building.MoveToBuildingEvent.AddListener(MovePeasants);
        }
    }

    private void OnNewDay()
    {
        MoveAllPeasantsTo(ResourceLocation.CrowdPit);
        UpdatePeasantNumberPanel();
    }

    private void UnsubscribeFromEvents()
    {
        ResourceCore.NewDayEvent.RemoveListener(OnNewDay);

        foreach (var building in ClickableBuildings)
        {
            building.MoveToBuildingEvent.RemoveListener(MovePeasants);
        }
    }

    private List<MaterialResourceType> AssignRandomAffinity(float affinityPercent)
    {
        var result = new List<MaterialResourceType>();

        if (RandomChance(affinityPercent))
        {
            result.Add(SelectRandomResource());
        }

        return result;

    }

    private MaterialResourceType SelectRandomResource()
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

    private Vector3 GetPosition(ResourceLocation location)
    {
        Vector3 result;
        switch (location)
        {
            case ResourceLocation.CrowdPit:
                result = CrowdPit.position;
                break;
            case ResourceLocation.Castle:
                result = Vector3.zero;
                break;
            case ResourceLocation.Mine:
                result = Mine.position;
                break;
            case ResourceLocation.Farm:
                result = Farm.position;
                break;
            case ResourceLocation.BlackSmiths:
                result = BlackSmith.position;
                break;
            case ResourceLocation.SacrificialPen:
                result = SACRIFICE.position;
                break;
            case ResourceLocation.Forest:
                result = Forest.position;
                break;
            case ResourceLocation.Lake:
                result = Lake.position;
                break;
            default:
                throw new ArgumentOutOfRangeException("location", "Unknown location selected to be spawned.");
        }

        return result;
    }
   
    #endregion

}
