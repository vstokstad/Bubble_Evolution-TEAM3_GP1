using UnityEngine;

namespace Actor.Component {
    public class PlayerRotationController : MonoBehaviour {
        private Vector2 _moveVector;
        private MovementController _parentMovementController;
        private RotationController _rotationController;
        public Vector3 CharacterFacingDirection { get; private set; }

        private void Awake(){
            _parentMovementController = GetComponentInParent<MovementController>();
            _rotationController = GetComponentInParent<RotationController>();
            CharacterFacingDirection = transform.right * -1;
        }

        private void FixedUpdate(){
            _moveVector = _parentMovementController.MoveVector;
            if (_moveVector.x != 0f) RotatePlayerCharacter();
            CharacterFacingDirection = transform.right * -1f;
        }

        private void RotatePlayerCharacter(){
            switch (_rotationController.cameraState) {
                case CameraPosition.Front:
                    transform.right = _moveVector.normalized * -1f;
                    break;
                case CameraPosition.Back:
                    transform.right = _moveVector.normalized * 1f;
                    break;
                case CameraPosition.Right:
                    transform.forward = _moveVector.normalized * 1f;
                    break;
                case CameraPosition.Left:
                    transform.forward = _moveVector.normalized * -1f;
                    break;
            }
        }
    }
}