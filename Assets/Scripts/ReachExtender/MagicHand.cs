using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DG.Tweening.DOTweenModuleUtils;

public class MagicHand : MonoBehaviour
{
    //[SerializeField] private GameObject onePlayer;
    // Start is called before the first frame update

    private bool bigMax = false;        //�}�W�b�N�n���h���L�т��������ǂ���
    [SerializeField] private GameObject nextArmParent;
    [SerializeField] private GameObject nextArmParentTop;
    [SerializeField] private GameObject myArmParentTop;


    void Start()
    {
        //1�b��ɐL�т���
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
            //Physics.Raycast(Vector3 origin(ray�̊J�n�n�_), Vector3 direction(ray�̌���), RaycastHit hitInfo(���������I�u�W�F�N�g�̏����i�[), float distance(ray�̔��ˋ���), int layerMask(���C���}�X�N�̐ݒ�));
            nextArmParent.SetActive(true);
        }
    }
}
