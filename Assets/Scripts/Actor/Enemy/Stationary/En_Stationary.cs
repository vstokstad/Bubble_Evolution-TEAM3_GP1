using System.Collections;
using Actor.Component;
using UnityEngine;
using Weapon;

namespace Actor.Enemies.Stationary {
    [RequireComponent(typeof(CustomGravityController))]
    [RequireComponent(typeof(GroundCheck))]
    [RequireComponent(typeof(MovementController))]
    public class En_Stationary : EnemyBase {
        public enum FacingDirection {
            Left = -1,
            Right = 1
        }

        public FacingDirection facingDirection;

        public Vector3 localSpawnPosition;

        public float spawnRateSeconds = 1.4f;
        public GameObject stationaryProjectile;

        public float projectileForce = 10f;
        public float projectileAngle = 60f;
        internal Animator _anim;

        internal Coroutine _animRoutine;

        protected CustomGravityController _gravityController;
        protected GroundCheck _groundCheck;
        protected MovementController _movementController;

        protected override void Awake(){
            base.Awake();


            _movementController = GetComponent<MovementController>();
            _gravityController = GetComponent<CustomGravityController>();
            _groundCheck = GetComponent<GroundCheck>();
            _anim = GetComponentInChildren<Animator>();

            ChangeState(new ES_Stationary_Initialize(this));
        }

        private void OnDrawGizmosSelected(){
            Vector3 spawnPos = localSpawnPosition;
            spawnPos.x *= (int) facingDirection;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.TransformPoint(spawnPos), 0.25f);
        }

        public override void OnBubbleTrapped(BubbleShot bubble){
            ChangeState(new ES_Stationary_TrappedInBubble(this, bubble));
        }

        internal bool SpawnProjectile(){
            Vector3 spawnPos = localSpawnPosition;
            spawnPos.x *= (int) facingDirection;

            StationaryProjectile sp =
                Instantiate(stationaryProjectile, _transform.TransformPoint(spawnPos), Quaternion.identity)
                    .GetComponent<StationaryProjectile>();
            if (sp) {
                sp.force = projectileForce;
                sp.transform.forward = _transform.right;
                sp.ApplyForce((int) facingDirection, projectileAngle);
                return true;
            }

            return false;
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
    public class StationaryState : EnemyState {
        public En_Stationary stationary;

        public StationaryState(En_Stationary p_Stationary){
            stationary = p_Stationary;
        }
    }

    //Entry state - changes immediately to the "true" default state when
    //executed.
    public class ES_Stationary_Initialize : StationaryState {
        public ES_Stationary_Initialize(En_Stationary p_Stationary) : base(p_Stationary){
            stationary = p_Stationary;
        }

        protected internal override void OnStateEnter(){
            stationary.ChangeState(new ES_Stationary_ThrowingRocks(stationary, stationary.spawnRateSeconds));
        }
    }

    public class ES_Stationary_ThrowingRocks : StationaryState {
        private float _projectileTimer;

        public float maxTimer;

        public ES_Stationary_ThrowingRocks(En_Stationary p_Stationary, float p_maxTimer) : base(p_Stationary){
            stationary = p_Stationary;
            maxTimer = p_maxTimer;
        }

        public float ProjectileTimer {
            get => _projectileTimer;
            private set {
                _projectileTimer = value;

                if (_projectileTimer <= 0) _projectileTimer = 0;
            }
        }

        protected internal override void OnStateEnter(){
            StopAnimRoutine();

            stationary._animRoutine = stationary.StartCoroutine(AnimationCycle());

            ProjectileTimer = maxTimer;
        }

        protected internal override void OnStateUpdate(float deltaTime){
            ProjectileTimer -= deltaTime;
        }

        protected internal override void OnStateExit(EnemyState nextState){
            StopAnimRoutine();
        }

        private IEnumerator AnimationCycle(){
            int phaseIndex = 0;
            stationary._anim.CrossFadeInFixedTime("Idle_Ready", 0.15f, 0);

            while (true) {
                switch (phaseIndex) {
                    case 0:
                        yield return new WaitUntil(() => ProjectileTimer <= 0);
                        break;

                    //Throw auto-transitions to Reload, Reload auto-transitions to Idle_Ready.
                    case 1:
                        stationary._anim.Play("Throw", 0, 0);
                        stationary.SpawnProjectile();
                        yield return new WaitUntil(() =>
                            stationary._anim.GetCurrentAnimatorStateInfo(0).IsName("Reload"));

                        break;


                    case 2:
                        ProjectileTimer = maxTimer;
                        break;
                }

                phaseIndex = (phaseIndex + 1) % 3;
            }
        }

        private void StopAnimRoutine(){
            if (stationary._animRoutine != null)
                stationary.StopCoroutine(stationary._animRoutine);
        }
    }

    public class ES_Stationary_TrappedInBubble : StationaryState {
        private readonly BubbleShot bubble;

        public ES_Stationary_TrappedInBubble(En_Stationary p_Stationary, BubbleShot p_Bubble) : base(p_Stationary){
            stationary = p_Stationary;
            bubble = p_Bubble;
        }

        protected internal override void OnStateEnter(){
            stationary.DisableComponents();
        }

        protected internal override void OnStateUpdate(float deltaTime){
            if (!bubble.isActiveAndEnabled) {
                stationary.EnableComponents();
                stationary.ChangeState(new ES_Stationary_ThrowingRocks(stationary, stationary.spawnRateSeconds));
            }
        }
    }
}