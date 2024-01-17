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

            //�v���C���[���X�^��������
            hitPlayer.SetStan(true);

            //���������璵�˕Ԃ�悤�ɂ���
            magicHand.bigMax = true;
        }

        if (other.tag == "OnePlayer" && isActive)
        {
            //���������璵�˕Ԃ�悤�ɂ���
            magicHand.bigMax = true;
        }
    }
}
