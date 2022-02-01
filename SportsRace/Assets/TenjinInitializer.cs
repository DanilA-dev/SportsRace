using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenjinInitializer : MonoBehaviour
{
   private void Start()
   {
        TenjinConnect();
   }

   private void OnApplicationPause(bool pauseStatus)
   {
        if (!pauseStatus)
            TenjinConnect();
   }

    public static void TenjinConnect()
    {
        BaseTenjin instance = Tenjin.getInstance("FTNSVXSVBJYARJM4HPHM8X9PYIPDWO1V");
        instance.Connect();
    }

}
