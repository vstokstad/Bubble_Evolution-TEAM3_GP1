using Actor.Component;
using Actor.Enemies;
using Algorithms;
using UnityEngine;
using Weapon;

namespace Actor.Enemy.Patroller {
    [RequireComponent(typeof(Rigidbody), typeof(MovementController), typeof(CustomGravityController))]
    public class En_Patroller : EnemyBase {
        [Tooltip("How far the enemy is allowed to be from the turning point to actually turn.")]
        public float distanceOffset = 1;

        [Tooltip("Which waypoint the patroller tracks.")]
        public WaypointSystem waypointSystem;

        [Tooltip("How fast the enemy turns towards a waypoint. 0 = Won't turn, 1 = Turn instantly.")]
        public float turningSpeed = 0.25f;

        protected CustomGravityController _gravityController;
        protected GroundCheck _groundCheck;

        //End of inspector-editable parameters

        protected MovementController _movementController;

        internal Transform localNode;
        internal int nodeIndex;
        internal bool reverseOrder;

        protected override void Awake(){
            base.Awake();
            ChangeState(new ES_Patrol_Initialize(this));

            _movementController = GetComponent<MovementController>();
            _gravityController = GetComponent<CustomGravityController>();
            _groundCheck = GetComponent<GroundCheck>();
        }

        protected internal void MoveX(float moveValue){
            _movementController.MoveVector.x = moveValue;
            _groundCheck.BuildMatrix();
        }

        protected internal void MoveXY(Vector2 moveValue){
            _movementController.MoveVector.x = moveValue.x;
            _movementController.MoveVector.y = moveValue.y;
            _groundCheck.BuildMatrix();
        }

        public override void OnBubbleTrapped(BubbleShot bubble){
            ChangeState(new ES_Patrol_TrappedInBubble(this, bubble));
        }

        public override void EnableComponents(){
            _movementController.enabled = true;
            _gravityController.enabled = true;
            _groundCheck.enabled = true;
        }

        public override void DisableComponents(){
            _movementController.enabled = false;
            _gravityController.enabled = false;
            _groundCheck.enabled = false;
        }
    }

    //Base for patroller states. Contains data/structures all states
    //share in common.
    public class PatrollerState : EnemyState {
        public En_Patroller patroller;

        public PatrollerState(En_Patroller p_Patroller){
            patroller = p_Patroller;
        }
    }

    //Entry state - changes immediately to the "true" default state when
    //executed.
    public class ES_Patrol_Initialize : PatrollerState {
        public ES_Patrol_Initialize(En_Patroller p_Patroller) : base(p_Patroller){
            patroller = p_Patroller;
        }

        protected internal override void OnStateEnter(){
            patroller.ChangeState(new ES_Patrol_Patrolling(patroller));
        }
    }

    //Patrolling state
    public class ES_Patrol_Patrolling : PatrollerState {
        public ES_Patrol_Patrolling(En_Patroller p_Patroller) : base(p_Patroller){
            patroller = p_Patroller;
        }

        protected internal override void OnStateEnter(){
            patroller.waypointSystem.GetNearestPoint(patroller.transform.position,
                out patroller.localNode, out patroller.nodeIndex);
        }

        //Checks when to turn, and will 
        protected internal override void OnStateFixedUpdate(){
            if (patroller.nodeIndex != -1) {
                WaypointTraverse();

                Vector3 direction =
                    new Vector3(patroller.localNode.position.x, 0, patroller.localNode.position.z)
                    - new Vector3(patroller.transform.position.x, 0, patroller.transform.position.z);

                float moveDirectionY =
                    (patroller.localNode.position - patroller.transform.position).normalized.y;

                patroller.transform.right = Vector3.Slerp(patroller.transform.right, direction, patroller.turningSpeed);
                patroller.MoveXY(new Vector2(1, moveDirectionY).normalized);
            }
            else {
                patroller.MoveX(0);
            }
        }

        private void WaypointTraverse(){
            if (Vector3.Distance(patroller.transform.position, patroller.localNode.position) <
                patroller.distanceOffset) {
                if (patroller.nodeIndex == 0 || patroller.nodeIndex == patroller.waypointSystem.nodes.Length - 1)
                    patroller.reverseOrder = patroller.nodeIndex == patroller.waypointSystem.nodes.Length - 1;

                TargetNextNode();
            }
        }

        private void TargetNextNode(){
            patroller.nodeIndex += patroller.reverseOrder ? -1 : 1;
            patroller.localNode = patroller.waypointSystem.nodes[patroller.nodeIndex];
        }
    }

    public class ES_Patrol_TrappedInBubble : PatrollerState {
        private readonly BubbleShot bubble;

        public ES_Patrol_TrappedInBubble(En_Patroller p_Patroller, BubbleShot p_Bubble) : base(p_Patroller){
            patroller = p_Patroller;
            bubble = p_Bubble;
        }

        protected internal override void OnStateEnter(){
            patroller.MoveX(0);
            patroller.DisableComponents();
        }

        protected internal override void OnStateUpdate(float deltaTime){
            if (!bubble) {
                patroller.EnableComponents();
                patroller.ChangeState(new ES_Patrol_Patrolling(patroller));
            }
        }
    }
}