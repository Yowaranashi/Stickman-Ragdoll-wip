using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _parallaxValue;
    private float _length, _startPosition;
    private void Start()
    {
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update()
    {
        float temp = _camera.transform.position.x * (1 - _parallaxValue);
        float distance = _camera.transform.position.x * _parallaxValue;

        transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

        if (temp > _startPosition + _length) _startPosition += _length;
        else if(temp < _startPosition - _length) _startPosition -= _length; 
    }
}
