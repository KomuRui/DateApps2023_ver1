using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    //[SerializeField] List<GameObject> magicHandTopList = new List<GameObject>();
    //[SerializeField] List<GameObject> magicHandList = new List<GameObject>();
    [SerializeField] ArmHierarchy armHierarchy;
    [SerializeField] ThrowAnimation rootBone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < armHierarchy.magicHandList.Count; i++)
        {
            if (armHierarchy.magicHandList[i].activeSelf)
            {
                transform.position = armHierarchy.magicHandTopList[i].transform.position ;
                transform.rotation = armHierarchy.magicHandTopList[i].transform.rotation;
                return;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ThreePlayer")
        {
            //ƒvƒŒƒCƒ„[‚É“–‚½‚Á‚½Žž‚ÉŒÄ‚ÔŠÖ”
            armHierarchy.HitPlayer();
        }
        else
        {
        }
    }
}
