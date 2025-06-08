using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyFinder : MonoBehaviour
{
    public float Direction;
    [SerializeField] private float _rayLength;
    [SerializeField] private Transform _rayStartPosition;
    [SerializeField] private LayerMask _enemyLayer;
    private BoxCollider2D _selfCollider;

    private List<GameObject> _findedEnemies = new List<GameObject>(2);
    private GameObject _lastFindedEnemy;

    public event Action<GameObject> OnEnemyFind;
    public event Action<GameObject> OnEnemyLost;

    private void FixedUpdate()
    {
        if (Direction != 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(_rayStartPosition.position, Vector2.right * Direction, _rayLength, _enemyLayer);
            Debug.DrawRay(_rayStartPosition.position, Vector2.right * Direction * _rayLength, Color.red);
            if (hit)
            {
                if (!_findedEnemies.Contains(hit.transform.gameObject))
                {
                    OnEnemyFind?.Invoke(hit.transform.gameObject);
                    _findedEnemies.Add(hit.transform.gameObject);
                    _lastFindedEnemy = hit.transform.gameObject;
                }
            }
            else if (_lastFindedEnemy != null)
            {
                OnEnemyLost?.Invoke(_lastFindedEnemy);
                _findedEnemies.Remove(_lastFindedEnemy.transform.gameObject);
                _lastFindedEnemy = null;
            }
        }
    }
}
