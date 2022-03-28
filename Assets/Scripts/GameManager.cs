using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameUIManager gameUIManager;

    private void Awake()
    {
        gameUIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameUIManager>();
    }

    private void Update()
    {
        GameStop();
    }

    private void GameStop()
    {
        if (gameUIManager.healthSlider.value <= 0)
        {
            gameUIManager.GameEnd();
        }
    }
}
