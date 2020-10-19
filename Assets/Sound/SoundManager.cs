using System;
using UnityEngine;

namespace Sound {
    public class SoundManager : MonoBehaviour {
        private AudioClip _audioClip;
        private AudioSource _audioSource;

        private void Awake(){
            _audioSource = GetComponent<AudioSource>();
            _audioClip = _audioSource.clip;
        }

        private void Start(){
            PlaySound(_audioSource, _audioClip);
        }

        public static void PlaySound(AudioSource audioSource, AudioClip audioClip){
            switch (audioClip.loadState) {
                case AudioDataLoadState.Unloaded:
                    audioClip.LoadAudioData();
                    break;
                case AudioDataLoadState.Loading:
                    WaitUntil waitUntil = new WaitUntil(audioClip.LoadAudioData);
                    waitUntil.MoveNext();
                    if (!audioSource.isPlaying) audioSource.Play();
                    break;
                case AudioDataLoadState.Loaded:
                    if (!audioSource.isPlaying) audioSource.Play();
                    break;
                case AudioDataLoadState.Failed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}