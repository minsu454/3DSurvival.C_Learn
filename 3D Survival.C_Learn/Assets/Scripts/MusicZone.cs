using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource audioSource;
    public float FadeTime;
    public float maxVolume;
    private float targetVolume;


    // Start is called before the first frame update
    void Start()
    {
        targetVolume = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = targetVolume;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mathf.Approximately(audioSource.volume, targetVolume))
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, (maxVolume / FadeTime) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = 0;
        }
    }
}
