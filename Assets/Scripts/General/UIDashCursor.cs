using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Actor.Player;

public class UIDashCursor : MonoBehaviour
{
    private Image _cursor;
    private Transform _transform;

    private Camera _camera;

    private PlayerInput _playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        _cursor = GetComponent<Image>();
        _transform = transform;
        _camera = Camera.main;

        _playerInput = MainGameManager.Player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 w2sp = _camera.WorldToScreenPoint(MainGameManager.Player.position);
        w2sp.z = 0;

        Vector3 mouseP = _playerInput.GetMouseScreenPosition();
        mouseP.z = 0;

        _transform.position = w2sp;

        //Quaternion.LookRotation((mouseP - _transform.position).normalized);

        _transform.up = Quaternion.FromToRotation(_transform.up, (mouseP - _transform.position).normalized) * _transform.up;
    }
}
