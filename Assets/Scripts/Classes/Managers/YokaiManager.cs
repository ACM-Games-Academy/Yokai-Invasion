using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class YokaiManager : MonoBehaviour
{
    private GameObject[] yokaiGameObjects = new GameObject[0];
    [SerializeField] 
    private YokaiSpawner yokaiSpawner;
    [SerializeField]
    private HordeSettings[] hordeSettings;

    private void Start()
    {
        yokaiGameObjects = yokaiSpawner.SummonHorde(hordeSettings[0]);
    }
}
