using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DG.Tweening.DOTweenModuleUtils;

public class MagicHand : MonoBehaviour
{
    //[SerializeField] private GameObject onePlayer;
    // Start is called before the first frame update

    private bool bigMax = false;        //マジックハンドが伸びきったかどうか
    [SerializeField] private GameObject nextArmParent;
    [SerializeField] private GameObject nextArmParentTop;
    [SerializeField] private GameObject myArmParentTop;


    void Start()
    {
        //1秒後に伸びきる
        Invoke("BigMax", 5f);

        if (nextArmParentTop != null)
        {
            transform.position = nextArmParentTop.transform.position;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!bigMax)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.001f, transform.localScale.z );


    }

    public void BigMax()
    {
        bigMax = true;
        if(nextArmParent != null)
        {
            Vector3 ray = transform.parent.gameObject.transform.position - myArmParentTop.gameObject.transform.position;
            //Physics.Raycast(Vector3 origin(rayの開始地点), Vector3 direction(rayの向き), RaycastHit hitInfo(当たったオブジェクトの情報を格納), float distance(rayの発射距離), int layerMask(レイヤマスクの設定));
            nextArmParent.SetActive(true);
        }
    }
}
