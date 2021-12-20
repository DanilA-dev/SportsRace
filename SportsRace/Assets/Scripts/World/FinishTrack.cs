using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class FinishTrack : ATrackEvent
{
    [Header("Pedestal")]
    [SerializeField] private float multiplierOffset;
    [SerializeField] private float jumpForce;
    [SerializeField] private float pedestalRiseSpeed;
    [SerializeField] private Vector3 jumpOffset;
    [SerializeField] private Transform risingPedestal;
    [Header("Move Points")]
    [SerializeField] private Transform pedestalMovePoint;
    [SerializeField] private Transform runnerUpPoint;
    [SerializeField] private Transform firstPlacePoint;
    [SerializeField] private Transform secondPlacePoint;
    [SerializeField] private Transform topPlatformPoint;

    [SerializeField] private UnityEvent Onx10Platform;
    [SerializeField] private UnityEvent OnPlatformRise;

    public static event Action OnCupEarned;


    private int _coinsMultiplier = 1;
    private int _positionIndex = 0;
    private bool _isColliding;
    
    public override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            r.StopRunnerSpecialParticles();
            if (r.IsFinished)
                return;

            r.IsFinished = true;
            _positionIndex++;
            r.SetFinishPosition(_positionIndex);
            r.State = RunnerState.Finish;
            Debug.Log($"{r.gameObject.name} is Finishend in {r.FinishIndex} place");
            StartCoroutine(JumpToPedestal(r, PedestalPos(r.FinishIndex).position));
        }
    }

    private Transform PedestalPos(int pos)
    {
        return pos == 1 ? firstPlacePoint : secondPlacePoint;
    }

    private IEnumerator JumpToPedestal(ARunner r, Vector3 dir)
    {
        r.transform.LookAt(dir);
        r.transform.DOMove(dir, 1);
        yield return new WaitForSeconds(1);
        r.SetFinishAnimation();
        r.Body.isKinematic = true;
        r.transform.DORotate(new Vector3(0, -180, 0),1);
        StartCoroutine(RiseFirstPlace(r));
    }

    private IEnumerator RiseFirstPlace(ARunner runner)
    {
        float xMultiplier = 0f;
        while (_positionIndex == 1)
        {
            var pedestalT = risingPedestal.transform.position;
            var runnerT = runner.transform.position;

            risingPedestal.transform.position = Vector3.MoveTowards(pedestalT, pedestalMovePoint.position, pedestalRiseSpeed * Time.deltaTime);
            runner.transform.position = Vector3.MoveTowards(runnerT, runnerUpPoint.position, pedestalRiseSpeed * Time.deltaTime);
            xMultiplier += Time.deltaTime * multiplierOffset;
            _coinsMultiplier = Mathf.RoundToInt(xMultiplier);
            if (risingPedestal.transform.position == pedestalMovePoint.position && runner.FinishIndex == 1 && runner as PlayerRunner)
            {
                PlayerRunner player = runner as PlayerRunner;

                player.TurnOnFinishCamera();

                _coinsMultiplier = 10;
                StopAllCoroutines();
                TopPlatform(runner);
                OnCupEarned?.Invoke();
            }
            yield return null;
        }
        CheckPlayerPos(runner);
    }

    private void TopPlatform(ARunner runner)
    {
        runner.RunnerAnimator.Play("Normal Sprint");
        Onx10Platform?.Invoke();
        var seq = DOTween.Sequence();
        seq.Append(runner.transform.DOMove(topPlatformPoint.position, 1.3f));
        seq.Join(runner.transform.DORotate(new Vector3(0, topPlatformPoint.position.y, 0), 1));
        seq.OnComplete(() =>Onx10PlayerWin(runner));

    }

    private void Onx10PlayerWin(ARunner runner)
    {
        int endMultiplier = _coinsMultiplier < 1 ? 1 : _coinsMultiplier;
        int totalPoints = endMultiplier * GameController.SessionScore;

        Debug.Log(totalPoints);
        runner.RunnerAnimator.Play("Victory");
        GameController.Data.Cups++;
        GameController.Data.Coins += totalPoints;
        GameController.CurrentState = GameState.Win;
        GameController.Data.WinsToNextRank++;
        SaveController.SaveData();
    }


    private void CheckPlayerPos(ARunner runner)
    {
       // StopAllCoroutines();
        runner.CheckPosition();
    } 
    

    public override void Unsubscribe()
    {
        _positionIndex = 0;
        _coinsMultiplier = 1;
        StopAllCoroutines();
    }
}
