using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectReplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<ParticleSystem>().isStopped)
            this.GetComponent<ParticleSystem>().Play();
    }
}
