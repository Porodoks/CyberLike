using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public float FlySpeed => _flySpeed;
    [SerializeField] private float _flySpeed;

    public float CheckRadius => _checkRadius;
    [SerializeField] private float _checkRadius;

    public float LifeTime => _lifeTime;
    [SerializeField] private float _lifeTime;

    public AudioClip AudioClip => _audioClip;
    [SerializeField] private AudioClip _audioClip;
}
