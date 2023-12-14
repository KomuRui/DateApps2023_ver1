using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonThreePlayer : GimmickBase
{
    [SerializeField] private GameObject shotPos;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 15f;

    private bool isHit = false;

    public override void Action()
    {
        //���𔭎�
        Shot();
    }

    public override void GimmickStart()
    {
    }

    public override void GimmickUpdate()
    {
        if ((Input.GetButtonDown("Abutton2") && isHit) || (Input.GetButtonDown("Abutton3") && isHit) || (Input.GetButtonDown("Abutton4") && isHit))
            this.SetIsInput(true);
    }

    //���𔭎�
    public void Shot()
    {
        //�e��\��
        bullet.gameObject.SetActive(true);

        //�e�𔭎ˈʒu��
        bullet.transform.position = shotPos.transform.position;

        //���˃x�N�g��
        Vector3 force = transform.forward;
        bullet.GetComponent<DeathRunBullet>().SetMoveDirection(force);

        //���x������
        bullet.GetComponent<DeathRunBullet>().SetMoveSpeed(bulletSpeed);


        //force *= bulletSpeed * Time.deltaTime;

        // Rigidbody�ɗ͂������Ĕ���
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.AddForce(force, ForceMode.Impulse);
    }

    void OnTriggerStay(Collider other)
    {
        isHit = true;
    }

    void OnTriggerExit(Collider other)
    {
        isHit = false;
    }
}
