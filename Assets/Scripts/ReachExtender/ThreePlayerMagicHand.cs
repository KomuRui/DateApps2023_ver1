using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.XR;

public class ThreePlayerMagicHand : MonoBehaviour
{
    private float speed = 8f;
    public bool bigMax = false;        //マジックハンドが伸びきったかどうか
    public bool isFinish = false;
    [SerializeField] private GameObject myArmParentTop;
    [SerializeField] ThreeArm arm;
    private Vector3 defScale;

    // Start is called before the first frame update
    void Start()
    {
        defScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //一回大きくなりきったら処理をしない
        if (transform.parent.gameObject.GetComponent<ReachExtenderThreePlayer>().GetIsMoving() && !bigMax)
        {
            isFinish = false;

            //伸びる
            Extend();

            MagicHandIsHit magicHandHit = myArmParentTop.GetComponent<MagicHandIsHit>();

            //ステージに当たったら反射の処理
            if (magicHandHit == null || !magicHandHit.isHit) return;

            bigMax = true;
        }
        //一回大きくなりきったら戻る
        if (transform.parent.gameObject.GetComponent<ReachExtenderThreePlayer>().GetIsMoving())
        {
            //戻る処理
            Return();
        }
    }

    //伸びる処理
    public void Extend()
    {
        //アームがプレイヤーをスタンさせないようにする
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
            transform.parent.gameObject.GetComponent<ReachExtenderThreePlayer>().SetIsMoving(false);
        }
    }
}
