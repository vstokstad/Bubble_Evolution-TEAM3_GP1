using UnityEngine;
using static Tags;

namespace Weapon {
    public class BubbleShot : MonoBehaviour {
        [SerializeField] [Tooltip("How long is the bubble active")] [Range(0f, 20f)]
        private float bubbleLifeTime = 10f;

        [SerializeField] [Tooltip("How long until the bubble starts to rise")] [Range(0f, 10f)]
        private float bubbleWaitInPositionTime = 2f;

        [SerializeField] [Tooltip("How fast will the bubble rise upwards")] [Range(0f, 20f)]
        private float bubbleRiseSpeed = 2f;

        private float _aliveTimer;
        private GameObject _capturedEnemy;

        private bool _enemyCaptured;
        private Vector3 _enemyScale;
        private Rigidbody _rigidbody;
        private Vector3 _targetEnemyScale;

        private void Awake(){
            _rigidbody = GetComponent<Rigidbody>();
            _aliveTimer = bubbleLifeTime;
            _rigidbody.isKinematic = true;
        }

        private void Update(){
            _aliveTimer -= Time.deltaTime;

            if (_aliveTimer <= 0f) BubblePop();
        }

        private void FixedUpdate(){
            //  if (_enemyCaptured && _aliveTimer > 0f) {
            //      _capturedEnemy.transform.localScale = Vector3.Lerp(_capturedEnemy.transform.localScale, _targetEnemyScale, _aliveTimer*Time.fixedDeltaTime) ;
            //  }
            if (!(_aliveTimer <= bubbleLifeTime - bubbleWaitInPositionTime)) return;
            BubbleRise();
        }

        private void OnEnable(){
            gameObject.SetActive(true);
            _aliveTimer = bubbleLifeTime;
            _enemyCaptured = false;
        }

        private void OnDisable(){
            if (_enemyCaptured)
                if (_capturedEnemy.activeSelf) {
                    _capturedEnemy.GetComponent<Rigidbody>().isKinematic = false;
                    _capturedEnemy.transform.localScale = _enemyScale;
                    _enemyCaptured = false;
                }

            WeaponPool.Instance.ReturnToPool(WeaponType.Bubble, gameObject);
        }

        private void OnBecameInvisible(){
            BubblePop();
        }


        private void OnTriggerEnter(Collider other){
            if (!other.gameObject.CompareTag(enemyTag)) return;
            // other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.isKinematic = true;
            other.transform.position = transform.position;
            _enemyCaptured = true;
            _capturedEnemy = other.gameObject;
            //  _enemyScale = _capturedEnemy.transform.localScale;
            //  _targetEnemyScale = _enemyScale * 0.5f;
            //  BubbleRise();
        }


        private void OnTriggerStay(Collider other){
            if (!other.gameObject.CompareTag(enemyTag)) return;
            transform.position += transform.up * (bubbleRiseSpeed * Time.fixedDeltaTime);
            other.transform.position = transform.position;
        }

        private void BubbleRise(){
            Transform currentTrans = transform;
            Vector3 pos = currentTrans.position;
            pos += currentTrans.up * (bubbleRiseSpeed * Time.fixedDeltaTime);
            transform.position = pos;
        }

        private void BubblePop(){
            if (_enemyCaptured)
                if (_capturedEnemy.activeSelf) {
                    _capturedEnemy.GetComponent<Rigidbody>().isKinematic = false;
                    _capturedEnemy.transform.localScale = _enemyScale;
                }

            _enemyCaptured = false;
            gameObject.SetActive(false);
        }
    }
}