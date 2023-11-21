using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    [SerializeField] private Image nextCommandImage; //���̃R�}���h�̉摜��\������ꏊ�̃��X�g
    [SerializeField] private List<Sprite> commandImageList = new List<Sprite>(); //�R�}���h�̉摜�̃��X�g�i���̉摜���g�����j
    [SerializeField] private float SPEED_MAX = 14.0f;       //�X�s�[�h�̍ő�
    [SerializeField] private float deceleration = 150.0f;       //������
    [SerializeField] private float missDeceleration = 50.0f;       //������
    [SerializeField] private float addSpeed = 100.0f;          // �{�^�����������Ƃ��v���C���[�̈ړ����x�̏㏸�l
    [SerializeField] private float moveSpeed = 0.0f;           // �v���C���[�̈ړ����x
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
    
    public bool buttonFlag = true;
    public bool goolFlag = false;
    public bool isDead = false;

    private Transform mainCameraTransform; // ���C���J������Transform

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //�v���C���[�̈ړ������̐��K��
        moveDirection.Normalize();

        buttonFlag = false;

        nextCommandImage.sprite = commandImageList[1];
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

        //��ԍX�V
        StateUpdata();
    }

    //�ړ�
    private void Move()
    {
        // ���͂��擾
        bool isAbuttonClick = Input.GetButtonDown("Abutton" + playerNum);
        bool isBbuttonClick = Input.GetButtonDown("Bbutton" + playerNum);

        //���͂��������Ă��邩�ǂ���
        bool isInputSuccess = true;


        if (buttonFlag)
        {
            // A�{�^���������Ă�����
            if (isAbuttonClick)
            {
                buttonCount += addSpeed;
                buttonFlag = !buttonFlag;
                nextCommandImage.sprite = commandImageList[1];
            }

            // B�{�^���������Ă�����
            if (isBbuttonClick)
            {
                isInputSuccess = false;
            }
        }
        else
        {
            // A�{�^���������Ă�����
            if (isAbuttonClick)
            {
                isInputSuccess = false;
            }

            // B�{�^���������Ă�����
            if (isBbuttonClick)
            {
                buttonCount += addSpeed;
                buttonFlag = !buttonFlag;
                nextCommandImage.sprite = commandImageList[0];
            }
        }
       
        //�������͂Ɏ��s���Ă�����
        if (!isInputSuccess)
        {
            buttonCount -= missDeceleration;
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
            Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbody���擾
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);    // �͂�������
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //�W�����v
            Jump();
        }

        //���t���[����������
        buttonCount -= deceleration;

        //buttonCount��0�Ȃ�
        if (buttonCount <= 0 )
        {
            buttonCount = 0.0f;
        }

        //�����X�s�[�h���ő�ɂȂ�����
        if(SPEED_MAX <= buttonCount)
        {
            buttonCount = SPEED_MAX;
        }

        moveSpeed = buttonCount;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        //�����܂ōs���Ə�����
        if(this.transform.position.y > 100)
        {
           // Destroy(this.gameObject);
        }
    }

    //�W�����v
    private void Jump()
    {
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

    // �����������ɌĂ΂��֐�
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            Debug.Log(playerNum + "P Goal"); // ���O��\������
            goolFlag = true;
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit"); // ���O��\������

            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();  // rigidbody���擾
            Vector3 force = new Vector3(0.0f, 800.0f, 1.0f);  // �͂�ݒ�
            rb.AddForce(force, ForceMode.Force);          // �͂�������

            isDead = true;
        }

       
    }
}