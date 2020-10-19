using Actor.Component;
using UnityEngine;
using static Tags;

namespace Level {
    [RequireComponent(typeof(Collider))]
    public class OneWayPlatformJump : MonoBehaviour {
        private Collider _collider;
        private GroundCheck _groundCheck;
        private bool _isOneWay;
        private bool _isPassable;
        private Rigidbody _playerRigidbody;

        private void Awake(){
            _collider = GetComponent<Collider>();
            _playerRigidbody = GameObject.FindWithTag(playerTag).GetComponent<Rigidbody>();
            _groundCheck = _playerRigidbody.GetComponent<GroundCheck>();
        }

        private void Start(){
            _isOneWay = CompareTag(oneWayPlatformTag);
        }

        private void FixedUpdate(){
            if (_groundCheck.DownCollision) {
                _collider.enabled = true;
                return;
            }

            float verticalVelocity = _playerRigidbody.velocity.y;
            _isPassable = verticalVelocity > 0f;

            if (_isOneWay && _isPassable) _collider.enabled = false;
            else
                _collider.enabled = true;
        }
    }
}