using UnityEngine;

[RequireComponent(typeof(Light))]
public class DirectionHelpScript : MonoBehaviour {
    private Light _light;
  
    public bool playerHasPassed = false;
    private void Awake(){
        _light = GetComponent<Light>();
     _light.color = Color.magenta;
    }

    private void FixedUpdate(){
    
        if (!playerHasPassed) return;
        _light.color = Color.green;
    }

    private void OnTriggerEnter(Collider other){
        if (!other.CompareTag(Tags.playerTag)) return;
        playerHasPassed = true;
    }
}
