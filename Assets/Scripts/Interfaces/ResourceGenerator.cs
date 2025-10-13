using UnityEngine;

public interface ResourceGenerator
{
    public void GenerateResource(int amount);
    public int GetProduction();
}
