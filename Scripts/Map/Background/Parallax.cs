using System;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField][Range(0, 1)] private float _horizontalParallaxStrength;
    [SerializeField][Range(0, 1)] private float _verticalParallaxStrength;

    private Vector2 _startingPos;
    private float _length;

    private Transform _selfTransform;
    private Vector2 _previousCameraPosition;
    void Start()
    {
        if (_cameraTransform == null)
            throw new NullReferenceException();

        _startingPos = transform.position;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;

        _selfTransform = transform;
        _previousCameraPosition = _cameraTransform.position;
    }

    void Update()
    {
        Vector3 position = _cameraTransform.position;
        float temp = position.x * (1 - _horizontalParallaxStrength);
        Vector2 distance = new Vector2(_cameraTransform.position.x - _previousCameraPosition.x, _cameraTransform.position.y - _previousCameraPosition.y);

        Vector3 newPosition = new Vector3(distance.x * _horizontalParallaxStrength, distance.y * _verticalParallaxStrength);

        _previousCameraPosition = _cameraTransform.position;
        _selfTransform.position += newPosition;

        //if (temp > _startingPos.x + (_length / 2))
        //{
        //    newPosition = new Vector2(_length, 0);
        //    _selfTransform.position += newPosition;
        //}
        //else if (temp < _startingPos.x - (_length / 2))
        //{
        //    newPosition = new Vector3(_length, 0);
        //    _selfTransform.position -= newPosition;
        //}
    }
}
