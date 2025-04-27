using UnityEngine;

public class InvokeGulseSFX : MonoBehaviour
{
    private AudioManager audioManager;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip TrafficAmbiance;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        sfxSource.mute = false;
        sfxSource.PlayOneShot(TrafficAmbiance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.textScroolClown);
            sfxSource.mute = true;
        }
    }
}
