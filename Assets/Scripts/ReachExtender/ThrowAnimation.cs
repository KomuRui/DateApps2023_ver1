using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw()
    {
        transform.Rotate(45, 0, 0);
        Invoke("TurnBack", 2);
    }

    public void TurnBack()
    {
        transform.Rotate(-45, 0, 0);
    }
}
