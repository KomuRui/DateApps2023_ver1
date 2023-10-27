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
        if (transform.eulerAngles.x > 20 && transform.eulerAngles.x < 335)
            transform.eulerAngles = new Vector3(20, 90, 0);
        if (transform.eulerAngles.x < 340 && transform.eulerAngles.x > 25)
            transform.eulerAngles = new Vector3(340, 90, 0);

        transform.position = pos;
    }
}
