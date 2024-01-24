using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] Arm arm;
    [SerializeField] private ThrowAnimation bone;

    private GameObject preArm; // 前に伸びてたアーム
    private int magicHandNum = 0;
    private int maxRefrect = 1;

    private Vector3 defScale;
    private float speed = 12f;

    public bool isFinish = false;


    void Start()
    {
        defScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //一回大きくなりきったら処理をしない
        if (transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<ReachExtenderOnePlayer>().GetIsMoving() && !bigMax)
        {
            isFinish = false;

            //伸びる
            Extend();

            MagicHandIsHit magicHandHit = myArmParentTop.GetComponent<MagicHandIsHit>();

            //ステージに当たったら反射の処理
            if (magicHandHit == null || !magicHandHit.isHit) return;

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
            Ray ray2 = new Ray(transform.position, myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.position);

            //レイを飛ばす
            if (UnityEngine.Physics.Raycast(ray2, out rayHit, 9999))
            {
                //反射ベクトルを作成
                Vector3 refrect = Vector3.Reflect(ray2.direction, rayHit.normal);

                //位置を設定
                nextArmParent.transform.position = rayHit.point;

                //yを反転
                Vector3 newRefrect = new Vector3(refrect.x, refrect.y, refrect.z);

                Debug.DrawRay(transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.position, ray2.direction * 9999, Color.red, 10, false);
                Debug.DrawRay(rayHit.point, newRefrect * 9999, Color.green, 10, false);

                //向きを設定
                nextArmParent.transform.LookAt(newRefrect + nextArmParent.transform.position);
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
        //アームがプレイヤーを捕まえるようにする
        arm.SetIsActive(true);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + speed * Time.deltaTime);
    }

    //戻る処理
    public void Return()
    {
        //アームがプレイヤーをスタンさせないようにする
        arm.SetIsActive(false);


        myArmParentTop.GetComponent<MagicHandIsHit>().isHit = false;

        bigMax = true;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - speed * 1.5f * Time.deltaTime);

        if (defScale.z >= transform.localScale.z)
        {
            bigMax = false;
            transform.localScale = defScale;

            //つぎのアームがあるなら消さない
            if (nextArmParent != null)
            {
                transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<ReachExtenderOnePlayer>().SetIsMoving(false);
                isFinish = true;
                if (bone != null)  bone.Throw();
            }
            else
            {
                this.gameObject.SetActive(false);
                isFinish = true;
            }
        }
    }
}