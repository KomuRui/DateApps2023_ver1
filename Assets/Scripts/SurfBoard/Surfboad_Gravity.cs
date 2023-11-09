using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfboad_Gravity : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;

    [SerializeField] private Vector3 localGravity;           //個別の重力
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FixedUpdate();
    }
    private void FixedUpdate()
    {
        SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。FixedUpdateが好ましい。
    }

    private void SetLocalGravity()
    {
        if (rb.useGravity == true)
            rb.AddForce(localGravity, ForceMode.Acceleration);
    }
}
