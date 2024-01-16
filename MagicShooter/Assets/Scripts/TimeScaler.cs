using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [SerializeField] private float _timeScale;

    private void Update()
    {
        Time.timeScale = _timeScale;
    }
}
