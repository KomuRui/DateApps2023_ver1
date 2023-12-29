using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProPera : MonoBehaviour
{
    [SerializeField] private float speed;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        angle += speed + Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(angle, this.transform.up);
    }
}
