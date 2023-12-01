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
    [SerializeField] private GameObject[] stage;              // ���̃Q�[���I�u�W�F�N�g
    [SerializeField] private float jumpPower;                 // �W�����v��
    [SerializeField] private int nowStageNum;                 // ����Ă��鏰�̔ԍ�
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�

    //���W�b�g�{�f�B
    private Rigidbody rb;

    //�W�����v���Ă��邩
    private bool isJump = false;
    private bool isJump2 = false;

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
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

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
        if (isJump  || isJump2) return;

        // ���͂��擾�p
        float horizontalInput = 0;
        float verticalInput = 0;

        // ���͂��擾
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        if (isVerticalInput) verticalInput = Input.GetAxis("L_Stick_V" + playerNum);

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

        // �ړ�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

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
        if (Input.GetButtonDown("Abutton" + playerNum))
        {
            //�ʏ��ԂɕύX
            ChangeStateTo(SlimeAnimationState.Idle);

            //��ɗ͂�������
            rb.AddForce(Vector3.up * jumpPower);
            isJump2 = true;
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
            tweener = transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f).OnComplete(() => { isJump2 = false; isJump = false; });
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
            tweener = transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f).OnComplete(() => { isJump2 = false; isJump = false; });
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
            isJump2 = false;

            if(rb != null)
                rb.velocity = Vector3.zero;
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
}
