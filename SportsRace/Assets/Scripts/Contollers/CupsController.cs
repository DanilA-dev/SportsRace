using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsController : MonoBehaviour
{
    [SerializeField] private UserData data;
    [SerializeField] private List<Cup> shelfCups = new List<Cup>();

    public List<Cup> ShelfCups => shelfCups;
    public Cup NextCup { get; private set; }

    private void Awake()
    {
        UpdateCups();
    }
   

    private void UpdateCups()
    {

        for (int i = 0; i < data.Cups; i++)
        {
            if(data.Cups < shelfCups.Count)
                shelfCups[i].UnLock();
        }
    }

   

    

}
