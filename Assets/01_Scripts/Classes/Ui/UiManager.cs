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

    private const int TOWER_INDEX = 0;



    void Update()
    {
        DisplayWoodCount();
        DisplayFoodCount();
        DisplayGoldCount();
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
        Overseer.Instance.GetManager<BuildingSpawner>().SpawnAtIndex(TOWER_INDEX);
    }
}
