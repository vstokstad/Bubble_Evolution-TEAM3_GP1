using System;
using UnityEngine;
using Weapon;

namespace Actor.Enemies {
    //Base class for standard enemies in the game.
    public class EnemyBase : MonoBehaviour {
        public enum BinaryDirection {
            Left = -1,
            Right = 1
        }


        public enum LevelSide {
            Front,
            Right,
            Back,
            Left
        }

        [SerializeField] protected internal BinaryDirection _facingDirection;

        public LevelSide levelSide;

        protected Bounds _towerBounds;
        protected Transform _transform;

        public EnemyState enemyState;

        protected virtual void Awake(){
            _transform = transform;
            _towerBounds = MainGameManager.Tower.GetComponent<Collider>().bounds;
            _towerBounds.Expand(5);

            ChangeFacingDirection(_facingDirection);
        }

        protected virtual void Update(){
            enemyState?.OnStateUpdate(Time.deltaTime);

            _transform.position = _towerBounds.ClosestPoint(_transform.position);
        }

        protected virtual void FixedUpdate(){
            enemyState?.OnStateFixedUpdate();
        }

        public virtual void OnBubbleTrapped(BubbleShot bubble){
            throw new NotImplementedException();
        }

        public virtual void EnableComponents(){
            throw new NotImplementedException();
        }

        public virtual void DisableComponents(){
            throw new NotImplementedException();
        }

        protected internal virtual void ChangeFacingDirection(BinaryDirection newDirection){
            _facingDirection = newDirection;
        }

        protected internal virtual void ChangeFacingDirection(int newDirection){
            newDirection = (int) Mathf.Sign(newDirection);
            _facingDirection = (BinaryDirection) newDirection;
        }

        /// <summary>
        ///     Sets transform.right to align with the residing side of the level.
        /// </summary>
        protected void SetRightDirection(){
            Quaternion vectorRot = Quaternion.Euler(0, 90 * (int) levelSide, 0);
            _transform.right = vectorRot * Vector3.right;
        }

        /// <summary>
        ///     Changes the current enemy state.
        /// </summary>
        /// <param name="newState">The new state to change into.</param>
        protected internal void ChangeState(EnemyState newState){
            enemyState?.OnStateExit(newState);

            enemyState = newState;

            enemyState?.OnStateEnter();
        }
    }

    //Base class for enemy states. 
    public class EnemyState {
        //Executes when the state has been changed.
        //The previous state can be proceeded as an argument.
        protected internal virtual void OnStateEnter(){ }

        //Executes once per frame.
        protected internal virtual void OnStateUpdate(float deltaTime){ }

        //Executes once every fixed timestep.
        protected internal virtual void OnStateFixedUpdate(){ }

        //Executes when the state is about to change.
        //The next state can be proceeded as an argument.
        protected internal virtual void OnStateExit(EnemyState nextState){ }
    }
}