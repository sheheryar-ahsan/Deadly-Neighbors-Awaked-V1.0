using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 5f;

    private void Awake()
    {
        Destroy(this.gameObject, time);
    }
}
