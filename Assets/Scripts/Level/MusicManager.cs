using UnityEngine;
using static Tags;

namespace Level {
    public class MusicManager : MonoBehaviour {
        public float lerpSpeed = 4f;
        private AudioSource _music;
        private Transform _playerTransform;
        private MainGameManager _timeComponent;
        private float _timer;

        private void Awake(){
            _music = GetComponent<AudioSource>();
            _playerTransform = GameObject.FindWithTag(playerTag).transform;
            _timeComponent = FindObjectOfType<MainGameManager>();
            _timer = _timeComponent.timeComponent.Timer;
            _music.spatialBlend = 1f;
            _music.pitch = 0f;
        }

        private void FixedUpdate(){
            float currentYPos = _playerTransform.position.y / 20f;
            currentYPos = Mathf.Clamp01(currentYPos);
            if (currentYPos < .9f && _timer > 250) {
                _music.pitch = Mathf.Lerp(_music.pitch, currentYPos, lerpSpeed * Time.fixedDeltaTime);
                _music.spatialBlend = Mathf.Lerp(_music.spatialBlend + currentYPos, 0, Time.fixedDeltaTime);
            }
            else {
                _music.pitch = 1f;
                _music.spatialBlend = 0f;
            }

            
        }
    }
}