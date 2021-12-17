using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerRightSwitch : MonoBehaviour
{
    [SerializeField] private Image awesomeIcon;
    [SerializeField] private float appearTime;
    [SerializeField] private float disappearTime;
    [SerializeField] Sprite[] sprites;

    private void OnEnable()
    {
        TrackEntity.OnRightSwitchPlayer += RightSwitch;
    }

    private void OnDestroy()
    {
        TrackEntity.OnRightSwitchPlayer -= RightSwitch;
    }

    private void RightSwitch()
    {
        var rand = sprites[Random.Range(0, sprites.Length)];
        awesomeIcon.sprite = rand;

        var seq = DOTween.Sequence();
        awesomeIcon.transform.DORewind();
        seq.Join(awesomeIcon.DOFade(1, 0.3f));
        seq.Append(awesomeIcon.transform.DOScale(1, appearTime).From(0));
        seq.Append(awesomeIcon.transform.DOShakeRotation(0.2f, 10));
        seq.Append(awesomeIcon.DOFade(0, disappearTime));
    }
}
