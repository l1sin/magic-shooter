using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator _animatior;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _animatior.SetBool("Attack", true);
        }
        else
        {
            _animatior.SetBool("Attack", false);
        }
    }
}
