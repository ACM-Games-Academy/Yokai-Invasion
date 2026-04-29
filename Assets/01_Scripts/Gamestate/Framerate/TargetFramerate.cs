using UnityEngine;

public class TargetFramerate : MonoBehaviour
{
    public int framerate;
    void Start()
    {
        Application.targetFrameRate = framerate;
    }
}
