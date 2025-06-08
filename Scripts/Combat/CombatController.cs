using UnityEngine;

[RequireComponent(typeof(SoundsController))]
public class CombatController : MonoBehaviour
{
    private SoundsController _soundsController;
    [SerializeField]
    private Projectile _projectile;
    [SerializeField]
    private Transform _projectileStartPoint;

    public DirectionMode DirectionCalculationMode = DirectionMode.transform;
    public float Direction;
    public enum DirectionMode
    {
        transform,
        manual,
    }
    private void Awake()
    {
        _soundsController = GetComponent<SoundsController>();
    }
    public void Fire()
    {
        GameObject projectileInstance = Instantiate(_projectile.gameObject, _projectileStartPoint.position, Quaternion.identity);
        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();
        if (DirectionCalculationMode == DirectionMode.transform)
        {
            Direction = Mathf.Sign(transform.localScale.x);
        }
        projectileScript.Init((int)Mathf.Sign(Direction));
        projectileScript.OnObjectHit += OnHit;
        //_soundsController.PlaySound(projectileScript.ProjectileData.AudioClip);
        Destroy(projectileInstance, projectileScript.ProjectileData.LifeTime);
    }

    private void OnHit(GameObject hittedObject, Projectile projectile)
    {
        Debug.Log($"{this}: Object hitted");
        Destroy(projectile.gameObject);
    }
}
