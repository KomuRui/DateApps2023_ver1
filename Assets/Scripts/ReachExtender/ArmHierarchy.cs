using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ArmHierarchy : MonoBehaviour
{
    public List<GameObject> magicHandTopList = new List<GameObject>();
    public List<GameObject> magicHandList = new List<GameObject>();

    private bool isReverse = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(magicHandList.First().GetComponent<MagicHand>().bigMax)
        {
            Invoke("SetReverseTrue", 3.0f);
        }

        if(isReverse)
            Return();
    }

    public void Return()
    {
        for (int i = 0; i < magicHandList.Count; i++)
        {
            if (magicHandList[i].activeSelf)
            {
                magicHandList[i].GetComponent<MagicHand>().Return();
                return;
            }
        }
    }

    public void SetReverseTrue()
    { 
        isReverse = true; 
    }

}
