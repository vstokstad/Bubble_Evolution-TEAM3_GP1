using System;
using UnityEngine;

namespace Actor.Component {
    [RequireComponent(typeof(Rigidbody), typeof(GroundCheck))]
    public class MovementController : MonoBehaviour {
        [Range(0, 200)] public float acceleration = 35f;
        [Range(0, 50)] public float maxSpeed = 10f;

        [Space(10)] [Range(0, 200)] public float accelerationAir = 12f;

        [Range(0, 50)] public float maxSpeedAir = 5f;

        [Space(10)] [Range(0, 200)] public float deceleration = 35f;

        [Range(0, 200)] public float decelerationAir = 35f;
        private Rigidbody _body;
        private GroundCheck _groundCheck;

        public bool canMoveVertically = false;

        [NonSerialized] public Vector2 MoveVector;

        public bool CanMove { get; private set; } = true;

        private void Awake(){
            _body = GetComponent<Rigidbody>();
            _groundCheck = GetComponent<GroundCheck>();
        }

        private void FixedUpdate(){
            if (!CanMove)
                return;

            if (MoveVector != Vector2.zero)
                Move();
            else
                Brake();
        }

        private void Move(){
            if (!_groundCheck.isWalkable)
                if (MoveVector.x > 0 ? _groundCheck.RightCollision : _groundCheck.LeftCollision)
                    return;

            Vector3 velocity = _groundCheck.WorldToGround(_body.velocity);

            Vector3 desiredVelocity =
                Vector3.right * (MoveVector.x * (_groundCheck.isWalkable ? maxSpeed : maxSpeedAir));
            desiredVelocity.y = canMoveVertically ? MoveVector.y * maxSpeedAir : velocity.y;

            float maxVelocityChange = (_groundCheck.isWalkable ? acceleration : accelerationAir) * Time.deltaTime;

            velocity = Vector3.MoveTowards(velocity, desiredVelocity, maxVelocityChange);

            _body.velocity = _groundCheck.GroundToWorld(velocity);
        }

        private void Brake(){
            Vector3 velocity = _groundCheck.WorldToGround(_body.velocity);

            Vector3 desiredVelocity = Vector3.zero;
            desiredVelocity.y = velocity.y;

            float maxVelocityChange = (_groundCheck.isWalkable ? deceleration : decelerationAir) * Time.deltaTime;

            velocity = Vector3.MoveTowards(velocity, desiredVelocity, maxVelocityChange);

            _body.velocity = _groundCheck.GroundToWorld(velocity);
        }

        public void SetMovement(bool canMove){
            CanMove = canMove;
        }
    }
}