using Assets.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LedderClimbing : MonoBehaviour
{
    [SerializeField] private LayerMask ledderLayer;
    [SerializeField] private Transform ledderCheckerTransform;
    [SerializeField] private Transform ledderBelowCheckerTransform;
    [SerializeField] private float ledderCheckRadius = 0.3f;
    [SerializeField] private float stepsDelayInS = 0.25f;
    private Animator _animator;
    public Vector3 offset = new Vector3(0, 1.25f, 0);
    public event Action<int> OnClimbEnd;

    private GameObject lastFindedPoint;
    
    private bool stepReady = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public bool FindLedderPoint(bool belowSearch)
    {
        Collider2D pointCollider = Physics2D.OverlapCircle
        (
            belowSearch ? ledderBelowCheckerTransform.position : ledderCheckerTransform.position, 
            ledderCheckRadius, 
            ledderLayer
        );

        if (pointCollider != null)
        {
            lastFindedPoint = pointCollider.gameObject;
            return true;
        }

        return false;
    }

    private List<Vector3> pointsPositions = new List<Vector3>(8);
    private int currentLedderPointIndex = 0;
    public Vector3 StartClimbing()
    {
        if (lastFindedPoint == null)
            return Vector3.zero;

        lastFindedPoint.TryGetComponent<LedderPoint>(out var ledderPoint);
        LedderPointMain mainPoint = null;
        if (ledderPoint == null)
            lastFindedPoint.TryGetComponent<LedderPointMain>(out mainPoint);
        else
            mainPoint = ledderPoint.MainPoint;

        if (mainPoint == null)
            return Vector3.zero;

        foreach (var point in mainPoint.Points)
        {
            pointsPositions.Add(point.transform.position);
        }
        if (ledderPoint != null)
        {
            for (int i = 0; i < pointsPositions.Count; i++)
            {
                if (pointsPositions[i] == ledderPoint.transform.position)
                    currentLedderPointIndex = i;
            }
        }

        StartCoroutine(StepsDelayCoroutine());
        return lastFindedPoint.transform.position;
    }

    private void EndClimbing(int direction)
    {
        currentLedderPointIndex = 1;
        OnClimbEnd?.Invoke(direction);
        pointsPositions.Clear();
    }

    public void EndClimb()
    {
        currentLedderPointIndex = 1;
        pointsPositions.Clear();
    }
    public Vector3 Next()
    {
        if (pointsPositions.Count == 0) return Vector3.zero;
        if (!stepReady)
            return Vector3.zero;

        try
        {
            Vector3 position = pointsPositions[currentLedderPointIndex + 1];
            Debug.Log($"Next: {position}");
            currentLedderPointIndex++;
            StartCoroutine(StepsDelayCoroutine());
            _animator.SetTrigger("Next");
            return position;
        }
        catch (ArgumentOutOfRangeException)
        {
            EndClimbing(1);
            return Vector3.zero;
        }
    }

    public Vector3 Previous()
    {
        if (pointsPositions.Count == 0)
            return Vector3.zero;

        if (!stepReady)
            return Vector3.zero;

        try
        {
            Vector3 position = pointsPositions[currentLedderPointIndex - 1];
            Debug.Log($"Previous: {position}");
            currentLedderPointIndex--;
            StartCoroutine(StepsDelayCoroutine());
            _animator.SetTrigger("Previous");
            return position;
        }
        catch (ArgumentOutOfRangeException)
        {
            EndClimbing(-1);
            return Vector3.zero;
        }
    }

    private IEnumerator StepsDelayCoroutine()
    {
        stepReady = false;
        yield return new WaitForSeconds(stepsDelayInS);
        stepReady = true;
    }
}
