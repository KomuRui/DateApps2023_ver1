using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConsecutivePlayer : MonoBehaviour
{
    //�A�j���[�V�����ɕK�v
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    [SerializeField] private Vector3 moveDirection = new Vector3(0.0f, 0.0f, -1.0f); //�v���C���[�̈ړ�����

    private Material faceMaterial;

    [SerializeField] private float deceleration = 150.0f;       //������
    [SerializeField] private float addSpeed = 1.1f;             // �{�^�����������Ƃ��v���C���[�̈ړ����x�̏㏸�l
    [SerializeField] private float moveSpeed = 0.01f;           // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;      // �v���C���[�̉�]���x
    [SerializeField] private bool isHorizontalInput = true;     // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;       // �c�̓��͋����邩
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private float buttonCount = 0.0f;           // ���͂��擾�p
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�

    private Transform mainCameraTransform; // ���C���J������Transform

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //�v���C���[�̈ړ������̐��K��
        moveDirection.Normalize();
    }

    //��̃e�N�X�`���ݒ�
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //����
        Move();

        //�W�����v
        //Jump();

        //��ԍX�V
        StateUpdata();
    }

    //�ړ�
    private void Move()
    {
        // ���͂��擾
        if (Input.GetButtonDown("Abutton" + playerNum))
        {
            buttonCount += addSpeed;
        }
            
        //���x��0�Ȃ��
        if (moveSpeed <= 0)
        {
            //�ʏ��ԂɕύX
            ChangeStateTo(SlimeAnimationState.Idle);
        }
        else
        {
            //�ʏ��ԂɕύX
            ChangeStateTo(SlimeAnimationState.Idle);

            //�W�����v��ԂɕύX
            //ChangeStateTo(SlimeAnimationState.Walk);

            // �ړ�
            //�A�j���[�V�����̑��x�ɍ��킹�邽�߂ɒx������
            Vector3 animationSpeed = new Vector3 (0.0f,0.0f,0.002f);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.position -= animationSpeed;
        }

        buttonCount -= addSpeed / deceleration;
        //buttonCount��0�Ȃ�
        if (buttonCount <=0 )
        {
            buttonCount = 0.0f;
        }

        moveSpeed = buttonCount;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //�W�����v
    private void Jump()
    {
        //A�{�^����������ĂȂ��̂Ȃ炱�̐揈�����Ȃ�
        if (!Input.GetButtonDown("Abutton1")) return;
        
        //�W�����v��ԂɕύX    
        ChangeStateTo(SlimeAnimationState.Jump);
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

    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (this == null) return;
        if (state == this.currentState) return;

        this.currentState = state;
    }
}