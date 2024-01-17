using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunSE : MonoBehaviour
{

    [SerializeField] private AudioClip canon;


    private AudioSource audioSource;
    private const float shortTime = 0.1f;
    private const float longTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public float CanonAudio()
    {
        audioSource.PlayOneShot(canon);
        return longTime;
    }

}
