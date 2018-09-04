using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    public Transform target;

    private bool LookedAtTarget;

    void OnEnable()
    {
        LookedAtTarget = false;
    }

    void Update()
    {
        if (!LookedAtTarget)
        {
            transform.LookAt(target.position);
            LookedAtTarget = true;
        }    
    }
}
