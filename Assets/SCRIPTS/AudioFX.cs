using UnityEngine;

public class AudioFX : MonoBehaviour
{
    public static AudioFX I;

    [Header("Clips")]
    public AudioClip plant;
    public AudioClip water;
    public AudioClip harvest;
    public AudioClip fail;   // ← AJOUTE ÇA

    AudioSource src;

    void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;

        src = GetComponent<AudioSource>();
        if (src == null)
            src = gameObject.AddComponent<AudioSource>();

        src.playOnAwake = false;
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        src.PlayOneShot(clip, volume);
    }
}