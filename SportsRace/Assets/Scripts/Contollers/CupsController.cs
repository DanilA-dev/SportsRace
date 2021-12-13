using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> shelfCups = new List<GameObject>();

    private void OnEnable()
    {
        for (int i = 0; i < GameController.Data.Cups; i++)
        {
            shelfCups[i].SetActive(true);
        }
    }

}
