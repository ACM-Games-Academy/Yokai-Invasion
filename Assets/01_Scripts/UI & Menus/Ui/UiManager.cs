using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private BuildingSettings settings;
    private UnitSettings unitSettings;

    [Header("UI Counters")]
    public TextMeshProUGUI woodCounterUI;
    public TextMeshProUGUI foodCounterUI;
    public TextMeshProUGUI goldCounterUI;
    public TextMeshProUGUI timeUI;

    [Header("Building Resource Costs")]
    public TextMeshProUGUI towerGoldCost;
    public TextMeshProUGUI towerWoodCost;
    public TextMeshProUGUI lumbermillGoldCost;
    public TextMeshProUGUI lumbermillWoodCost;
    public TextMeshProUGUI farmGoldCost;
    public TextMeshProUGUI farmWoodCost;

    [Header("Unit Resource Costs")]
    public TextMeshProUGUI soldierGoldCost;
    public TextMeshProUGUI soldierFoodCost;
    public TextMeshProUGUI villagerGoldCost;
    public TextMeshProUGUI villagerFoodCost;

    [Header("Health Counters")]
    public TextMeshProUGUI playerHealthCounter;
    public TextMeshProUGUI templeHealthCounter;

    private int wood;
    private int food;
    private int gold;

    private GameObject hero;
    private GameObject playerHealth;
    private HeroAttack heroAttack;

    private GameObject temple;
    private GameObject templeHealth;
    private TempleHealth templeHealthScript;

    private void Start()
    {
        settings = Overseer.Instance.Settings.BuildingSettings;
        unitSettings = Overseer.Instance.Settings.UnitSettings;

        hero = GameObject.Find("TempHero");
        heroAttack = hero.GetComponent<HeroAttack>();
        heroAttack.HeroTookDamage += DisplayPlayerHealthCount;
        playerHealthCounter.text = heroAttack.CurrentHealth.ToString();
        playerHealth = GameObject.Find("Player Health");
        playerHealth.SetActive(false);

        temple = GameObject.Find("Temple");
        templeHealthScript = temple.GetComponent<TempleHealth>();
        templeHealthScript.TempleTookDamage += DisplayTempleHealthCount;
        templeHealthCounter.text = templeHealthScript.CurrentHealth.ToString();
        templeHealth = GameObject.Find("Temple Health");
        templeHealth.SetActive(false);


        var resourceManager = Overseer.Instance.GetManager<ResourceManager>();
        resourceManager.UpdateWood += DisplayWoodCount;
        resourceManager.UpdateFood += DisplayFoodCount;
        resourceManager.UpdateGold += DisplayGoldCount;

        var nightCycle = Overseer.Instance.GetManager<NightCycle>();
        nightCycle.DawnStarted += DisplayTimeDawn;
        nightCycle.DayStarted += DisplayTimeDay;
        nightCycle.DuskStarted += DisplayTimeDusk;
        nightCycle.NightStarted += DisplayTimeNight;

        //checks resource cost - the most inefficient code ive written since we started
        towerGoldCost.text = $"{settings.BuildingOptions[0].GoldCost.ToString()} Gold";
        towerWoodCost.text = $"{settings.BuildingOptions[0].WoodCost.ToString()} Wood";
        lumbermillGoldCost.text = $"{settings.BuildingOptions[1].GoldCost.ToString()} Gold";
        lumbermillWoodCost.text = $"{settings.BuildingOptions[1].WoodCost.ToString()} Wood";
        farmGoldCost.text = $"{settings.BuildingOptions[2].GoldCost.ToString()} Gold";
        farmWoodCost.text = $"{settings.BuildingOptions[2].WoodCost.ToString()} Wood";

        soldierGoldCost.text = $"{unitSettings.UnitOptions[0].GoldCost.ToString()} Gold";
        soldierFoodCost.text = $"{unitSettings.UnitOptions[0].FoodCost.ToString()} Food";
        villagerGoldCost.text = $"{unitSettings.UnitOptions[1].GoldCost.ToString()} Gold";
        villagerFoodCost.text = $"{unitSettings.UnitOptions[1].FoodCost.ToString()} Food";

        DisplayTimeDawn();
    }

    private void DisplayPlayerHealthCount()
    {
        playerHealthCounter.text = heroAttack.CurrentHealth.ToString();
    }
    private void DisplayTempleHealthCount()
    {
        templeHealthCounter.text = templeHealthScript.CurrentHealth.ToString();
    }

    //Resource Counters UI ---------------------

    private void DisplayWoodCount()
    {
        wood = Overseer.Instance.GetManager<ResourceManager>().CurrentWood();
        woodCounterUI.text = wood.ToString();
    }
    private void DisplayFoodCount()
    {
        food = Overseer.Instance.GetManager<ResourceManager>().CurrentFood();
        foodCounterUI.text = food.ToString();
    }
    private void DisplayGoldCount()
    {
        gold = Overseer.Instance.GetManager<ResourceManager>().CurrentGold();
        goldCounterUI.text = gold.ToString();
    }


    //Time UI ---------------------------
    private void DisplayTimeDawn()
    {
        timeUI.text = "Dawn";
        playerHealth.SetActive(false);
        templeHealth.SetActive(false);
    }
    private void DisplayTimeDay()
    {
        timeUI.text = "Day";
        playerHealth.SetActive(false);
        templeHealth.SetActive(false);
    }
    private void DisplayTimeDusk()
    {
        timeUI.text = "Dusk";
        playerHealth.SetActive(false);
        templeHealth.SetActive(false);
    }
    private void DisplayTimeNight()
    {
        timeUI.text = "Night";
        playerHealth.SetActive(true);
        templeHealth.SetActive(true);
        DisplayTempleHealthCount();
        DisplayPlayerHealthCount();
    }
}
