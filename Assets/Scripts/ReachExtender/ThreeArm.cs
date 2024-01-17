using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeArm : MonoBehaviour
{
    [SerializeField] private bool isActive = false;
    [SerializeField] GameObject magicHandTop;
    [SerializeField] ThreePlayerMagicHand magicHand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = magicHandTop.transform.position;
        transform.rotation = magicHandTop.transform.rotation;
    }

    public void SetIsActive(bool a)
    {
        isActive = a;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "ThreePlayer" && isActive)
        {
            Vector3 vector3 = other.transform.position - transform.position;
            ReachExtenderThreePlayer hitPlayer = other.GetComponent<ReachExtenderThreePlayer>();

            //プレイヤーをスタンさせる
            hitPlayer.SetStan(true);

            //当たったら跳ね返るようにする
            magicHand.bigMax = true;
        }

        if (other.tag == "OnePlayer" && isActive)
        {
            //当たったら跳ね返るようにする
            magicHand.bigMax = true;
        }
    }
}
