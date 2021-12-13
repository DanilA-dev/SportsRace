using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class FinishTrack : ATrackEvent
{
    [Header("Pedestal")]
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


    private int _coinsMultiplier = 1;
    private int _positionIndex = 0;
    private bool _isColliding;
    
    public override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
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
            xMultiplier += Time.deltaTime;
            _coinsMultiplier = Mathf.RoundToInt(xMultiplier);
            if (risingPedestal.transform.position == pedestalMovePoint.position && runner.FinishIndex == 1 && runner as PlayerRunner)
            {
                _coinsMultiplier = 10;
                StopAllCoroutines();
                TopPlatform(runner);
            }
            yield return null;
        }
        CheckPlayerPos(runner);
    }

    private void TopPlatform(ARunner runner)
    {
        runner.RunnerAnimator.Play("Normal Sprint");
        var seq = DOTween.Sequence();
        seq.Append(runner.transform.DOMove(topPlatformPoint.position, 1.3f));
        seq.Join(runner.transform.DORotate(new Vector3(0, topPlatformPoint.position.y, 0), 1));
        seq.OnComplete(() =>OnPlayerWin(runner));

    }

    private void OnPlayerWin(ARunner runner)
    {
        int endMultiplier = _coinsMultiplier < 1 ? 1 : _coinsMultiplier;
        int totalPoints = endMultiplier * GameController.SessionScore;
        Debug.Log(totalPoints);
        GameController.Data.Coins += totalPoints;
        runner.RunnerAnimator.Play("Victory");
        GameController.CurrentState = GameState.Win;
        GameController.Data.WinsToNextRank++;
        SaveController.SaveData();
    }


    private void CheckPlayerPos(ARunner runner)
    {
        StopAllCoroutines();
        runner.SetFinishAnimation();
        

        if (runner as PlayerRunner && runner.FinishIndex == 1)
        {
            OnPlayerWin(runner);
        }
        else
        {
            if (runner as PlayerRunner)
                runner.RunnerAnimator.Play("Defeated");

            GameController.Data.Coins += GameController.SessionScore;
            SaveController.SaveData();
            GameController.CurrentState = GameState.Lose;
        }
    } 
    

    public override void Unsubscribe()
    {
        _positionIndex = 0;
        _coinsMultiplier = 1;
        StopAllCoroutines();
    }
}
