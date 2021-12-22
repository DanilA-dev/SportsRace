using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    void DisableButtons(float time);

    void TurnOnPlayerPedestalCam();
    void TurnOffPlayerPedestalCam();

}
