using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : GimmickBase
{
    //�������Ă��邩�ǂ���
    private bool isExplode = false;

    [SerializeField] private float explodeTime = 3f;
    [SerializeField] private GameObject explodeParticle;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed = 180.0f;    // �v���C���[�̉�]���x

    private int sign = 1;

    //����̃A�N�V�������N����
    public override void Action()
    {
        Explode();
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
        // �ړ��������v�Z
        Vector3 moveDirection = Vector3.right * sign;

        // �ړ�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        //�������������Ă�����
        if (isExplode && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<DeathRunPlayer>().FallPlayer();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") sign *= -1;
    }

    //��������
    public void Explode()
    {
        isExplode = true;
        explodeParticle.transform.position = transform.position;
        GameObject o = Instantiate(explodeParticle, this.transform);
        o.transform.position = transform.position;
        o.transform.localScale = transform.localScale;
        Invoke("ExplodeEnd", explodeTime);
    }

    //�������I�������
    public void ExplodeEnd()
    {
        Destroy(gameObject);
    }
}
