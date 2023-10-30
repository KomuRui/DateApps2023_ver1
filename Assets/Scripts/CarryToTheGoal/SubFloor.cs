using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubFloor : MonoBehaviour
{
    [SerializeField] private MainFloor mainFloor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = mainFloor.transform.rotation;
    }
}
