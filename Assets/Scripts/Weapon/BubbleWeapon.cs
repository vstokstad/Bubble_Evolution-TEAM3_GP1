using Actor.Component;
using UnityEngine;

namespace Weapon {
    public class BubbleWeapon : MonoBehaviour, WeaponManager.IWeapon {
        [SerializeField] [Tooltip("Time between Shots")]
        private float timeBetweenShots = 0.5f;

        [SerializeField] [Tooltip("How far from the player should the bubble spawn")]
        private float bubbleToPlayerPosition = 3f;

        [SerializeField] [Tooltip("How high from the player center should the bubble spawn")]
        private float bubbleToPlayerHeightPosition = 1f;

        private GameObject _player;
        private PlayerRotationController _playerRotationController;
        private Transform _playerTransform;

        private Vector3 _shootDirection;
        private float _shootTimer;

        private void Awake(){
            _player = gameObject;
            _playerTransform = _player.transform;
        }

        private void Start(){
            _shootTimer = timeBetweenShots;
            _playerRotationController = GetComponentInChildren<PlayerRotationController>();
            _shootDirection = _playerRotationController.CharacterFacingDirection;
        }

        private void Update(){
            _shootTimer -= Time.deltaTime;
        }

        private void FixedUpdate(){
            _shootDirection = _playerRotationController.CharacterFacingDirection;
        }

        public void Shoot(){
            if (!(_shootTimer <= 0f)) return;
            //  new WaitForFixedUpdate();

            GameObject bubble = WeaponPool.Instance.Get(WeaponType.Bubble);
            Vector3 initialPosition = _playerTransform.position;
            initialPosition.y += bubbleToPlayerHeightPosition;
            initialPosition += _shootDirection * bubbleToPlayerPosition;
            bubble.transform.position = initialPosition;
            bubble.SetActive(true);

            _shootTimer = timeBetweenShots;
        }
    }
}