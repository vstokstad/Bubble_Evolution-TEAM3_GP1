using System;
using UnityEngine;
using static Tags;

namespace Actor.Component {
    [DefaultExecutionOrder(-1001)]
    public class PlayerBoundaries : MonoBehaviour {
        private Vector3 _boundPosition;
        private Bounds _levelBounds;
        private GameObject _tower;
        internal float playerWidth;
        [NonSerialized] internal float towerWidthX;
        [NonSerialized] internal float towerWidthZ;

        private void Awake(){
            Bounds b = GetComponentInChildren<CapsuleCollider>().bounds;
            _tower = GameObject.FindWithTag(towerTag);
            playerWidth = b.size.x;
            _boundPosition = transform.position;
            _levelBounds = _tower.GetComponent<Collider>().bounds;
            towerWidthX = _levelBounds.max.x + playerWidth * 3f;
            towerWidthZ = _levelBounds.max.z + playerWidth * 3f;
        }

        private void FixedUpdate(){
            _boundPosition = transform.position;
            _boundPosition.x = Mathf.Clamp(_boundPosition.x, towerWidthX * -1f,
                towerWidthX);
            _boundPosition.z = Mathf.Clamp(_boundPosition.z, towerWidthZ * -1f,
                towerWidthZ);
            transform.position = _boundPosition;
        }
    }
}