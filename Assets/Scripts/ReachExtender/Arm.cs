using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] List<GameObject> magicHandTopList = new List<GameObject>();
    [SerializeField] List<GameObject> magicHandList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < magicHandList.Count; i++)
        {
            if (magicHandList[i].activeSelf)
            {
                transform.position = magicHandTopList[i].transform.position ;
                return;
            }
        }
        transform.position = trans.position;
    }
}
