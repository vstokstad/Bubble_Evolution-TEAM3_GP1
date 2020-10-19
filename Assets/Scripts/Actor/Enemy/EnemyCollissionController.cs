using Actor.Enemies;
using UnityEngine;
using Weapon;
using static Tags;

namespace Actor.Enemy {
    public class EnemyCollissionController : MonoBehaviour {
        [SerializeField] [Range(0f, 10000f)] private float knockBackPower = 1000f;
        [SerializeField] [Range(0f, 100f)] private float knockBackRange = 10f;
        [SerializeField] [Range(0f, 1f)] private float upwardsPower = 0.5f;
        private BubbleShot _bubbleShot;
        private bool _canGetTrapped;
        private Collider _collider;
        private EnemyBase _enemyBase;
        private bool _isKnockerReady;
        private bool _trappedInBubble;

        private void Start(){
            _isKnockerReady = true;

            if (TryGetComponent(out EnemyBase component)) {
                _enemyBase = component;
                _canGetTrapped = true;
            }
            else {
                _canGetTrapped = false;
            }
        }

        private void FixedUpdate(){
            if (!_trappedInBubble) return;
            if (!_bubbleShot.isActiveAndEnabled) _trappedInBubble = false;
        }


        private void OnCollisionEnter(Collision other){
            if (other.collider.CompareTag(playerTag)) {
                if (!_isKnockerReady) return;
                other.rigidbody.AddExplosionForce(knockBackPower, transform.position, knockBackRange, upwardsPower);
            }
            else if (other.collider.CompareTag(bubbleTag)) {
                if (!_canGetTrapped) return;
                if (_trappedInBubble) return;
                _isKnockerReady = false;
                _bubbleShot = other.gameObject.GetComponent<BubbleShot>();
                _enemyBase.OnBubbleTrapped(_bubbleShot);
            }
        }

        private void OnCollisionExit(Collision other){
            if (!other.collider.CompareTag(bubbleTag)) return;
            _isKnockerReady = true;
        }

        private void OnTriggerEnter(Collider other){
            if (!other.CompareTag(bubbleTag)) return;
            if (!_canGetTrapped) return;
            if (_trappedInBubble) return;
            _isKnockerReady = false;
            _bubbleShot = other.gameObject.GetComponent<BubbleShot>();
            _enemyBase.OnBubbleTrapped(_bubbleShot);
        }

        private void OnTriggerExit(Collider other){
            if (!other.CompareTag(bubbleTag)) return;
            _isKnockerReady = true;
        }
    }
}