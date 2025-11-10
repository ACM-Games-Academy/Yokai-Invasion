using UnityEngine;

[CreateAssetMenu(fileName = "DirectionalLighting", menuName = "Scriptable Objects/DirectionalLighting")]
public class DirectionalLighting : ScriptableObject
{
    public Gradient AmbientColour;
    public Gradient DirectionalColour;
    public Gradient FogColour;
}
