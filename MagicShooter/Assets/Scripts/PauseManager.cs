using System;
using UnityEngine;

public static class PauseManager
{
    public static bool Paused;
    public static float CurrentTimeScale = 1f;
    public static event Action PauseOn;
    public static event Action PauseOff;
    public static void SetPause(bool state)
    {
        Paused = state;
        if (Paused)
        {
            PauseOn?.Invoke();
            CurrentTimeScale = 0;
            Time.timeScale = CurrentTimeScale;
        }
        else
        {
            PauseOff?.Invoke();
            CurrentTimeScale = 1;
            Time.timeScale = CurrentTimeScale;
        }
    }

    public static void TogglePause()
    {
        SetPause(!Paused);
    }
}
