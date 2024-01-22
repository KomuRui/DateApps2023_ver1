using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    public GameObject dino;


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(dino.transform);
    }
}
