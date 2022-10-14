using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carAudioController : MonoBehaviour
{
    [SerializeField] private float audioPitch;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.pitch = audioPitch;
    }
}
