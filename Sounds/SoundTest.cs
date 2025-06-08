using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class SoundTest : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;
    private AudioSource _audioSource;
    private Animator _animator;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("Run");
    }

    private void FootStep()
    {
        _audioSource.PlayOneShot(_clip);
    }
}