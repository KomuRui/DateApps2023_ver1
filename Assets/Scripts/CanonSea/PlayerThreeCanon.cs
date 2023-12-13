using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerThreeCanon : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;          // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;    // �v���C���[�̉�]���x
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩

    private Rigidbody rBody;
    private Transform mainCameraTransform; // ���C���J������Transform

    // Start is called before the first frame update
    void Start()
    {
        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //���W�b�g�{�f�B�擾
        rBody = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.nowMiniGameManager.IsFinish())
            //����
            Move();

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.7f, 4.7f), transform.position.y, Mathf.Clamp(transform.position.z, -1.0f, 1.0f));//1.7f


    }

    //�ړ�
    private void Move()
    {
        // ���͂��擾�p
        float horizontalInput = 0;
        float verticalInput = 0;

        // ���͂��擾
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + this.GetComponent<PlayerNum>().playerNum);

        //���͂��Ȃ��̂Ȃ�
        if (horizontalInput == 0 && verticalInput == 0)
        {
            //�ʏ��ԂɕύX
            //ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        // �J�����̌�������Ƀv���C���[���ړ�
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y��������0�ɂ��邱�ƂŐ��������ɐ���

        // �ړ��������v�Z
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // �ړ�
        rBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerHammer"/* && isSuper == false*/)
        {
            Debug.Log("�Ԃꂽ!");

            

            // �~�j�Q�[���Ɏ��񂾂��Ƃ�`����
            //GameManager.nowMiniGameManager.PlayerDead(this.transform.GetChild(1).GetComponent<PlayerNum>().playerNum);
            //GameManager.nowMiniGameManager.PlayerFinish(this.transform.GetChild(1).GetComponent<PlayerNum>().playerNum);

            //�I�u�W�F�N�g���폜
            //Destroy(this.gameObject);
        }
    }

    
}