using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFloor : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ///”ÍˆÍ“à‚É‚¨‚³‚ß‚é
        if (transform.eulerAngles.z > 10 && transform.eulerAngles.z < 335)
            transform.eulerAngles = new Vector3(0, 0, 10);
        if (transform.eulerAngles.z < 350 && transform.eulerAngles.z > 25)
            transform.eulerAngles = new Vector3(0, 0, 350);

        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        transform.position = pos;
    }
}
