using Actor.Component;
using UnityEngine;

namespace Weapon {
    public class BubbleJump : MonoBehaviour {
        private Collider _collider;
        private DashController _dashController;
        private GroundCheck _groundCheck;
        private bool _isPassable;
        private Rigidbody _playerRigidbody;

        private void Awake(){
            _collider = GetComponent<Collider>();
            _playerRigidbody = GameObject.FindWithTag(Tags.playerTag).GetComponent<Rigidbody>();
            _groundCheck = _playerRigidbody.GetComponent<GroundCheck>();
            _isPassable = true;
            _dashController = _playerRigidbody.GetComponent<DashController>();
        }

        private void FixedUpdate(){
            if (_groundCheck.DownCollision) return;
            float verticalVelocity = _playerRigidbody.velocity.y;
            float playerPositionY = _playerRigidbody.transform.position.y;
            _isPassable = verticalVelocity > 0f && playerPositionY < transform.position.y;

            _collider.isTrigger = _isPassable;
        }

        private void OnCollisionEnter(Collision other){
            if (!other.collider.CompareTag(Tags.playerTag)) return;
            _dashController.DashReady = true;
        }

        private void OnTriggerEnter(Collider other){
            if (!other.CompareTag(Tags.playerTag)) return;
            _dashController.DashReady = true;
        }

        private void OnTriggerExit(Collider other){
            if (!other.CompareTag(Tags.playerTag)) return;
            _dashController.DashReady = true;
        }
    }
}