using System;
using System.Collections.Generic;
using UnityEngine;

public class CallDeath : MonoBehaviour
{
    public Action HeroDead;

    public void Die()
    {
        HeroDead?.Invoke();
        Debug.LogWarning("GAME OVER!");
        Time.timeScale = 0;
    }
}
