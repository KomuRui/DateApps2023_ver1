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
    private bool isReverseHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(magicHandList.First().GetComponent<MagicHand>().bigMax && !isReverseHit)
        {
            Invoke("SetReverseTrue", 3.0f);
            isReverseHit = true;
        }

        if(isReverse)
            Return();
    }

    public void Return()
    {
        bool allFinish = true;
        for (int i = 0; i < magicHandList.Count; i++)
        {
            //マジックハンドを戻す処理
            if (magicHandList[i].activeSelf)
            {
                magicHandList[i].GetComponent<MagicHand>().Return();
                break;
            }
        }

        for (int i = 0; i < magicHandList.Count; i++)
        {
            //すべてのアームの処理が終わっているか調べる
            if (!magicHandList[i].GetComponent<MagicHand>().isFinish)
            {
                allFinish = false;
            }
        }

        if (allFinish)
        {
            isReverse = false;
            isReverseHit = false;
        }
    }

    public void SetReverseTrue()
    { 
        isReverse = true;
        for (int i = 0; i < magicHandList.Count; i++)
        {
            magicHandList[i].GetComponent<MagicHand>().bigMax = false;
        }
    }

}
