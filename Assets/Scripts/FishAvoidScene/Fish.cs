using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    //public NotHitStickPlayer onePlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, -0.01f);
    }

    //�y���M��
    public void Penguin()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }

    //�C���J
    public void Dolphin()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }

    //���Q
    public void Fishes()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }

    //�T��
    public void Shark()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }
}
