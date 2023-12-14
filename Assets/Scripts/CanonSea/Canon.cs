using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;          // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;    // �v���C���[�̉�]���x
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = false;     // �c�̓��͋����邩
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();  // rigidbody���擾
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish())
        {
            //����
            Move();
        }
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

        //������ԂɕύX
        //ChangeStateTo(SlimeAnimationState.Walk);

        // �J�����̌�������Ƀv���C���[���ړ�
        //Vector3 forwardDirection = mainCameraTransform.forward;
        //Vector3 rightDirection = mainCameraTransform.right;
        //forwardDirection.y = 0f; // Y��������0�ɂ��邱�ƂŐ��������ɐ���

        // �ړ��������v�Z
        //Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // �ړ�
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        //rb.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        if(horizontalInput > 0)
        {
            rb.AddForce(transform.right * 10.0f);
        }
        if(verticalInput < 0)
        {
            rb.AddForce(-transform.right * 10.0f);
        }

        //rb.velocity = -moveDirection;

        //transform.position.x = Math.Clamp(transform.position.x, -3.5f, 3.5f);

        //Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        //transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "RightWall")
        {
            Debug.Log("��������!");
            transform.Rotate(90, 0, 0);
        }
        else
        {
            //Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();

            //if(otherRb != null)
            //{
            //    Vector3 knockbackForce = -otherRb.velocity * 50.0f;

            //    otherRb.AddForce(knockbackForce);
            //}
        }
        //isBound = true;
    }
}
