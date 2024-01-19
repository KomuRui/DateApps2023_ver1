using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SETable : MonoBehaviour
{

    [SerializeField] private AudioClip shortFlute;
    [SerializeField] private AudioClip longFlute;
    [SerializeField] private AudioClip miss;
    [SerializeField] private AudioClip up;


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

    //’Z‚¢“J–Â‚ç‚·(‰¹‚ÌŠÔ‚ğ•Ô‚·)
    public float PlayShortFlute() {
        audioSource.PlayOneShot(shortFlute);
        return shortTime;
    }

    //’·‚¢“J–Â‚ç‚·(‰¹‚ÌŠÔ‚ğ•Ô‚·)
    public float PlayLongFlute() {
        audioSource.PlayOneShot(longFlute);
        return longTime;
    }

    //’E—
    public float MissAudio()
    {
        audioSource.PlayOneShot(miss);
        return longTime;
    }


    //Šøã‚°‚é‚Æ‚«SE
    public float UpAudio()
    {
        audioSource.PlayOneShot(up);
        return longTime;
    }
}
