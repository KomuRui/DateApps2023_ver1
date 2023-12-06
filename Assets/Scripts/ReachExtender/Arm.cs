using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] Transform trans;

    //
    [SerializeField] List<GameObject> magicHandTopList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in magicHandTopList)
        {
            if(item.activeSelf)
            {
                transform.position = trans.position;
                return;
            }
            
        }
        
    }
}
