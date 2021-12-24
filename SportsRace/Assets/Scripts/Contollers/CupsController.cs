using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsController : MonoBehaviour
{
    [SerializeField] private List<Cup> shelfCups = new List<Cup>();

    public List<Cup> ShelfCups => shelfCups;


    private void OnEnable()
    {
        for (int i = 0; i < GameController.Data.Cups; i++)
        {
            shelfCups[i].UnLock();
        }
    }

    

}
