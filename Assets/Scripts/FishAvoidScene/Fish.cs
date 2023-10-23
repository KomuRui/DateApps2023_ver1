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

    //ペンギン
    public void Penguin()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }

    //イルカ
    public void Dolphin()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }

    //魚群
    public void Fishes()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }

    //サメ
    public void Shark()
    {
        transform.position += new Vector3(0, 0, -0.1f);
    }
}
