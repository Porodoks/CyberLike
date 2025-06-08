using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileData ProjectileData => _projectileData;
    [SerializeField] private ProjectileData _projectileData;

    public LayerMask EnemyLayer => _enemyLayer;
    private LayerMask _enemyLayer;

    private Transform _selfTransform;
    private int _flyDirection = 0;

    public event Action<GameObject, Projectile> OnObjectHit;
    private HashSet<GameObject> _hittedObjects = new HashSet<GameObject>();
    public void Init(int direction)
    {
        _selfTransform = transform;
        _flyDirection = direction;
    }
    private void FixedUpdate()
    {
        if (_flyDirection == 0)
        {
            throw new ArgumentException($"{this}: Направление полёта равняется 0");
        }
        Vector3 translation = new Vector3(ProjectileData.FlySpeed * Time.fixedDeltaTime * _flyDirection, 0);
        transform.Translate(translation);
        CheckHit();
    }
    private void CheckHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(_selfTransform.position, ProjectileData.CheckRadius, EnemyLayer);
        if (hit != null)
        {
            _hittedObjects.TryGetValue(hit.gameObject, out GameObject hitObject);
            if (hitObject == null)
            {
                _hittedObjects.Add(hit.gameObject);
                OnObjectHit(hitObject, this);
            }
        }
    }
    private void OnDestroy()
    {
        _hittedObjects.Clear();
    }
}
