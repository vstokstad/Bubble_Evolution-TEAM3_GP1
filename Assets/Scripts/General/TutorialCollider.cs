using UnityEngine;

public class TutorialCollider : MonoBehaviour
{
    public string tutorialMessage;

    private void OnTriggerEnter(Collider other){
        if (!other.CompareTag(Tags.playerTag)) return;
        UIController.instance.UpdateTutorialText(tutorialMessage);
    }
}
