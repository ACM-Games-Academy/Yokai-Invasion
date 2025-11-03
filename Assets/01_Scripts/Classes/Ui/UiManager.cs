using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("UI Counters")]
    public TextMeshProUGUI woodCounterUI;
    public TextMeshProUGUI foodCounterUI;
    public TextMeshProUGUI goldCounterUI;

    private int wood;
    private int food;
    private int gold;

    private void Start()
    {
        //subscribing to events from resource manager
        Overseer.Instance.GetManager<ResourceManager>().UpdateWood += DisplayWoodCount;
        Overseer.Instance.GetManager<ResourceManager>().UpdateFood += DisplayFoodCount;
        Overseer.Instance.GetManager<ResourceManager>().UpdateGold += DisplayGoldCount;
    }

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

    public void SpawnTower()
    {
        if (Overseer.Instance.GetManager<BuildingSpawner>().buildModeState == BuildingSpawner.BuildMode.active)
        {
            Overseer.Instance.GetManager<BuildingSpawner>().SpawnAtIndex(Overseer.Instance.GetManager<BuildingSpawner>().indexDictionary["Tower"]);
        }
    }
}
