using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinishTrack : MonoBehaviour
{
    [Header("Pedestal")]
    [SerializeField] private float speedToPedestal;
    [SerializeField] private float pedestalRiseSpeed;
    [SerializeField] private Transform risingPedestal;
    [Header("Move Points")]
    [SerializeField] private Transform pedestalMovePoint;
    [SerializeField] private Transform runnerUpPoint;
    [SerializeField] private Transform firstPlacePoint;
    [SerializeField] private Transform secondPlacePoint;
    [SerializeField] private Transform topPlatformPoint;

    private Vector3 _startRisingPedestalPos;

    private int _coinsMultiplier;
    private int _positionIndex = 0;


    private void Start()
    {
        _startRisingPedestalPos = risingPedestal.transform.position;
        risingPedestal.transform.position = _startRisingPedestalPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ARunner r))
        {
            _positionIndex++;
            Debug.Log("Finish");
            r.IsFinished = true;

            r.SetFinishPosition(_positionIndex);
            StartCoroutine(MoveToPedestal(r, PedestalPos(r.FinishIndex).position));
        }
    }

    private Transform PedestalPos(int pos)
    {
        return pos == 1 ? firstPlacePoint : secondPlacePoint;
    }

    private IEnumerator MoveToPedestal(ARunner runner, Vector3 dir)
    {
        var moveTime = 2.5f;
        runner.UnFreezeBody(RigidbodyConstraints.FreezeRotationY);
        for (float i = 0; i < moveTime; i+= Time.deltaTime)
        {
            runner.Move(dir, 150);
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            runner.transform.rotation = Quaternion.RotateTowards(runner.transform.rotation, rot, 500 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
        Debug.Log("We went to pedestal!");
        runner.Body.isKinematic = true;
        runner.RunnerAnimator.Play("Idle");
        StartCoroutine(RiseFirstPlace(runner));
    }

    private IEnumerator RiseFirstPlace(ARunner runner)
    {
        float xMultiplier = 0f;
        while (_positionIndex == 1)
        {
            Debug.Log("rise!");
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
                StartCoroutine(TopPlatform(runner));
            }
            yield return null;
        }
        Debug.Log("lose??!");
        CheckPlayerPos(runner);
    }

    private IEnumerator TopPlatform(ARunner runner)
    {
        var moveTime = 2.5f;
        runner.Body.isKinematic = false;
        for (float i = 0; i < moveTime; i += Time.deltaTime)
        {
           // runner.Move(Vector3.forward, 200);
            yield return new WaitForEndOfFrame();
        }
        CheckPlayerPos(runner);
    }

    public void OnRestart()
    {
        _positionIndex = 0;
        _coinsMultiplier = 0;
        risingPedestal.transform.position = _startRisingPedestalPos;
    }

    private void CheckPlayerPos(ARunner runner)
    {
        StopAllCoroutines();
        runner.RunnerAnimator.Play("Idle");

        if (runner as PlayerRunner && runner.FinishIndex == 1)
        {
            Debug.Log("You win!");
            GameController.CurrentState = GameState.Win;
        }
        else
        {
            Debug.Log("You lost!");
            GameController.CurrentState = GameState.Lose;
        }
    }    
}
