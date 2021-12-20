using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] private Button toMenuButton;
    [SerializeField] private float timeToEnableButton;

    private void OnEnable()
    {
        toMenuButton.enabled = false;
        StartCoroutine(Timer(timeToEnableButton));
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        toMenuButton.enabled = true;
    }
}
