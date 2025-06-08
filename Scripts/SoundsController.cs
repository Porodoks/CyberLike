using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsController : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip soundClip, float volume = 1f, float pitch = 1f)
    {
        //audioSource.pitch = pitch;
        audioSource.PlayOneShot(soundClip/*, volume*/);
    }
    public void PlaySound(AudioClip soundClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(soundClip, position, volume);
    }
}

