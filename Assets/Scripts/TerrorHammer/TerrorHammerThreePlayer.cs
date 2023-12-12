using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TerrorHammerThreePlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;          // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;    // �v���C���[�̉�]���x
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] public float startPosX;     // �X�^�[�g
    [SerializeField] public float checkPosX;     // �`�F�b�N
    [SerializeField] private float nowPosX;     // �v���C���[
    [SerializeField] private int point;     // �v���C���[
    [SerializeField] public TextMeshProUGUI pointText;       //�_���e�L�X�g
    [SerializeField] private GameObject HammerOb;  // �n���}�[
    
    private Vector3 initializeRotate;
    private Vector3 AttackRotate;
    private bool isAttack;
    private bool isSuper;
    

    private Rigidbody rBody;
    private Transform mainCameraTransform; // ���C���J������Transform

    // Start is called before the first frame update
    void Start()
    {
        nowPosX = startPosX;
        point = 0;
        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //���W�b�g�{�f�B�擾
        rBody = this.GetComponent<Rigidbody>();

        //����
        initializeRotate = new Vector3(90, 0, 180);
        AttackRotate = new Vector3(0, 0, 180);
        isAttack = true;
        isSuper = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.nowMiniGameManager.IsFinish() && point < 1 && this.transform.localScale.y > 0.5f)
            //����
            Move();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.7f, 4.7f),transform.position.y, Mathf.Clamp(transform.position.z, -1.0f, 1.0f));//1.7f

        //�`�F�b�N
        if(nowPosX == startPosX && this.transform.position.x > checkPosX)
        {
            nowPosX = checkPosX;
        }

        if (nowPosX == checkPosX && this.transform.position.x < startPosX)
        {
            nowPosX = startPosX;
            point++;
            pointText.SetText(point.ToString());
            Debug.Log(point);
        }

        //�U��
        if (Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) && isAttack)
        {
            isAttack = false;
            HammerOb.transform.DORotate(AttackRotate, 0.5f).SetEase(Ease.InBack);

            //1.5�b��ɂ�����
            Invoke("HammerUp", 0.5f);
            Invoke("HammerAttack", 2.0f);
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

    public void HammerUp()
    {
        HammerOb.transform.DORotate(new Vector3 (initializeRotate.x, initializeRotate.y,initializeRotate.z), 0.5f).SetEase(Ease.InQuad);
    }

    public void HammerAttack()
    {
        isAttack = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerHammer"/* && isSuper == false*/)
        {
            Debug.Log("�Ԃꂽ!");

            this.transform.localScale = new Vector3(1, 0.3f, 1);
            isSuper = true;
            Invoke("MovePlayer", 2f);
            Invoke("WeakPlayer", 3f);

            // �~�j�Q�[���Ɏ��񂾂��Ƃ�`����
            //GameManager.nowMiniGameManager.PlayerDead(this.transform.GetChild(1).GetComponent<PlayerNum>().playerNum);
            //GameManager.nowMiniGameManager.PlayerFinish(this.transform.GetChild(1).GetComponent<PlayerNum>().playerNum);

            //�I�u�W�F�N�g���폜
            //Destroy(this.gameObject);
        }
    }

    void WeakPlayer()
    {
        isSuper = false;
    }

    void MovePlayer()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}