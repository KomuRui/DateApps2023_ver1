using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DG.Tweening.DOTweenModuleUtils;

public class MagicHand : MonoBehaviour
{
    //[SerializeField] private GameObject onePlayer;
    // Start is called before the first frame update

    public bool bigMax = false;        //マジックハンドが伸びきったかどうか
    private bool isReverse = false;       //マジックハンドが戻っているかどうか

    [SerializeField] private GameObject nextArmParent;
    [SerializeField] private GameObject nextArmParentTop;
    [SerializeField] private GameObject myArmParentTop;
    [SerializeField] ArmHierarchy armHierarchy;

    private GameObject preArm; // 前に伸びてたアーム
    private int magicHandNum = 0;
    private int maxRefrect = 1; 

    private Vector3 defScale;
    private float speed = 0.008f;

    public bool isFinish = false;


    void Start()
    {
        defScale = transform.localScale;
    }
    
    // Update is called once per frame
    void Update()
    {
        //一回大きくなりきったら処理をしない
        if (transform.parent.gameObject.transform.parent.gameObject.GetComponent<ReachExtenderOnePlayer>().GetIsMoving() && !bigMax)
        {
            isFinish = false;

            //伸びる
            Extend();

            //ステージに当たったら反射の処理
            if (myArmParentTop.GetComponent<MagicHandIsHit>() == null || !myArmParentTop.GetComponent<MagicHandIsHit>().isHit) return;

            bigMax = true;

            //反射回数が一定に達していなかったら
            if (magicHandNum < maxRefrect)
                Refrect();
        }
    }

    public void Refrect()
    {
        if (nextArmParent != null)
        {
            //レイが当たった時情報
            RaycastHit rayHit;

            //レイを飛ばす方向
            Vector3 ray = myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.position;

            //レイを飛ばす方向
            Ray ray2 = new Ray(transform.position, myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.position);

            //レイを飛ばす
            if (UnityEngine.Physics.Raycast(ray2, out rayHit, 9999))
            {
                //反射ベクトルを作成
                Vector3 refrect = Vector3.Reflect(ray, rayHit.normal);

                //位置を設定
                nextArmParent.transform.position = rayHit.point;

                //向きを設定
                nextArmParent.transform.LookAt(refrect);
            }

            //反射回数をカウント
            nextArmParent.GetComponent<MagicHand>().SetMagicHandNum(magicHandNum + 1);
            nextArmParent.SetActive(true);
        }
    }

    public void SetMagicHandNum(int num)
    {
        magicHandNum = num;
    }

    //伸びる処理
    public void Extend()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + speed);
    }

    //戻る処理
    public void Return()
    {
        myArmParentTop.GetComponent<MagicHandIsHit>().isHit = false;

        bigMax = true;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - speed * 1.5f);

        if (defScale.z >= transform.localScale.z)
        {
            bigMax = false;
            transform.localScale = defScale;

            //つぎのアームがあるなら消さない
            if (nextArmParent != null)
            {
                transform.parent.gameObject.GetComponent<ReachExtenderOnePlayer>().SetIsMoving(false);
                isFinish = true;
            }
            else
            {
                this.gameObject.SetActive(false);
                isFinish = true;
            }
        }
    }
}
