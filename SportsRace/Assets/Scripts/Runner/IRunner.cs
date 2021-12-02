using UnityEngine;
using UnityEngine.AI;

public interface IRunner 
{
    NavMeshAgent GetAgent();
    Transform GetTransform();

    void Move(Vector3 dir, float speed);
    void FinishStop();


}
