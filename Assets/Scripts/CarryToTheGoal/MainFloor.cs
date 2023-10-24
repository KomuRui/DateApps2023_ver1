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
        if (transform.eulerAngles.z > 20 && transform.eulerAngles.z < 335)
            transform.eulerAngles = new Vector3(0, 0, 20);
        if (transform.eulerAngles.z < 340 && transform.eulerAngles.z > 25)
            transform.eulerAngles = new Vector3(0, 0, 340);

        transform.position = pos;
    }
}
