using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[System.Serializable]
public class CoreEvent : UnityEvent { }

public class ResourceManagementCore : MonoBehaviour
{
    #region Property
    public GameState CurrentGameState { get; private set; }

    public CoreEvent UpdateUIEvent;
    public CoreEvent NewDayEvent;
    public CoreEvent DayEndEvent;


    public int CurrentDay { get; private set; }
    #endregion

    //#region Public Fields
    //[Header("Dependencies To be wired up")]
    //public ResourceManagementTestUIManager DisplayManager;
    //#endregion

    #region PrivateFields
    //[SerializeField]
    //[Tooltip("Number of peasants on game start")]
    //private int _nPeasantsOnStartup = 5;

    //[SerializeField]
    //[Tooltip("Stat boost multiplier")]
    //private float _statBoostMax = 5;

    //[SerializeField]
    //[Tooltip("Percentage chance of gaining a random affinity")]
    //[Range(0.0f, 100.0f)]
    //private float _affinityChancePercent = 10.0f;


    private int _winSceneIndex = 1;
    private int _loseSceneIndex = 2;

    #endregion

    #region Unity lifecycles
    public void Awake()
    {
        Debug.Log("Resource management core Awake");
        Initialise();
    }
    public void Start()
    {
        Debug.Log("Resource management core Start");
    }
    #endregion

    #region Implementation


    public void DayEnd(GameState gameState)
    {
        // Generate resources
        GenerateResources(gameState);

        // Let the Dragon purrr
        LetTheDragonLoose(gameState);
    }

    public void OnDayEnd()
    {
        if (DayEndEvent != null) DayEndEvent.Invoke();
        CurrentDay++;
        Debug.Log("End of the day Detected!");
        DayEnd(CurrentGameState);

        //DisplayManager.UpdateUI(ResourceState, _currentDay);

        if (UpdateUIEvent != null) UpdateUIEvent.Invoke();
        if (NewDayEvent != null) NewDayEvent.Invoke();

    }

    public void OnAttackDragon()
    {
        var result = FightWithDragon(CurrentGameState.Dragon, CurrentGameState.GetWeaponResource(), CurrentGameState.GetPeasants());

        if (result == true)
        {
            GoToWin();
        } else {
            GoToLose();
        }
    }

    public void Initialise()
    {
        if (CurrentGameState == null)
        {
            CurrentGameState = new GameState();
        }

        Random.InitState(Guid.NewGuid().GetHashCode()); // pretty much guarantees uniqueness between gameruns

        
    }

    public int GetNumberOfPeastantsAt(ResourceLocation location)
    {
        var peasants = CurrentGameState.ResourceState.Peasants;

        return peasants.Count(p => p.CurrentLocation == location);
    }

    public IEnumerable<Peasant> GetPeasantsAt(ResourceLocation location)
    {
        var peasants = CurrentGameState.ResourceState.Peasants;

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


    #region ImplementationDetail

    private void GoToLose()
    {
        SceneManager.LoadScene(_loseSceneIndex);
    }

    private void GoToWin()
    {
        SceneManager.LoadScene(_winSceneIndex);
    }

    private void LetTheDragonLoose(GameState gameState)
    {
        var currentResourceState = gameState.ResourceState;
        var dragon = gameState.Dragon;
        var king = gameState.King;

        int dailyAppetite = dragon.DailyAppetitite;
        int nPeasantsInPen = GetNumberOfPeastantsAt(ResourceLocation.SacrificialPen);

        EatHumans(GetPeasantsAt(ResourceLocation.SacrificialPen));

        if (dailyAppetite > nPeasantsInPen)
        {
            AttackRandomLocation();
        }
    }

    private void AttackRandomLocation()
    {
        var randomLocation = SelectRandomLocation();

        var peastantsToDie = GetPeasantsAt(randomLocation);

        foreach (var peasant in peastantsToDie)
        {
            peasant.Die();
        }

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

    private void EatHumans(IEnumerable<Peasant> peasants)
    {
        var allPeasants = CurrentGameState.GetPeasants().ToList();

        foreach (var peasant in peasants)
        {
            allPeasants.Remove(peasant);
            //peasant.Die();
        }

        CurrentGameState.ResourceState.UpdatePeastants(allPeasants);
    }

    private void GenerateResources(GameState currentState)
    {
        // Weapon
        var nPeasantsAtWeapon = GetNumberOfPeastantsAt(ResourceLocation.BlackSmiths);
        GenerateWeapons(currentState.GetWoodResource(), currentState.GetIronResource(), nPeasantsAtWeapon);

        // Food
        var nPeasantsAtFarm = GetNumberOfPeastantsAt(ResourceLocation.Farm);
        currentState.AddFoodResource(nPeasantsAtFarm);

        // Iron
        var nPeasantsAtMine = GetNumberOfPeastantsAt(ResourceLocation.Mine);
        currentState.AddIronResource(nPeasantsAtMine);

        // Wood
        var nPeasantsAtForrest = GetNumberOfPeastantsAt(ResourceLocation.Forest);
        currentState.AddWoodResource(nPeasantsAtForrest);

    }

    private void GenerateWeapons(int nWood, int nIron, int nPeasants)
    {
        Debug.Log("Creating weapons with wood: " + nWood + " iron: " + nIron + " peasants: " + nPeasants);
        var nWeaponsToCreate = new int[] { nWood, nIron, nPeasants }.Min();
        Debug.Log("Making " + nWeaponsToCreate + " weapons.");

        CurrentGameState.SubtractWoodResource(nWeaponsToCreate);
        CurrentGameState.SubtractIronResource(nWeaponsToCreate);
        CurrentGameState.AddWeaponResource(nWeaponsToCreate);
    }

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
    

    #endregion

}