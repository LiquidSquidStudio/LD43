using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ResourceManagementCore : MonoBehaviour
{
    #region Property
    public KingData King { get; private set; }
    public DragonData Dragon { get; private set; }
    public GameResourceState ResourceState { get; private set; }
    #endregion

    #region Public Fields
    [Header("Dependencies To be wired up")]
    public ResourceManagementTestUIManager DisplayManager;
    #endregion

    #region PrivateFields
    [SerializeField]
    [Tooltip("Number of peasants on game start")]
    private int _nPeasantsOnStartup = 5;

    [SerializeField]
    [Tooltip("Stat boost multiplier")]
    private float _statBoostMax = 5;

    [SerializeField]
    [Tooltip("Percentage chance of gaining a random affinity")]
    [Range(0.0f, 100.0f)]
    private float _affinityChancePercent = 10.0f;

    [SerializeField]
    [Tooltip("Percentage chance of gaining a random affinity")]
    [Range(0, 10)]
    private int _currentDay = 1;

    private int _winSceneIndex = 1;
    private int _loseSceneIndex = 2;

    #endregion

    #region Unity lifecycles
    public void Awake()
    {
        Debug.Log("Resource management core Awake");
    }
    public void Start()
    {
        Debug.Log("Resource management core Start");
        Initialise();
        DisplayManager.UpdateUI(ResourceState, _currentDay);
    }

    #endregion

    #region Implementation

    #region For the UI

    public void AddWoodResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Wood, incrementValue);
    }

    public void SubtractWoodResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Wood, decrementValue);
    }

    public int GetWoodResource()
    {
        return ResourceState.nWoodResources;
    }

    public void AddIronResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Iron, incrementValue);
    }

    public void SubtractIronResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Iron, decrementValue);
    }

    public int GetIronResource()
    {
        return ResourceState.nIronResources;
    }

    public void AddFoodResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Food, incrementValue);
    }

    public void SubtractFoodResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Food, decrementValue);
    }

    public int GetFoodResource()
    {
        return ResourceState.nFoodResources;
    }

    public void AddWeaponResource(int incrementValue)
    {
        AddMaterialResource(MaterialResourceType.Weapon, incrementValue);
    }

    public void SubtractWeaponResource(int decrementValue)
    {
        SubtractMaterialResource(MaterialResourceType.Weapon, decrementValue);
    }

    public int GetWeaponResource()
    {
        return ResourceState.nWeaponResources;
    }

    public void AddPeasant(int incrementValue)
    {
        throw new NotImplementedException();
    }

    public void SubtractPeasantResource(int decrementValue)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Peasant> GetPeasants()
    {
        return ResourceState.Peasants;
    }

    public int GetNumberOfPeasants()
    {
        return ResourceState.nPeasants;
    }

    #endregion

    #region Core Game mechanics

    public GameResourceState DayEnd(GameResourceState currentGameState)
    {
        // Generate resources
        GenerateResources(currentGameState);

        // Let the Dragon purrr
        LetTheDragonLoose(currentGameState, Dragon, King);

        return ResourceState;
    }

    private void LetTheDragonLoose(GameResourceState currentGameState, DragonData dragon, KingData king)
    {
        int dailyAppetite = dragon.DailyAppetitite;
        int nPeasantsInPen = GetNumberOfPeastantsAt(ResourceLocation.SacrificialPen);

        EatHumans(GetPeasantsAt(ResourceLocation.SacrificialPen));

        if (dailyAppetite > nPeasantsInPen)
        {
            AttackRandomLocation(currentGameState);
        }
    }

    private void AttackRandomLocation(GameResourceState currentGameState)
    {
        var randomLocation = SelectRandomLocation();

        var peastantsToDie = GetPeasantsAt(randomLocation);

        foreach (var peasant in peastantsToDie)
        {
            peasant.Die();
        }

    }

    private void EatHumans(IEnumerable<Peasant> peasants)
    {
        var allPeasants = GetPeasants().ToList();
        
        foreach (var peasant in peasants)
        {
            allPeasants.Remove(peasant);
            //peasant.Die();
        }

        ResourceState.UpdatePeastants(allPeasants);
    }

    private void GenerateResources(GameResourceState currentState)
    {
        // Weapon
        var nPeasantsAtWeapon = GetNumberOfPeastantsAt(ResourceLocation.BlackSmiths);
        GenerateWeapons(GetWoodResource(), GetIronResource(), nPeasantsAtWeapon);

        // Food
        var nPeasantsAtFarm = GetNumberOfPeastantsAt(ResourceLocation.Farm);
        AddWeaponResource(nPeasantsAtFarm);

        // Iron
        var nPeasantsAtMine = GetNumberOfPeastantsAt(ResourceLocation.Mine);
        AddWeaponResource(nPeasantsAtMine);

        // Wood
        var nPeasantsAtForrest = GetNumberOfPeastantsAt(ResourceLocation.Forrest);
        AddWeaponResource(nPeasantsAtForrest);

    }

    private void GenerateWeapons(int nWood, int nIron, int nPeasants)
    {
        var nWeaponsToCreate = new int[] { nWood, nIron, nPeasants }.Min();

        SubtractWoodResource(nWeaponsToCreate);
        SubtractIronResource(nWeaponsToCreate);
        AddWeaponResource(nWeaponsToCreate);
    }

    public void OnDayEnd()
    {
        _currentDay++;
        Debug.Log("End of the day Detected!");
        var updatedResources = DayEnd(ResourceState);
        DisplayManager.UpdateUI(ResourceState, _currentDay);
    }

    public void OnAttackDragon()
    {
        var result = FightWithDragon(Dragon, GetWeaponResource(), GetPeasants());

        if (result == true)
        {
            GoToWin();
        } else {
            GoToLose();
        }
    }

    private void GoToLose()
    {
        SceneManager.LoadScene(_loseSceneIndex);
    }

    private void GoToWin()
    {
        SceneManager.LoadScene(_winSceneIndex);
    }

    public void Initialise()
    {
        if (DisplayManager == null)
        {
            throw new InvalidOperationException("Public Fields for Resouce Management Core is invalid. Wire it up fool.");
        }

        Random.InitState(Guid.NewGuid().GetHashCode()); // pretty much guarantees uniqueness between gameruns

        if (King == null) { King = new KingData(); }   
        if (Dragon == null) { Dragon = new DragonData(); }   
        if (ResourceState == null) {

            var peasants = GeneratePeasants(_nPeasantsOnStartup);
            ResourceState = new GameResourceState(peasants:peasants);
        }
    }

    public int GetNumberOfPeastantsAt(ResourceLocation location)
    {
        var peasants = ResourceState.Peasants;

        return peasants.Count(p => p.CurrentLocation == location);
    }

    public IEnumerable<Peasant> GetPeasantsAt(ResourceLocation location)
    {
        var peasants = ResourceState.Peasants;

        return peasants.Where(p => p.CurrentLocation == location);
    }

    /// <summary>
    /// Core logic to figh with the dragon
    /// </summary>
    /// <param name="dragon"></param>
    /// <param name=""></param>
    /// <returns></returns>
    public bool FightWithDragon(DragonData dragon, int nWeapons, IEnumerable<Peasant> fighters)
    {
        var totalFightingForce = CalculateTotalForce(nWeapons, fighters);

        bool result = totalFightingForce >= dragon.FightingStrength;

        return result;
    }

    #endregion

    #endregion  

    #region ImplementationDetail

    private int CalculateTotalForce(int nWeapons, IEnumerable<Peasant> fighters)
    {
        int result = nWeapons;

        var giftedFighters = fighters.Where(f => f.ResourceAffinity.Contains(MaterialResourceType.Weapon));

        if (giftedFighters.Count() > 0)
        {
            foreach (var giftedFighter in giftedFighters)
            {
                result += (int)giftedFighter.StatBoostBonus;
            }
        }

        return result;
    }

    private IEnumerable<Peasant> GeneratePeasants(int nPeasants)
    {
        var peasants = new List<Peasant>();

        int count = 0;

        while (count < nPeasants)
        {
            int level = 0;
            var location = ResourceLocation.CrowdPit;
            float statBoost = GenerateRandomStatBoost();
            var affinity = AssignRandomAffinity();
            bool transit = false;

            peasants.Add(
                new Peasant(
                    level: level,
                    location: location,
                    statBoostBonus: statBoost,
                    resourceAffinity: affinity,
                    inTransit: transit
                ));

            count++;
        }

        return peasants;
    }

    private List<MaterialResourceType> AssignRandomAffinity()
    {
        var result = new List<MaterialResourceType>();

        if (RandomChance(_affinityChancePercent))
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

    private ResourceLocation SelectRandomLocation()
    {
        int nLocation = Enum.GetNames(typeof(ResourceLocation)).Length;
        int randommIndex = (int)Random.Range(0.0f, nLocation);
        return (ResourceLocation)randommIndex;
    }

    private bool RandomChance(float chance)
    {
        var comparer = Random.Range(0.0f, 100.0f);

        return (comparer <= chance);
    }

    private float GenerateRandomStatBoost()
    {
        return Random.Range(0.0f, _statBoostMax);
    }

    private void AddMaterialResource(MaterialResourceType gameResource, int incrementValue)
    {
        if (incrementValue <= 0)
            return;

        switch (gameResource)
        {
            case MaterialResourceType.Wood:
                ResourceState.nWoodResources += incrementValue;
                break;
            case MaterialResourceType.Iron:
                ResourceState.nIronResources += incrementValue;
                break;
            case MaterialResourceType.Food:
                ResourceState.nFoodResources += incrementValue;
                break;
            case MaterialResourceType.Weapon:
                ResourceState.nWeaponResources += incrementValue;
                break;

            default:
                throw new ArgumentException("Unknown resource passed in to add");
        }

    }

    private void SubtractMaterialResource(MaterialResourceType gameResource, int decrementValue)
    {
        if (decrementValue <= 0) return;

        int newValue = 0;
        switch (gameResource)
        {
            case MaterialResourceType.Wood:
                newValue = ResourceState.nWoodResources - decrementValue;
                ResourceState.nWoodResources = Math.Max(0, newValue);
                break;

            case MaterialResourceType.Iron:
                newValue = ResourceState.nIronResources - decrementValue;
                ResourceState.nIronResources = Math.Max(0, newValue);
                break;

            case MaterialResourceType.Food:
                newValue = ResourceState.nFoodResources - decrementValue;
                ResourceState.nFoodResources = Math.Max(0, newValue);
                break;

            case MaterialResourceType.Weapon:
                newValue = ResourceState.nWeaponResources - decrementValue;
                ResourceState.nWeaponResources = Math.Max(0, newValue);
                break;

            default:
                throw new ArgumentException("Unknown resource passed in to add");
                
        }
    }

    #endregion

}