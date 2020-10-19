using System;
using System.Collections;
using UnityEngine;

namespace Actor.Component
{
    [RequireComponent(typeof(Rigidbody))]
    public class DashController : MonoBehaviour
    {
        [Range(0, 90)]
        public float dashSlopeLimit = 45;

        [Range(0, 5), Tooltip("The period of time the player can't dash after finishing one.")]
        public float dashCooldown = 0.5f;

        [Range(0, 5), Tooltip("The period of time the player can't move left or right after dashing.")]
        public float inputDisablePeriod = 0.5f;

        public bool isDashing = false;

        public AnimationCurve dashCurve;

        private int _collisionLayerIndex = 8;
        private float _timeInDash;

        public Action<bool> onDashReady;

        private bool _dashReady;
        public bool DashReady
        {
            get => _dashReady;
            set
            {
                _dashReady = value;
                onDashReady?.Invoke(value);
            }
        }

        private Coroutine _dashRoutine;
        private Coroutine _cooldownRoutine;

        private ContactPoint[] _contactPoints = new ContactPoint[8];


        private Rigidbody _body;
        private Transform _transform;
        private Camera _mainCamera;
        private GroundCheck _groundCheck;
        private MovementController _movementController;
        
        [NonSerialized]
        public Vector3 Direction;



        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _transform = transform;
            _mainCamera = Camera.main;
            _groundCheck = GetComponent<GroundCheck>();
            _movementController = GetComponent<MovementController>();
        }
        
        public void Dash(Vector3 mousePosition)
        {
            if(isDashing || !DashReady)
                return;
            
            Vector2 screenPosition = _mainCamera.WorldToScreenPoint(_transform.position);
            
            Vector3 direction = ((Vector2) mousePosition - screenPosition).normalized;
            direction = _transform.TransformDirection(direction);

            DashReady = false;
            
            _groundCheck.CancelCoyoteTime();
            
            _dashRoutine = StartCoroutine(Dashing(direction));
        }

        void StopDash()
        {
            if (isDashing)
            {
                isDashing = false;
                StopCoroutine(_dashRoutine);
                StartCoroutine(DashCooldown(dashCooldown - _timeInDash));
                _timeInDash = 0;
            }
        }

        IEnumerator Dashing(Vector3 dashDirection)
        {
            isDashing = true;

            Direction = dashDirection;

            float maxTime = dashCurve.keys[dashCurve.length - 1].time;
            while (_timeInDash < maxTime)
            {
                _body.velocity = Direction * dashCurve.Evaluate(_timeInDash);
                
                yield return new WaitForFixedUpdate();
                
                _timeInDash += Time.fixedDeltaTime;
            }
            
            isDashing = false;
            StartCoroutine(DashCooldown(dashCooldown - maxTime));
            StartCoroutine(DashDisableMovePeriod(inputDisablePeriod));
            _timeInDash = 0;
        }

        private IEnumerator DashCooldown(float cooldown)
        {
            bool hasLanded = _groundCheck.isJumpable;
            float startTime = Time.time;

            while (Time.time < startTime + cooldown)
            {
                yield return null;
                if (_groundCheck.isJumpable)
                    hasLanded = true;
            }

            while (!hasLanded)
            {
                yield return null;
                hasLanded = _groundCheck.isJumpable;
            }

            DashReady = true;
        }

        private IEnumerator DashDisableMovePeriod(float period)
        {
            float startTime = Time.time;

            _movementController.SetMovement(false);

            yield return new WaitForSeconds(period);

            _movementController.SetMovement(true);
        }

        private void OnCollisionStay(Collision collision)
        {
            if(!isDashing)
                return;

            int contactCount = collision.GetContacts(_contactPoints);
            //print(contactCount);
            for (int i = 0; i < contactCount; i++)
            {
                if (_contactPoints[i].otherCollider.gameObject.layer == _collisionLayerIndex)
                {
                    float angle = Vector3.Angle(Vector3.up, _contactPoints[i].normal);
                    
                    if (angle >= dashSlopeLimit)
                    {
                        StopDash();
                    }
                }
            }
        }
    }
}

