using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NotHitStickPlayer : MonoBehaviour
{
    //�A�j���[�V�����ɕK�v
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;
    public Animator animator;
    public int damType;
    private Material faceMaterial;

    [SerializeField] private float moveSpeed = 5.0f;          // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;    // �v���C���[�̉�]���x
    [SerializeField] private float gravitySpeed = 0.05f;      // �d�͑��x
    [SerializeField] private float beforeInput = 0;           // �O��̓��͒l
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] private bool isAnimIdle = true;          // �ʏ펞�̃A�j���[�V���������邩
    [SerializeField] private bool isAnimWalk = true;          // �����A�j���[�V���������邩 
    [SerializeField] private bool isAnimJump = true;          // �W�����v�A�j���[�V���������邩
    [SerializeField] private bool isAnimAttack = true;        // �U���A�j���[�V���������邩
    [SerializeField] private bool isAnimDamage = true;        // �_���[�W�A�j���[�V���������邩
    [SerializeField] public GameObject[] stage;              // ���̃Q�[���I�u�W�F�N�g
    [SerializeField] private float jumpPower;                 // �W�����v��
    [SerializeField] public int nowStageNum;                 // ����Ă��鏰�̔ԍ�
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    [SerializeField] private float stunTime = 2;              //�X�^������

    //���W�b�g�{�f�B
    private Rigidbody rb;

    //�W�����v���Ă��邩
    private bool isJump = false;
    private bool isJump2 = false;

    //�W�����v�ł����Ԃ��ǂ���
    private bool canJump = true;

    private bool isInvokeJump = false;

    private float jumpSpeedRatio = 1;

    //���G���ǂ���
    private bool isInvincible = false;

    //�X�^����Ԃ��ǂ���
    public bool isStun = false;

    // ���C���J������Transform
    private Transform mainCameraTransform;

    private Tweener tweener;

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //���W�b�g�{�f�B�擾
        rb = GetComponent<Rigidbody>();

        //�v���C���[�ԍ��ݒ�
        playerNum = this.GetComponent<PlayerNum>().playerNum;
    }

    //��̃e�N�X�`���ݒ�
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //�J�n���Ă��Ȃ����I����Ă���̂Ȃ�
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish() || isStun) return;

        //��ԍX�V
        StateUpdata();

        //����
        Move();

        //�W�����v
        Jump();
    }

    //�ړ�
    private void Move()
    {
        //�W�����v���Ă���Ȃ炱�̐揈�����Ȃ�
        if (isJump) return;

        // ���͂��擾�p
        float horizontalInput = 0;
        float verticalInput = 0;

        // ���͂��擾
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum);
        if (isVerticalInput) verticalInput = Input.GetAxis("L_Stick_V" + this.GetComponent<PlayerNum>().playerNum);

        //���͂��Ȃ��̂Ȃ�
        if (horizontalInput == 0 && verticalInput == 0)
        {
            //�ʏ��ԂɕύX
            ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        //������ԂɕύX
        ChangeStateTo(SlimeAnimationState.Walk);

        // �J�����̌�������Ƀv���C���[���ړ�
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y��������0�ɂ��邱�ƂŐ��������ɐ���

        // �ړ��������v�Z
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        //�W�����v���Ȃ�x��
        if (isJump2)
        {
            jumpSpeedRatio = 0.5f;
        }
        else
        {
            jumpSpeedRatio = 1;
        }

        // �ړ�
        transform.position += moveDirection * moveSpeed * Time.deltaTime * jumpSpeedRatio;

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //�W�����v
    private void Jump()
    {
        //�W�����v���Ă���Ȃ��Ƀ��C���΂�
        if (isJump)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.up); // Ray�𐶐�
            RaycastHit hit2;
            Ray ray2 = new Ray(new Vector3(transform.position.x + 0.30f, transform.position.y, transform.position.z), Vector3.up); // Ray�𐶐�
            RaycastHit hit3;
            Ray ray3 = new Ray(new Vector3(transform.position.x - 0.30f, transform.position.y, transform.position.z), Vector3.up); // Ray�𐶐�

            if (Physics.Raycast(ray, out hit, 10000) || Physics.Raycast(ray2, out hit2, 10000) || Physics.Raycast(ray3, out hit3, 10000))
            {
                //���ɖ߂�
                StartCoroutine(Drop(0.3f));
            }
           

            return;
        }

        if (isJump2) return;

        //�ʏ�
        //�W�����v�o�����ԂȂ�
        if (Input.GetButtonDown("Abutton" + playerNum) && canJump)
        {
            //�ʏ��ԂɕύX
            ChangeStateTo(SlimeAnimationState.Idle);

            //��ɗ͂�������
            rb.AddForce(Vector3.up * jumpPower);
            isJump2 = true;

            canJump = false;

            return;
        }

        int beforeStage = nowStageNum;
        float nowInput = Input.GetAxis("L_Stick_V" + playerNum);

        //�����W�����v(�ʂ̑����)
        if (nowInput <= -0.8f && beforeInput > -0.8f)
        {
            nowStageNum--;
            nowStageNum = Math.Max(nowStageNum, 0);
            if (beforeStage == nowStageNum) return;
            tweener = transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
            tweener.Play();
            ChangeStateTo(SlimeAnimationState.Idle);
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
            isJump2 = true;
        }
        else if (nowInput >= 0.8f && beforeInput < 0.8f)
        {
            nowStageNum++;
            nowStageNum = Math.Min(nowStageNum, stage.Length - 1);
            if (beforeStage == nowStageNum) return;
            tweener = transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
            tweener.Play();
            ChangeStateTo(SlimeAnimationState.Idle);
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
            isJump2 = true;
        }

        beforeInput = nowInput;
    }

    //��ԍX�V
    private void StateUpdata()
    {
        switch (currentState)
        {
            case SlimeAnimationState.Idle:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !isAnimIdle) return;

                currentState = SlimeAnimationState.Idle;
                animator.SetFloat("Speed", 0);
                SetFace(faces.Idleface);
                break;

            case SlimeAnimationState.Walk:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || !isAnimWalk) return;

                currentState = SlimeAnimationState.Walk;
                animator.SetFloat("Speed", 1.0f);
                SetFace(faces.WalkFace);
                break;

            case SlimeAnimationState.Jump:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || !isAnimJump) return;

                SetFace(faces.jumpFace);
                animator.SetTrigger("Jump");
                break;

            case SlimeAnimationState.Attack:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || !isAnimAttack) return;
                SetFace(faces.attackFace);
                animator.SetTrigger("Attack");
                break;

            case SlimeAnimationState.Damage:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")
                 || !isAnimDamage) return;

                animator.SetTrigger("Damage");
                animator.SetInteger("DamageType", damType);
                SetFace(faces.damageFace);
                break;
        }
    }

    //��ԕύX
    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (this == null) return;
        if (state == this.currentState) return;

        this.currentState = state;
    }
        
    void OnCollisionEnter(Collision collision)  
    {
        if (collision.transform.tag == "Stage")
        {
            if (!canJump)
            {
                //0.5�b��ɃW�����v�o����悤��
                Invoke("CanJump", 0.5f);
            }

            //�G�t�F�N�g�̔����ʒu�����߂�
            Vector3 efePos = collision.contacts[0].point;
            efePos.x = transform.position.x;
            efePos.z = transform.position.z;

            //�G�t�F�N�g
            ((NotHitStickGameManager)GameManager.nowMiniGameManager).tyakutiEffect(efePos);

            isJump = false;
            isJump2 = false;

            if(rb != null)
                rb.velocity = Vector3.zero;
        }

        //�v���C���[�ɓ���������
        if (collision.transform.tag == "Player")
        {
            //�������������v���C���[�����ɂ��邩�A�X�^�����Ă�����I���
            if (collision.transform.position.y < transform.position.y || isStun)
            {
                if(!gameObject.GetComponent<NotHitStickPlayer>().isStun)
                    DowbleJump();

                isJump = false;

                //�G�t�F�N�g
                ((NotHitStickGameManager)GameManager.nowMiniGameManager).hitEffect(new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y + 0.1f, collision.contacts[0].point.z));

                return;
            }
           
            
            //�W�����v���Ă��āA���肪�X�^������Ȃ�������
            //����ɖ��G�ł͂Ȃ��Ȃ�
            if(collision.gameObject.GetComponent<NotHitStickPlayer>().isJump || collision.gameObject.GetComponent<NotHitStickPlayer>().isJump2 &&
                !collision.gameObject.GetComponent<NotHitStickPlayer>().isStun && !isInvincible)

                //�X�^����Ԃɂ���
                StunMe();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Sea")
        {
            StartCoroutine(Kill(1.0f));
        }
    }

    //���S
    IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
        GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);
        Destroy(this.gameObject);
    }

    //���Ƃ�
    IEnumerator Drop(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�͂��~�߂�
        rb.velocity = Vector3.zero;

        //�_�Ŏ~�߂�
        tweener.Pause();
    }

    //�X�^����Ԃɂ���
    public void StunMe()
    {
        //�X�^����Ԃ�
        isStun = true;

        //�ׂ��
        this.transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);

        // 2�b��ɉ���
        Invoke("CancellationStun",1.5f);�@
    }

    //�X�^������
    public void CancellationStun()
    {
        //�ׂ��
        this.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        isStun = false;

        //���G��
        isInvincible = true;

        //2�b�㖳�G����
        Invoke("ResetInvincible", 2);
    }

    //�_�u���W�����v
    public void DowbleJump()
    {
        if (isInvokeJump) return;

        isInvokeJump = true;

        //�W�����v
        rb.AddForce(Vector3.up * (jumpPower * 0.8f));

        //�����W�����v���ł��Ȃ��悤��
        Invoke("ResetJump", 0.3f);
    }


    public void ResetJump()
    {
        isInvokeJump = false;
    }

    //���G����
    public void ResetInvincible()
    {
        isInvincible = false;
    }

    //�W�����v�o����悤�ɂ���
    public void CanJump()
    {
        canJump = true;
    }
}
