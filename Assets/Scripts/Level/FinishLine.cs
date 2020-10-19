using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if (!other.CompareTag(Tags.playerTag)) return;
        UIController.instance.LevelFinish(UIController.instance.seconds);
    }
}
