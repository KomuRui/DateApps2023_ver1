using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonMirror : MonoBehaviour
{
    [SerializeField] private GameObject canonMain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(-canonMain.transform.position.x, canonMain.transform.position.y, -canonMain.transform.position.z);
        transform.localEulerAngles = new Vector3(canonMain.transform.localEulerAngles.x, canonMain.transform.localEulerAngles.y + 180, canonMain.transform.localEulerAngles.z);
    }
}
