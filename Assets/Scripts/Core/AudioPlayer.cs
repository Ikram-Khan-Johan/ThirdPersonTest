using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour, ISoundPlayer
{
   [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

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
    public void PlaySound()
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
