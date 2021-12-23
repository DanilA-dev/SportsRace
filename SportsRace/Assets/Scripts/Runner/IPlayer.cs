using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPlayer
{
    event Action<PlayerRunner> OnTapEvent;

    void DisableButtons(float time);

    void TurnOnPlayerPedestalCam();
    void TurnOffPlayerPedestalCam();

}
