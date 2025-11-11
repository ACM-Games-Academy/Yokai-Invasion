using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private BuildingSettings settings;

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

    private int wood;
    private int food;
    private int gold;

    private void Start()
    {
        settings = Overseer.Instance.Settings.BuildingSettings;
        //subscribing to events from resource manager
        Overseer.Instance.GetManager<ResourceManager>().UpdateWood += DisplayWoodCount;
        Overseer.Instance.GetManager<ResourceManager>().UpdateFood += DisplayFoodCount;
        Overseer.Instance.GetManager<ResourceManager>().UpdateGold += DisplayGoldCount;
        Overseer.Instance.GetManager<NightCycle>().DawnStarted += DisplayTimeDawn;
        Overseer.Instance.GetManager<NightCycle>().DayStarted += DisplayTimeDay;
        Overseer.Instance.GetManager<NightCycle>().DuskStarted += DisplayTimeDusk;
        Overseer.Instance.GetManager<NightCycle>().NightStarted += DisplayTimeNight;

        //checks resource cost - the most inefficient code ive written since we started
        towerGoldCost.text = $"{settings.BuildingOptions[0].GoldCost.ToString()} Gold";
        towerWoodCost.text = $"{settings.BuildingOptions[0].WoodCost.ToString()} Wood";
        lumbermillGoldCost.text = $"{settings.BuildingOptions[1].GoldCost.ToString()} Gold";
        lumbermillWoodCost.text = $"{settings.BuildingOptions[1].WoodCost.ToString()} Wood";
        farmGoldCost.text = $"{settings.BuildingOptions[2].GoldCost.ToString()} Gold";
        farmWoodCost.text = $"{settings.BuildingOptions[2].WoodCost.ToString()} Wood";
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
    }
    private void DisplayTimeDay()
    {
        timeUI.text = "Day";
    }
    private void DisplayTimeDusk()
    {
        timeUI.text = "Dusk";
    }
    private void DisplayTimeNight()
    {
        timeUI.text = "Night";
    }
}
