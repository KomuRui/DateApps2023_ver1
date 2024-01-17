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

    [SerializeField] public List<Image> nextCommandImage; //���̃R�}���h�̉摜��\������ꏊ�̃��X�g
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
    [SerializeField] private ChasesManager chasesManager;     //�}�l�[�W���[
    [SerializeField] private ConsecutiveSE se;                          //SE

    public int threePlayerNum = 0;
    public bool buttonFlag = true;
    public bool isGoal = false; //�S�[��������
    public bool isDead = false; //����ł��邩
    public bool isMiss = false; //�����ɓ��͂��ԈႦ�����ǂ���

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

        nextCommandImage[threePlayerNum].sprite = commandImageList[1];
        int a = 0;
    }

    //��̃e�N�X�`���ݒ�
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //�������Q�[�����n�܂��Ă��āA�I����Ă����A�S�[�����Ă��Ȃ�������
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish() && !isGoal)
        {
            //����
            Move();
        }

        //��ԍX�V
        StateUpdata();
    }

    //�ړ�
    private void Move()
    {
        // ���͂��擾
        bool isAbuttonClick = Input.GetButtonDown("Abutton" + this.gameObject.GetComponent<PlayerNum>().playerNum);
        bool isBbuttonClick = Input.GetButtonDown("Bbutton" + this.gameObject.GetComponent<PlayerNum>().playerNum);

        //���͂��������Ă��邩�ǂ���
        bool isInputSuccess = true;


        if (buttonFlag)
        {
            // A�{�^���������Ă�����
            if (isAbuttonClick)
            {
                buttonCount += addSpeed;
                buttonFlag = !buttonFlag;
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
            }
        }

        //�������͂Ɏ��s���Ă�����
        if (!isInputSuccess)
        {
            //��������
            buttonCount -= missDeceleration;

            isMiss = true;
            Invoke(nameof(SetFalseMiss), 0.1f);
        }

        ImageChange();

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
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime * 400, ForceMode.Force);    // �͂�������
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //�W�����v
            Jump();
        }

        //���t���[����������
        buttonCount -= deceleration;

        //buttonCount��0�Ȃ�
        if (buttonCount <= 0)
        {
            buttonCount = 0.0f;
        }

        //�����X�s�[�h���ő�ɂȂ�����
        if (SPEED_MAX <= buttonCount)
        {
            buttonCount = SPEED_MAX;
        }

        moveSpeed = buttonCount;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

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
            //Debug.Log(playerNum + "P Goal"); // ���O��\������
            //isGoal = true;

            //�Q�[���}�l�[�W���[�ɏI��������Ƃ�`����
            //chasesManager.PlayerGoal(this.GetComponent<PlayerNum>().playerNum);
            //GameManager.nowMiniGameManager.PlayerFinish(this.gameObject.GetComponent<PlayerNum>().playerNum);
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit"); // ���O��\������

            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();  // rigidbody���擾
            Vector3 force = new Vector3(0.0f, 15000.0f, 0.0f);  // �͂�ݒ�
            rb.AddForce(force * Time.deltaTime, ForceMode.Force);          // �͂�������

            isDead = true;
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);

            se.MissAudio();
            //�Q�[���}�l�[�W���[�ɏI��������Ƃ�`����
            //GameManager.nowMiniGameManager.PlayerFinish(this.gameObject.GetComponent<PlayerNum>().playerNum);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            Debug.Log(playerNum + "P Goal"); // ���O��\������
            isGoal = true;

            ChasesManager a = GameManager.nowMiniGameManager.gameObject.GetComponent<ChasesManager>();
            a.goalPlayer.Add(gameObject.GetComponent<PlayerNum>().playerNum);
        }
    }

    /// <summary>
    /// �摜��ς���֐�
    /// </summary>
    public void ImageChange()
    {
        //���O�ɓ��͂��Ԉ���Ă�����
        if(isMiss)
        {
            if (buttonFlag) 
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[2];
            }
            else
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[3];
            }
        }
        else
        {
            if (buttonFlag)
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[0];
            }
            else
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[1];
            }
        }
        
    }

    //�Z�b�^�[
    public void SetFalseMiss()
    {
        isMiss = false;
    }

}