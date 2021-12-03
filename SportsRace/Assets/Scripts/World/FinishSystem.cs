using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSystem : MonoBehaviour
{
    [SerializeField] private int multiplier;
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
    [Space]
    [SerializeField] private bool firstPlaceTaken = false;
    [SerializeField] private bool secondPlaceTaken = false;

    private Vector3 _startRisingPedestalPos;

    private void Start()
    {
        _startRisingPedestalPos = risingPedestal.transform.position;
        risingPedestal.transform.position = _startRisingPedestalPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ARunner>() != null)
        {
            var r = other.GetComponent<ARunner>();
            CheckFirst();
            r.FinishStop();
            StartCoroutine(MoveToPedestal(r, firstPlacePoint.position));
        }
    }

    private void CheckFirst()
    {
        if (firstPlaceTaken)
            secondPlaceTaken = true;

        if (!firstPlaceTaken)
            firstPlaceTaken = true;
    }

    private IEnumerator MoveToPedestal(ARunner runner, Vector3 dir)
    {
        var moveTime = 3f;

        for (float i = 0; i < moveTime; i+= Time.deltaTime)
        {
            runner.Agent.speed = speedToPedestal;
            runner.Agent.Warp(dir);
            //runner.Agent.destination = dir;
            yield return new WaitForEndOfFrame();
        }
        runner.Agent.ResetPath();
        runner.Agent.enabled = false;
        Debug.Log("We went to pedestal!");

        StartCoroutine(RiseFirstPlace(runner));
    }

    private IEnumerator RiseFirstPlace(ARunner runner)
    {
        float xMultiplier = 0f;
        while (!secondPlaceTaken)
        {
            var pedestalT = risingPedestal.transform.position;
            var runnerT = runner.transform.position;

            risingPedestal.transform.position = Vector3.MoveTowards(pedestalT, pedestalMovePoint.position, pedestalRiseSpeed * Time.deltaTime);
            runner.transform.position = Vector3.MoveTowards(runnerT, runnerUpPoint.position, pedestalRiseSpeed * Time.deltaTime);
            xMultiplier += Time.deltaTime;
            multiplier = Mathf.RoundToInt(xMultiplier);
            if (risingPedestal.transform.position == pedestalMovePoint.position)
            {
                multiplier = 10;
                StopAllCoroutines();
                StartCoroutine(TopPlatform(runner));
            }
            yield return null;
        }
        PlayerWin();
    }

    private IEnumerator TopPlatform(ARunner runner)
    {
        var moveTime = 3f;

        for (float i = 0; i < moveTime; i += Time.deltaTime)
        {
            runner.transform.position = Vector3.MoveTowards(runner.transform.position, topPlatformPoint.position,
                                                            1 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        PlayerWin();
       
    }

    private void PlayerWin()
    {
        StopAllCoroutines();
        GameController.CurrentState = GameState.Win;
        Debug.Log("PlayerWin");
    }    
}
