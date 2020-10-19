using System;
using UnityEngine;

namespace Actor.Component {
    [RequireComponent(typeof(PlayerBoundaries), typeof(Rigidbody))]
    public class RotationController : MonoBehaviour {
        [SerializeField] [Tooltip("Camera Field of View")] [Range(16f, 90f)]
        private float cameraFOV = 40f;

        [SerializeField] [Tooltip("Camera Distance from center")]
        private float cameraDistance = 75f;

        [SerializeField] [Tooltip("Position of the camera")]
        public CameraPosition cameraState = CameraPosition.Front;

        [SerializeField] private float rotationSpeed = 1.5f;

        [Tooltip("Player vertical position + this value gets you vertical camera position ")] [SerializeField]
        private float cameraVerticalOffset = 10f;

        [SerializeField] [Tooltip("Camera rotation forward angle")]
        private float camPitch = -3f;

        private Transform _cameraTransform;
        private Vector3 _camPosBack;
        private Vector3 _camPosFront;
        private Vector3 _camPosition;
        private Vector3 _camPosLeft;
        private Vector3 _camPosRight;
        private Quaternion _camRotation;
        private Quaternion _camRotBack;
        private Quaternion _camRotFront;
        private Quaternion _camRotLeft;
        private Quaternion _camRotRight;
        private GroundCheck _groundCheck;
        private Camera _mainCamera;

        private PlayerBoundaries _playerBoundaries;
        // private DashController _dashController;

        private Rigidbody _rigidbody;
        private AudioSource _rotateSound;
        private float _rotationTriggerPoint;

        private void Awake(){
            _groundCheck = GetComponent<GroundCheck>();
            _mainCamera = FindObjectOfType<Camera>();
            _playerBoundaries = GetComponent<PlayerBoundaries>();
            // _dashController = GetComponent<DashController>();
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _rotateSound = _mainCamera.GetComponent<AudioSource>();


            _cameraTransform = _mainCamera.transform;
            float verticalOffset = transform.position.y + cameraVerticalOffset;
            _rotationTriggerPoint = _playerBoundaries.towerWidthX - _playerBoundaries.playerWidth;


            _camPosFront = new Vector3(0f, verticalOffset, -cameraDistance);
            _camPosRight = new Vector3(cameraDistance, verticalOffset, 0f);
            _camPosBack = new Vector3(0f, verticalOffset, cameraDistance);
            _camPosLeft = new Vector3(-cameraDistance, verticalOffset, 0f);
            _camRotFront = Quaternion.Euler(camPitch, 0f, 0f);
            _camRotRight = Quaternion.Euler(camPitch, -90f, 0f);
            _camRotBack = Quaternion.Euler(camPitch, -180f, 0f);
            _camRotLeft = Quaternion.Euler(camPitch, -270f, 0f);
            _camPosition = _camPosFront;
            _camRotation = _camRotFront;
        }

        private void Start(){
            _mainCamera.fieldOfView = cameraFOV;
            RotateCamera();
        }


        private void FixedUpdate(){
           
            Vector3 playerPosition = transform.position;
            float verticalOffset = cameraVerticalOffset + playerPosition.y;
            _camPosition.y = Mathf.SmoothStep(_camPosition.y, verticalOffset, rotationSpeed*4 * Time.fixedDeltaTime);
           
            Vector3 adjustedPlayerPosition = playerPosition;
             adjustedPlayerPosition.x = 0f;
             adjustedPlayerPosition.z = 0f;
          
            _cameraTransform.LookAt(adjustedPlayerPosition);
            camPitch = Mathf.Clamp(camPitch, -3f, 3f);
            _mainCamera.fieldOfView =
                Mathf.SmoothStep(_mainCamera.fieldOfView, Mathf.Clamp(cameraFOV + (_rigidbody.velocity.magnitude/2), cameraFOV,60f), Time.fixedDeltaTime);

            _camPosFront.Set(0f, verticalOffset, -cameraDistance);
            _camPosRight.Set(cameraDistance, verticalOffset, 0f);
            _camPosBack.Set(0f, verticalOffset, cameraDistance);
            _camPosLeft.Set(-cameraDistance, verticalOffset, 0f);
            _camRotFront = Quaternion.Euler(camPitch, 0f, 0f);
            _camRotRight = Quaternion.Euler(camPitch, -90f, 0f);
            _camRotBack = Quaternion.Euler(camPitch, -180f, 0f);
            _camRotLeft = Quaternion.Euler(camPitch, -270f, 0f);

            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _camPosition,
                rotationSpeed * Time.fixedDeltaTime);

            _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _camRotation,
                rotationSpeed * Time.fixedDeltaTime);
            
            transform.position = CheckForRotation(playerPosition);
        }

        private Vector3 CheckForRotation(Vector3 playerPosition){
            switch (cameraState) {
                case CameraPosition.Front:
                    if (playerPosition.x > _rotationTriggerPoint) {
                        RotateCameraRight();
                        playerPosition.x = _rotationTriggerPoint - 1;
                    }
                    else if (playerPosition.x < -_rotationTriggerPoint) {
                        RotateCameraLeft();
                        playerPosition.x = -_rotationTriggerPoint + 1;
                    }

                    break;

                case CameraPosition.Right:
                    if (playerPosition.z > _rotationTriggerPoint) {
                        RotateCameraRight();
                        playerPosition.z = _rotationTriggerPoint - 1;
                    }
                    else if (playerPosition.z < -_rotationTriggerPoint) {
                        RotateCameraLeft();
                        playerPosition.z = -_rotationTriggerPoint + 1;
                    }

                    break;

                case CameraPosition.Back:
                    if (playerPosition.x < -_rotationTriggerPoint) {
                        RotateCameraRight();
                        playerPosition.x = -_rotationTriggerPoint + 1;
                    }
                    else if (playerPosition.x > _rotationTriggerPoint) {
                        RotateCameraLeft();
                        playerPosition.x = _rotationTriggerPoint - 1;
                    }

                    break;

                case CameraPosition.Left:
                    if (playerPosition.z < -_rotationTriggerPoint) {
                        RotateCameraRight();
                        playerPosition.z = -_rotationTriggerPoint + 1;
                    }
                    else if (playerPosition.z > _rotationTriggerPoint) {
                        RotateCameraLeft();
                        playerPosition.z = _rotationTriggerPoint - 1;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return playerPosition;
        }

        private void RotateCamera(){
            if (!_rotateSound.isPlaying) _rotateSound.Play();
            switch (cameraState) {
                case CameraPosition.Front:
                    _camPosition = _camPosFront;
                    _camRotation = _camRotFront;
                    //_dashController.direction = Quaternion.FromToRotation(Quaternion.LookRotation(transform.right), Qu)
                    transform.right = _camRotation * Vector3.right;
                    _rigidbody.constraints =
                        RigidbodyConstraints.FreezeRotation + (int) RigidbodyConstraints.FreezePositionZ;
                    _groundCheck.BuildMatrix();
                    break;
                case CameraPosition.Right:
                    _camPosition = _camPosRight;
                    _camRotation = _camRotRight;
                    transform.right = _camRotation * Vector3.right;
                    _rigidbody.constraints =
                        RigidbodyConstraints.FreezeRotation + (int) RigidbodyConstraints.FreezePositionX;
                    _groundCheck.BuildMatrix();
                    break;
                case CameraPosition.Back:
                    _camPosition = _camPosBack;
                    _camRotation = _camRotBack;
                    transform.right = _camRotation * Vector3.right;
                    _rigidbody.constraints =
                        RigidbodyConstraints.FreezeRotation + (int) RigidbodyConstraints.FreezePositionZ;
                    _groundCheck.BuildMatrix();
                    break;
                case CameraPosition.Left:
                    _camPosition = _camPosLeft;
                    _camRotation = _camRotLeft;
                    transform.right = _camRotation * Vector3.right; //_cameraTransform.right;
                    _rigidbody.constraints =
                        RigidbodyConstraints.FreezeRotation + (int) RigidbodyConstraints.FreezePositionX;
                    _groundCheck.BuildMatrix();
                    break;
                default:
                    _camPosition = _camPosFront;
                    _camRotation = _camRotFront;
                    transform.right = _camRotation * Vector3.right; //_cameraTransform.right;
                    _rigidbody.constraints =
                        RigidbodyConstraints.FreezeRotation + (int) RigidbodyConstraints.FreezePositionZ;
                    _groundCheck.BuildMatrix();
                    break;
            }
        }

        public void RotateCameraRight(){
            if (cameraState != CameraPosition.Left) {
                cameraState += 1;

                RotateCamera();
            }
            else if (cameraState == CameraPosition.Left) {
                cameraState = CameraPosition.Front;
                RotateCamera();
            }
        }

        public void RotateCameraLeft(){
            if (cameraState != CameraPosition.Front) {
                cameraState -= 1;
                RotateCamera();
            }
            else if (cameraState == CameraPosition.Front) {
                cameraState = CameraPosition.Left;
                RotateCamera();
            }
        }
    }
}