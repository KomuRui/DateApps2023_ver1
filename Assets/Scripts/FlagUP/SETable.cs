using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SETable : MonoBehaviour
{

    [SerializeField] private AudioClip shortFlute;
    [SerializeField] private AudioClip longFlute;
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

    //�Z���J�炷(���̎��Ԃ�Ԃ�)
    public float PlayShortFlute() {
        audioSource.PlayOneShot(shortFlute);
        return shortTime;
    }

    //�����J�炷(���̎��Ԃ�Ԃ�)
    public float PlayLongFlute() {
        audioSource.PlayOneShot(longFlute);
        return longTime;
    }
}
