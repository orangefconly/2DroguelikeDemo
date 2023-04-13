using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameManager gameManager;
    public void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
