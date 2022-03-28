using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    // base class for all future states
    public virtual State Tick(ZombieManager zombieManager)
    {
        Debug.Log("Running State");
        return this;
    }
}
