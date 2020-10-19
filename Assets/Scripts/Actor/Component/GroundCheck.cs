using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Component {
    [DefaultExecutionOrder(-1010)]
    public class GroundCheck : MonoBehaviour {
        [Range(0, 90)] public float maxAngleWalk = 45f;
        [Range(0, 90)] public float maxAngleJump = 90f;
        [Range(0, 2)] public float coyoteTime = 0.2f;
        public bool isWalkable;
        public bool isJumpable;
        public Vector3 groundNormal;

        private readonly List<ContactPoint> _contactPoints = new List<ContactPoint>();

        private Coroutine _coyoteTimeRoutine;
        private DashController _dashController;
        private Matrix4x4 _groundMatrix;
        private Matrix4x4 _inverseGroundMatrix;

        private bool _isCoyoteTimeRoutineRunning;
        private JumpController _jumpController;
        private MovementController _movementController;

        private int _points;

        private Transform _transform;
        private readonly ContactPoint[] intermediaryContactPointsArray = new ContactPoint[5];

        public bool RightCollision { get; private set; }
        public bool LeftCollision { get; private set; }
        public bool UpCollision { get; private set; }
        public bool DownCollision { get; private set; }

        private void Awake(){
            _transform = transform;
            _movementController = GetComponent<MovementController>();
            _jumpController = GetComponent<JumpController>();
            _dashController = GetComponent<DashController>();
        }

        private void FixedUpdate(){
            bool newIsJumpableState = false;
            isWalkable = false;
            groundNormal = Vector3.up;

            RightCollision = false;
            LeftCollision = false;
            UpCollision = false;
            DownCollision = false;

            float lowestDot = 1;
            Vector3 actorDirection = _movementController.MoveVector.x == 0
                ? Vector3.down
                : Vector3.right * Mathf.Sign(_movementController.MoveVector.x);

            for (int i = 0; i < _points; i++) {
                Vector3 pointNormal = _contactPoints[i].normal;
                float angle = Vector3.Angle(Vector3.up, pointNormal);

                if (angle <= maxAngleWalk) {
                    isWalkable = true;

                    float dot = Vector3.Dot(pointNormal, actorDirection);
                    if (dot < lowestDot) {
                        lowestDot = dot;
                        groundNormal = pointNormal;
                    }
                }

                if (angle <= maxAngleJump)
                    newIsJumpableState = true;

                UpdateCollisionDirections(pointNormal);
            }

            if (newIsJumpableState != isJumpable && _jumpController) UpdateJumpState(newIsJumpableState);

            _points = 0;
            _contactPoints.Clear();

            if ((Vector3) _groundMatrix.GetColumn(1) != groundNormal) BuildMatrix();
        }

        private void OnCollisionStay(Collision other){
            int pointsReturned = other.GetContacts(intermediaryContactPointsArray);
            for (int i = 0; i < pointsReturned; i++) _contactPoints.Add(intermediaryContactPointsArray[i]);

            _points += pointsReturned;
        }

#if DEBUG
        private void OnDrawGizmosSelected(){
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            for (int i = 0; i < _points; i++) Gizmos.DrawWireSphere(_contactPoints[i].point, 0.05f);

            Vector3 position = _transform.position;
            Gizmos.DrawLine(position, position + (Vector3) _groundMatrix.GetColumn(0));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, position + (Vector3) _groundMatrix.GetColumn(1));
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(position, position + (Vector3) _groundMatrix.GetColumn(2));
        }
#endif

        private void UpdateCollisionDirections(Vector3 collisionNormal){
            Vector3 normal = WorldToGround(collisionNormal);

            if (Math.Abs(normal.x - 1) < 0.9f)
                LeftCollision = true;
            else if (Math.Abs(normal.x + 1) < 0.9f)
                RightCollision = true;
            else if (Math.Abs(normal.y + 1) < 0.9f)
                UpCollision = true;
            else if (Math.Abs(normal.y - 1) < 0.9f)
                DownCollision = true;
        }

        private void UpdateJumpState(bool newJumpState){
            if (!newJumpState) {
                if (!_isCoyoteTimeRoutineRunning && !_jumpController.IsJumping && !_dashController.isDashing)
                    _coyoteTimeRoutine = StartCoroutine(CoyoteTimeRoutine());
                else if (_jumpController.IsJumping || _dashController.isDashing)
                    isJumpable = false;
            }

            else {
                isJumpable = true;
                if (_isCoyoteTimeRoutineRunning)
                    StopCoroutine(_coyoteTimeRoutine);
            }
        }

        private IEnumerator CoyoteTimeRoutine(){
            _isCoyoteTimeRoutineRunning = true;

            yield return new WaitForSeconds(coyoteTime);

            isJumpable = false;

            _isCoyoteTimeRoutineRunning = false;
        }

        public void CancelCoyoteTime(){
            if (_isCoyoteTimeRoutineRunning)
                StopCoroutine(_coyoteTimeRoutine);

            _isCoyoteTimeRoutineRunning = false;

            isJumpable = false;
        }

        public void BuildMatrix(){
            Vector3 up = groundNormal;
            Vector3 forward = Vector3.Cross(_transform.right, up);
            Vector3 right = Vector3.Cross(up, forward);

            _groundMatrix.SetColumn(0, right);
            _groundMatrix.SetColumn(1, up);
            _groundMatrix.SetColumn(2, forward);
            _groundMatrix[3, 3] = 1;

            _inverseGroundMatrix = _groundMatrix.inverse;
        }

        public Vector3 GroundToWorld(Vector3 direction){
            return _groundMatrix.MultiplyVector(direction);
        }

        public Vector3 WorldToGround(Vector3 direction){
            return _inverseGroundMatrix.MultiplyVector(direction);
        }
    }
}