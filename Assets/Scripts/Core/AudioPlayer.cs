using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour, ISoundPlayer
{
   [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        if (audioSource == null)
        {
          gameObject.AddComponent<AudioSource>();
        }
       
    }
    void Start()
    {
        
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the GameObject.");
        }

    }
    public void PlaySound(AudioClip audioClip)
    {
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            Debug.LogError("AudioSource component is missing on the GameObject.");
            return;
        }
        audioSource.PlayOneShot(audioClip);

    }

    public void StopSound()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            Debug.LogError("AudioSource component is missing on the GameObject.");
            return;
        }
        audioSource.Stop();
    }

}
