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
    private int magicHandNum = 0;


    void Start()
    {
        //1�b��ɐL�т���
        //Invoke("BigMax", 5f);

        if (nextArmParentTop != null)
        {
            transform.position = nextArmParentTop.transform.position;
        }

        //�Ⴊ�������������
        RaycastHit rayHit;

        //���C���΂�����
        Vector3 ray = myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.position;

        //���C���΂�����
        Ray ray2 = new Ray(transform.position, myArmParentTop.gameObject.transform.position - transform.parent.gameObject.transform.position);

        //���C���΂�
        Debug.DrawRay(transform.position, ray * 9999999, Color.red, 30);
        if (UnityEngine.Physics.Raycast(ray2, out rayHit, 9999))
        {
            Vector3 refrect = Vector3.Reflect(ray, rayHit.normal);

            Debug.DrawRay(rayHit.point, refrect * 9999999, Color.red, 30);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!bigMax)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.001f, transform.localScale.z );
        //rayHit.normal;

        if(myArmParentTop.GetComponent<MagicHandIsHit>().isHit)
        {
            BigMax();
        }
    }

    public void BigMax()
    {
        bigMax = true;
        //if(nextArmParent != null)
        //{
        //    Vector3 ray = transform.parent.gameObject.transform.position - myArmParentTop.gameObject.transform.position;
        //    //Debug.DrawRay(transform.position, ray, )
        //    //Physics.Raycast(Vector3 origin(ray�̊J�n�n�_), Vector3 direction(ray�̌���), RaycastHit hitInfo(���������I�u�W�F�N�g�̏����i�[), float distance(ray�̔��ˋ���), int layerMask(���C���}�X�N�̐ݒ�));
        //    nextArmParent.SetActive(true);
        //}
    }
}
