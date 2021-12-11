using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CoinsTrigger : MonoBehaviour
{
    [SerializeField] private int coinsAfterTrack;
    [SerializeField] private UnityEvent OnCoinGet;

    private void OnTriggerExit(Collider other)
    {
        PlayerRunner player = other.GetComponent<PlayerRunner>();
        if(player)
        {
            GameController.GetSessionScore(coinsAfterTrack);
            OnCoinGet?.Invoke();
        }
    }
    
}
