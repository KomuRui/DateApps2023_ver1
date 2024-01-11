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
        //�v���C���[��߂܂��Ă�����
        if (hitPlayer != null)
        {
            hitPlayer.transform.position = transform.position;
        }

        //�A�[�����ړ����鏈��
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
        if (other.tag == "ThreePlayer" && isActive)
        {
            //�v���C���[�ɓ����������ɌĂԊ֐�
            armHierarchy.HitPlayer();
            hitPlayer = other.gameObject;
        }
        else
        {
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
