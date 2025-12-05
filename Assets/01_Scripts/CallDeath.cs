using System;
using System.Collections.Generic;
using UnityEngine;

public class CallDeath : MonoBehaviour
{
    public Action HeroDead;

    private AudioSettings audioSettings;
    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        audioSettings = Overseer.Instance.Settings.AudioSettings;
    }

    public void Die()
    {
        HeroDead?.Invoke();
        Debug.LogWarning("GAME OVER!");
        Time.timeScale = 0;
    }
}
