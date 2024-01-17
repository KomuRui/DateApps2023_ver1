using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    //[SerializeField] List<GameObject> magicHandTopList = new List<GameObject>();
    //[SerializeField] List<GameObject> magicHandList = new List<GameObject>();
    [SerializeField] ArmHierarchy armHierarchy;
    [SerializeField] ThrowAnimation rootBone;
    public GameObject hitPlayer;

    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーを捕まえていたら
        if (hitPlayer != null)
        {
            hitPlayer.transform.position = transform.position;
        }

        //アームを移動する処理
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

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "ThreePlayer" && isActive)
        {
            //プレイヤーに当たった時に呼ぶ関数
            armHierarchy.HitPlayer();
            //hitPlayer = other.gameObject;

            Vector3 vector3 = other.transform.position - transform.position;
            ReachExtenderThreePlayer hitPlayer = other.GetComponent<ReachExtenderThreePlayer>();
            hitPlayer.SetIsDead(true);
            hitPlayer.SetMove(vector3.normalized);
        }
    }

    public void SetIsActive(bool a)
    { 
        isActive = a;
    }

    public GameObject GetHitPlayer()
    {
        return hitPlayer;
    }

}
