using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClipArr;
    private AudioSource audioSource;
    private Rigidbody myRb;
    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    private void Start()
    {
        myRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Mathf.Abs(myRb.velocity.y) < 0.1f)
        {
            if (myRb.velocity.magnitude > footstepThreshold)
            {
                if (Time.time - footStepTime > footstepRate)
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClipArr[Random.Range(0, footstepClipArr.Length)]);
                }
            }
        }
    }
}
