using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsController : MonoBehaviour
{
    [SerializeField] private List<Cup> shelfCups = new List<Cup>();

    public List<Cup> ShelfCups => shelfCups;


    private void Start()
    {
        GameController.OnMenuEnter += OnMenuEnter;
    }

    private void OnDestroy()
    {
        GameController.OnMenuEnter -= OnMenuEnter;
    }

    private void OnMenuEnter()
    {
        for (int i = 0; i < GameController.Data.Cups; i++)
        {
            shelfCups[i].UnLock();
        }
    }

   

    

}
