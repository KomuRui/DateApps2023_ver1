using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfboad_Gravity : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;

    [SerializeField] private Vector3 localGravity;           //�ʂ̏d��
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
        SetLocalGravity(); //�d�͂�AddForce�ł����郁�\�b�h���ĂԁBFixedUpdate���D�܂����B
    }

    private void SetLocalGravity()
    {
        if (rb.useGravity == true)
            rb.AddForce(localGravity, ForceMode.Acceleration);
    }
}
