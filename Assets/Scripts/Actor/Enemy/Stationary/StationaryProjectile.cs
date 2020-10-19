using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StationaryProjectile : MonoBehaviour
{
    public float existenceTime = 3f;
    public float force = 10f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, existenceTime);
    }

    public void ApplyForce(int signedDirection, float angle)
    {
        Vector3 forceDirection = Quaternion.Euler(0, 0, angle) * transform.forward;
        _rb.AddForce(forceDirection * force, ForceMode.Impulse);
    }
}
