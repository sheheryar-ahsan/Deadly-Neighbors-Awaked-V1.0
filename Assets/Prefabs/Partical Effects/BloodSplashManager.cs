using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplashManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyTheObject());
    }
    IEnumerator DestroyTheObject()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
