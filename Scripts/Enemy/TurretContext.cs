using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyFinder), typeof(CombatController), typeof(Animator))]
public class Turret : MonoBehaviour
{
    private Animator _animator;
    private EnemyFinder _enemyFinder;
    private CombatController _combatController;
    public float Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            OnDirectionChange();
        }
    }
    private float _direction;

    private void Awake()
    {
        _enemyFinder = GetComponent<EnemyFinder>();
        _enemyFinder.OnEnemyFind += OnEnemyFind;
        _enemyFinder.OnEnemyLost += OnEnemyLost;

        _combatController = GetComponent<CombatController>();
        _combatController.DirectionCalculationMode = CombatController.DirectionMode.manual;

        _animator = GetComponent<Animator>();

        TurnDelayCoroutine = StartCoroutine(StartTurnDelayCoroutine());
    }

    private void Update()
    {
        if (enemy != null)
        {
            if (!firingSeriesStarted && firingSeriesDelayExpired)
            {
                FiringSeriesCoroutine = StartCoroutine(StartFiringSeriesCoroutine());
            }
        }
    }
    private void OnDirectionChange()
    {
        _enemyFinder.Direction = Direction;
        _combatController.Direction = Direction;
        TurnDelayCoroutine = StartCoroutine(StartTurnDelayCoroutine());
    }
    
    private GameObject enemy;
    private void OnEnemyFind(GameObject enemy)
    {
        this.enemy = enemy;
        StopCoroutine(TurnDelayCoroutine);
    }

    private void OnEnemyLost(GameObject enemy)
    {
        this.enemy = null;
        TurnDelayCoroutine = StartCoroutine(StartTurnDelayCoroutine());
    }

    public void OnAnimationStart(float direction)
    {
        Direction = direction;    
    }

    public void OnAnimationEnd(float direction)
    {
        Direction = direction;
    }

    [SerializeField] private float turnDelayTimeInS = 2f;
    public Coroutine TurnDelayCoroutine
    {
        get => _turnDelayCoroutine;
        set
        {
            if (_turnDelayCoroutine != null)
                StopCoroutine(_turnDelayCoroutine);

            _turnDelayCoroutine = value;
        }
    }
    private Coroutine _turnDelayCoroutine;
    private IEnumerator StartTurnDelayCoroutine()
    {
        yield return new WaitForSeconds(turnDelayTimeInS);
        _animator.SetTrigger("Continue");
    }

    [SerializeField] private float firingSeriesDelayInS = 2f;
    private bool firingSeriesDelayExpired = true;
    public Coroutine FiringSeriesDelayCoroutine
    {
        get => _firingSeriesDelayCoroutine;
        set
        {
            if (_firingSeriesDelayCoroutine != null)
                StopCoroutine(_firingSeriesDelayCoroutine);

            _firingSeriesDelayCoroutine = value;
        }
    }
    private Coroutine _firingSeriesDelayCoroutine;
    private IEnumerator StartFiringSeriesDelayCoroutine()
    {
        firingSeriesDelayExpired = false;
        yield return new WaitForSeconds(firingSeriesDelayInS);
        firingSeriesDelayExpired = true;
    }

    [SerializeField] private float delayBetweenSeriesShotsInS = 0.2f;
    [SerializeField] private int shotsInSeries = 3;
    public Coroutine FiringSeriesCoroutine
    {
        get => _firingSeriesDelayCoroutine;
        set
        {
            if (_firingSeriesCoroutine != null)
                StopCoroutine(_firingSeriesDelayCoroutine);

            _firingSeriesDelayCoroutine = value;
        }
    }
    private Coroutine _firingSeriesCoroutine;
    private bool firingSeriesStarted;
    private IEnumerator StartFiringSeriesCoroutine()
    {
        firingSeriesStarted = true;
        int curShotIndex = 0;
        float expiredTimeBetweenShots = 0f;
        while (curShotIndex < shotsInSeries)
        {
            expiredTimeBetweenShots += Time.deltaTime;
            if (expiredTimeBetweenShots > delayBetweenSeriesShotsInS)
            {
                _combatController.Fire();
                expiredTimeBetweenShots = 0f;
                curShotIndex++;
            }
            yield return null;
        }
        firingSeriesStarted = false;
        FiringSeriesDelayCoroutine = StartCoroutine(StartFiringSeriesDelayCoroutine());
    }
}