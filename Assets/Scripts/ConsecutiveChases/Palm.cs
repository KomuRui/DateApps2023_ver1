using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palm : MonoBehaviour
{
    //éùÇΩÇÍÇƒÇ¢ÇÈÇ©Ç«Ç§Ç©
    private bool isPickUp = false;
    [SerializeField] private float speed = 1.0f;
    public GameObject throwObj = null;

    private Vector3 moveDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir.normalized * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        
    }

    public void SetDir(Vector3 dir)
    {
        moveDir = dir;
    }

    public void SetisPickUp(bool flag)
    {
        isPickUp = flag;
    }
}
