using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sdada : MonoBehaviour
{
    public GameObject par;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Abutton1"))
        {
            par.SetActive(true);
            par.GetComponent<ParticleSystem>().Play();
        }
    }
}
