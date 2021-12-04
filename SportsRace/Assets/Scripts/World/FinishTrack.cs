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
            r.FinishStop();
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
        var moveTime = 3f;
        runner.UnFreezeBody(RigidbodyConstraints.FreezeRotationY);
        for (float i = 0; i < moveTime; i+= Time.deltaTime)
        {
            runner.transform.position = Vector3.MoveTowards(runner.transform.position,
                                                            dir, speedToPedestal * Time.deltaTime);
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            runner.transform.rotation = Quaternion.RotateTowards(runner.transform.rotation, rot, 500 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
        Debug.Log("We went to pedestal!");
        StartCoroutine(RiseFirstPlace(runner));
    }

    private IEnumerator RiseFirstPlace(ARunner runner)
    {
        float xMultiplier = 0f;
        while (_positionIndex == 1 && runner.IsFinished && runner as PlayerRunner)
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
                StartCoroutine(TopPlatform(runner));
            }
            yield return null;
        }
        CheckPlayerPos(runner);
    }

    private IEnumerator TopPlatform(ARunner runner)
    {
        var moveTime = 3f;
        for (float i = 0; i < moveTime; i += Time.deltaTime)
        {
            runner.transform.position = Vector3.MoveTowards(runner.transform.position, topPlatformPoint.position,
                                                           3 * Time.deltaTime);
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
        if(runner as PlayerRunner && runner.FinishIndex == 1)
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
