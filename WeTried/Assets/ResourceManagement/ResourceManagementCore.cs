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
    #endregion

    #region Public Fields
    [Header("Game Initialisation settings")]
    public int StartFoodResource = 30;
    public int StartWoodResource = 0;
    public int StartIronResource = 0;
    public int StartWeaponResource = 0;
    [Space]
    [Header("Game Config")]
    [Tooltip("Number of peasants one food feeds")]
    public int PeasantsPerFood = 5;
    #endregion

    #region PrivateFields
    private int _winSceneIndex = 1;
    private int _loseSceneIndex = 2;

    #endregion

    #region Unity lifecycles
    public void Awake()
    {
        Initialise();
    }
    #endregion

    #region Implementation
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameState"></param>
    /// <returns>Game end</returns>
    public bool DayEnd(GameState gameState)
    {
        bool result = false;

        // Generate resources
        GenerateResources(gameState);

        // Feed the peasants with food
        FeedPeasants(gameState);

        // Let the Dragon purrr
        LetTheDragonLoose(gameState);

        Debug.Log("nPeasants : " + CurrentGameState.GetNumberOfPeasants());

        if (CurrentGameState.GetNumberOfPeasants() < 1)
        {
            result = true;
        }

        return result;
    }

    public void OnDayEnd()
    {
        Debug.Log("End of the day Detected!");
        if (DayEndEvent != null) DayEndEvent.Invoke();

        CurrentGameState.IncrementDay(1);
        bool gameEnd = DayEnd(CurrentGameState);

        //DisplayManager.UpdateUI(ResourceState, _currentDay);

        if (gameEnd)
        {
            GoToLose();
            return;
        } 

        if (NewDayEvent != null) NewDayEvent.Invoke();
        if (UpdateUIEvent != null) UpdateUIEvent.Invoke();

        if (PersistingData.storyProgression <= 3)
        {
            Debug.Log("Progressing story");
            PersistingData.gs = CurrentGameState;
            SceneManager.LoadScene(3);
        }
    }

    public void OnAttackDragon()
    {
        var result = FightWithDragon(CurrentGameState.Dragon, CurrentGameState.GetWeaponResource(), CurrentGameState.GetPeasants());

        if (result == true)
        {
            GoToWin();
        }

        OnDayEnd();
    }

    public void Initialise()
    {
        if (PersistingData.storyProgression == 0 && CurrentGameState == null)
        {
            CurrentGameState = new GameState(foodResource: StartFoodResource);
        }
        else
        {
            CurrentGameState = PersistingData.gs;
        }

        Random.InitState(Guid.NewGuid().GetHashCode()); // pretty much guarantees uniqueness between gameruns
    }

    public int GetNumberOfPeastantsAt(ResourceLocation location)
    {
        var peasants = CurrentGameState.ResourceState.Peasants;

        return peasants.Count(p => (p.CurrentLocation == location) && (p.IsInTrasit == false));
    }

    public IEnumerable<Peasant> GetPeasantsAt(ResourceLocation location)
    {
        var peasants = CurrentGameState.ResourceState.Peasants;

        return peasants.Where(p => (p.CurrentLocation == location) && (p.IsInTrasit == false));
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

    public void RemovePeasant(Peasant peasant)
    {
        var peasants = CurrentGameState.GetPeasants().ToList();
        peasants.Remove(peasant);
        CurrentGameState.ResourceState.UpdatePeastants(peasants);
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
        var dragon = gameState.Dragon;

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
            peasant.Die();
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

    private void FeedPeasants(GameState gameState)
    {
        int nFood = gameState.GetFoodResource();
        int nPossibleSurvivers = nFood * PeasantsPerFood;
        int nTotalPeasants = gameState.GetNumberOfPeasants();

        Debug.Log("Food: " + nFood.ToString() + " Peasants: " + nTotalPeasants.ToString());

        if (nPossibleSurvivers >= nTotalPeasants)
        {
            int nFoodToEat = (int)nTotalPeasants / PeasantsPerFood;
            gameState.ResourceState.nFoodResources -= nFoodToEat;
            Debug.Log("Survived. Ate Food: " + nFoodToEat);
        }
        else
        {
            int nDead = nTotalPeasants - nPossibleSurvivers;
            var peasants = gameState.GetPeasants().ToList();

            Debug.Log("Hunger Death. Casualites: " + nDead.ToString());


            for (int i = 0; i < nDead; i++)
            {
                int randomIndex = Random.Range(0, nTotalPeasants);

                var peasant = peasants.Skip(randomIndex).Take(1).First();

                peasants.Remove(peasant);
                peasant.Die();
            }

            // update list
            gameState.ResourceState.UpdatePeastants(peasants);
        }
    }

    #endregion
}