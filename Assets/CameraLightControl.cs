using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Tags;

public class CameraLightControl : MonoBehaviour {
    private Volume _volume;
    private Vignette _vignette;
    private Transform _playerTransform;


    private void Awake(){
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _vignette);
        _playerTransform = GameObject.FindWithTag(playerTag).transform;
    }

    private void FixedUpdate(){
        Vector3 screenPoint =
            Camera.main.WorldToScreenPoint(_playerTransform.position).normalized;
        _vignette.center.value = Vector2.Lerp(_vignette.center.value, screenPoint*0.75f, 10f * Time.fixedDeltaTime );
    }
}