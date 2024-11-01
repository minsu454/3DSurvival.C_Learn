using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject UIPrefabs;

    private void Awake()
    {
        Instantiate(UIPrefabs);
    }
}
