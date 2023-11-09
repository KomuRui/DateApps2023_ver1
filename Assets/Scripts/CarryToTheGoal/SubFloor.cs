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
        //transform.eulerAngles += new Vector3(0, 0, 0.001f);
        this.transform.localEulerAngles = mainFloor.transform.localEulerAngles;

        ///”ÍˆÍ“à‚É‚¨‚³‚ß‚é
        if (transform.eulerAngles.z > 10 && transform.eulerAngles.z < 335)
            transform.eulerAngles = new Vector3(0, 0, 10);
        if (transform.eulerAngles.z < 350 && transform.eulerAngles.z > 25)
            transform.eulerAngles = new Vector3(0, 0, 350);
    }
}
