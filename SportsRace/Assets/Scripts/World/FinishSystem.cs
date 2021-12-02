using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSystem : MonoBehaviour
{
    [Header("Pedestal")]
    [SerializeField] private float speedToPedestal;
    [SerializeField] private float riseSpeed;
    [SerializeField] private Transform risingPedestal;
    [Header("Move Points")]
    [SerializeField] private Transform firstPlacePoint;
    [SerializeField] private Transform secondPlacePoint;

    [SerializeField] private bool firstPlaceTaken = false;
    [SerializeField] private bool secondPlaceTaken = false;


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<IRunner>() != null)
        {
            var r = other.GetComponentInParent<IRunner>();
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

    private IEnumerator MoveToPedestal(IRunner runner, Vector3 dir)
    {
        var moveTime = 3f;

        for (float i = 0; i < moveTime; i+= Time.deltaTime)
        {
            runner.GetAgent().speed = speedToPedestal;
            runner.GetAgent().SetDestination(dir);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("We went to pedestal!");
        runner.GetAgent().ResetPath();
        runner.GetAgent().enabled = false;

        StartCoroutine(RiseFirstPlace(runner));
    }

    private IEnumerator RiseFirstPlace(IRunner runner)
    {
        while(!secondPlaceTaken)
        {
            Debug.Log("Test");
            risingPedestal.localScale = new Vector3(risingPedestal.localScale.x,
                                         Mathf.Lerp(risingPedestal.localScale.y, 35, riseSpeed * Time.deltaTime),
                                                    risingPedestal.localScale.z);
            runner.GetTransform().position = new Vector3(runner.GetTransform().position.x,
                                         Mathf.Lerp(runner.GetTransform().position.y, 35, riseSpeed * Time.deltaTime),
                                                    runner.GetTransform().position.z);

            yield return null;
        }
    }
}
