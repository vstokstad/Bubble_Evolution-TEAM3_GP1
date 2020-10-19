using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon {
    public class WeaponPool : MonoBehaviour {
        [SerializeField] [Tooltip("Add the 'Bubble' prefab here")]
        private GameObject bubblePrefab;

        [SerializeField] [Range(1, 20)] private int numberOfPreloadedBubbles = 15;
        private Queue<GameObject> _bubbleQue;
        private bool prefabIsLoaded = true;
        public static WeaponPool Instance { get; private set; }

        private void Awake(){
            if (Instance != null) {
                Debug.LogError(
                    $"Only one weaponPool is allowed to exist in the scene at once. Destroying object '{this}'...");
                Destroy(this);
                return;
            }

            Instance = this;
            //handling user error where the prefab is not set in the editor.
            if (bubblePrefab == null) {
                prefabIsLoaded = false;
                throw new NullReferenceException("no bubblePrefab loaded in the weapon pool script in the editor");
            }


            _bubbleQue = new Queue<GameObject>(numberOfPreloadedBubbles);
            if (prefabIsLoaded) AddWeapon(_bubbleQue, bubblePrefab, numberOfPreloadedBubbles);
        }

        private void Start(){
            _bubbleQue.TrimExcess();
        }

        public GameObject Get(WeaponType currentWeapon){
            switch (currentWeapon) {
                case WeaponType.Bubble:
                    if (prefabIsLoaded) {
                        if (_bubbleQue.Count == 0) AddWeapon(_bubbleQue, bubblePrefab, 1);

                        if (_bubbleQue.Peek().activeInHierarchy != true) return _bubbleQue.Dequeue();
                        GameObject[] bubbleArray = _bubbleQue.ToArray();
                        foreach (GameObject o in bubbleArray) {
                            if (o.activeSelf) continue;
                            if (o.activeInHierarchy) continue;
                            return o;
                        }

                        return _bubbleQue.Dequeue();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentWeapon), currentWeapon, null);
            }

            throw new NullReferenceException("No prefab loaded in the Weapon pool script in the editor.");
        }

        private static void AddWeapon(Queue<GameObject> queue, GameObject weaponPrefab, int number){
            for (int i = 0; i < number; i++) {
                GameObject shot = Instantiate(weaponPrefab);
                shot.gameObject.SetActive(false);
                queue.Enqueue(shot);
            }
        }

        public void ReturnToPool(WeaponType weaponType, GameObject shot){
            switch (weaponType) {
                case WeaponType.Bubble:
                    _bubbleQue.Enqueue(shot);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
        }
    }
}