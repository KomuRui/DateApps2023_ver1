using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Windows;
using Random = UnityEngine.Random;

public class ChasesPlayer : MonoBehaviour
{
    //�A�j���[�V�����ɕK�v
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    
    private Material faceMaterial;

    [SerializeField] private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 1.0f); //�v���C���[�̈ړ�����
    [SerializeField] private float deceleration = 150.0f;       //������
    [SerializeField] private float missDeceleration = 50.0f;       //������
    [SerializeField] private float SPEED_MAX = 15.0f;           //�X�s�[�h�̍ő�
    [SerializeField] private float addSpeed = 1000.0f;             // �{�^�����������Ƃ��v���C���[�̈ړ����x�̏㏸�l
    [SerializeField] private float moveSpeed = 0.01f;           // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;      // �v���C���[�̉�]���x
    [SerializeField] private bool isHorizontalInput = true;     // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;       // �c�̓��͋����邩
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private float buttonCount = 0.0f;                // ���͂��擾�p
    [SerializeField] private float crossAxisV;                        //�\���L�[�̏c�̓��͒l
    [SerializeField] private float crossAxisH;                        //�\���L�[�̉��̓��͒l
    [SerializeField] private float COMMAND_SIZE_MAX = 4;              //���̃R�}���h�̃��X�g�̍ő吔
    [SerializeField] private int playerNum;                           // �v���C���[�ԍ�
    [SerializeField] private Queue<COMMAND_TYPE> nextCommand = new();                    //���̃R�}���h�̃L���[
    [SerializeField] private List<Tuple<COMMAND_TYPE, COMMAND_STATE>> commandList = new();                    //���̃R�}���h�̃L���[
    [SerializeField] public List<Image> nextCommandImageList = new List<Image>(); //���̃R�}���h�̉摜��\������ꏊ�̃��X�g
    [SerializeField] private List<Sprite> commandImageList = new List<Sprite>(); //�R�}���h�̉摜�̃��X�g�i���̉摜���g�����j
    [SerializeField] private List<Sprite> newCommandImageList = new List<Sprite>(); //�R�}���h�̉摜�̃��X�g�i���̉摜���g�����j

    private bool crossKeyContinuous = false;    //�\���L�[
    private bool isControll = false;            //���삵�Ă��邩�ǂ���
    bool isMiss = false;//���O�Ƀ~�X�������ǂ���
    COMMAND_TYPE nextCommandButton = COMMAND_TYPE.COMMAND_MAX;                    //���̃R�}���h

    private Transform mainCameraTransform; // ���C���J������Transform

    //�R�}���h�̎��
    public enum COMMAND_TYPE
    {
        CROSS_BUTTON_UP = 0,
        CROSS_BUTTON_DOWN,
        CROSS_BUTTON_LEFT,
        CROSS_BUTTON_RIGHT,
        NONE,
        COMMAND_MAX,
    }

    //�R�}���h���͂̌���
    public enum COMMAND_RESULT
    {
        SUCCESS = 0,
        MISS,
        NONE,
        MAX,
    }

    public enum COMMAND_STATE
    {
        DEFAULT = 0,
        NEXT,
        SUCCESS,
        MISS,
        MAX,
    }

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //�v���C���[�̈ړ������̐��K��
        moveDirection.Normalize();

        //�R�}���h�̏�����
        KeepCommand();

        //�R�}���h�̏�Ԃ̏�����
        KeepStateCommand(COMMAND_RESULT.NONE);

        //�R�}���h�̉摜�����ւ���
        SetCommandImage();

        ///////////�����O/////////////
        //KeepCommand();

        //�R�}���h�摜������
        //SetCommandImage();
        ///////////////////////////////
    }

    //��̃e�N�X�`���ݒ�
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //�������Q�[�����n�܂��Ă��āA�I����Ă��Ȃ�������
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish())
        {
            //�R�}���h�̉摜�����ւ���
            SetCommandImage();

            //����
            Move();
        }

        //��ԍX�V
        StateUpdata();
    }

    //�ړ�
    private void Move()
    {
        //���͂��󂯕t����
        COMMAND_RESULT command = CheckOnCommandButton();

        //�R�}���h���͂��������Ă�����
        switch (command)
        {
            case COMMAND_RESULT.SUCCESS:

                //�X�s�[�h���グ��
                buttonCount += addSpeed;
                /////////////////////�����O/////////////////
                //�R�}���h�����������ꍇ�̏���
                //SuccessCommand();
                ////////////////////////////////////////////

                break;
            case COMMAND_RESULT.MISS:

                //�R�}���h�����s�����ꍇ�̏���
                //���ʌ�������
                buttonCount -= missDeceleration;

                break;
            case COMMAND_RESULT.NONE:

                break;
            default:
                break;
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
            //ChangeStateTo(SlimeAnimationState.Jump);

            // �ړ�
            Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbody���擾
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime * 150, ForceMode.Force);    // �͂�������
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //�W�����v
            Jump();
        }

        //�R�}���h�̏�Ԃ�ς���
        KeepStateCommand(command);

        buttonCount -= deceleration;

        //buttonCount��0�Ȃ�
        if (buttonCount <= 0)
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

    //���̃R�}���h�̃{�^���������ꂽ���ǂ������ׂ�
    public COMMAND_RESULT CheckOnCommandButton()
    {
        //�\���L�[�̓��͂��󂯎��
        crossAxisV = UnityEngine.Input.GetAxis("D_Pad_V" + this.GetComponent<PlayerNum>().playerNum);
        crossAxisH = UnityEngine.Input.GetAxis("D_Pad_H" + this.GetComponent<PlayerNum>().playerNum);

        //�\���L�[�������ꂽ�������񉟂���悤�ɂȂ�
        if (crossAxisV == 0 && crossAxisH == 0)
        {
            crossKeyContinuous = false;
        }

        if (!crossKeyContinuous)
        {
            COMMAND_TYPE priorityCommand = COMMAND_TYPE.NONE;   //�D��R�}���h
            float priorityLevel = 0.0f;     //�D��x

            //�R�}���h�`�F�b�N       
            if (crossAxisV > 0)
            {
                //�D��R�}���h����
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_UP;

                //�D��x����
                priorityLevel = Mathf.Abs(crossAxisV);

                Debug.Log("��");
            }
            if (crossAxisV < 0 && priorityLevel < Mathf.Abs(crossAxisV))
            {
                //�D��R�}���h����
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_DOWN;

                //�D��x����
                priorityLevel = Mathf.Abs(crossAxisV);
                Debug.Log("��");
            }
            if (crossAxisH < 0 && priorityLevel < Mathf.Abs(crossAxisH))
            {
                //�D��R�}���h����
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_LEFT;

                //�D��x����
                priorityLevel = Mathf.Abs(crossAxisH);

                Debug.Log("��");
            }
            if (crossAxisH > 0 && priorityLevel < Mathf.Abs(crossAxisH))
            {
                //�D��R�}���h����
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_RIGHT;

                Debug.Log("�E");
            }

            //�R�}���h���͂��ĂȂ�������
            if (priorityCommand == COMMAND_TYPE.NONE)
            {
                crossKeyContinuous = false;
                return COMMAND_RESULT.NONE;
            }
            //�R�}���h���͂ɐ������Ă�����
            else if (nextCommandButton == priorityCommand)
            {
                crossKeyContinuous = true;
                return COMMAND_RESULT.SUCCESS;
            }
            //�R�}���h���͂Ɏ��s������
            else
            {
                crossKeyContinuous = true;
                return COMMAND_RESULT.MISS;
            }
        }
        return COMMAND_RESULT.NONE;
    }

    //�����_���ō�����R�}���h�I��
    public COMMAND_TYPE RandCommand()
    {
        switch (Random.Range(0, 4))
        {
            case 0 :
                return COMMAND_TYPE.CROSS_BUTTON_UP;
            case 1:
                return COMMAND_TYPE.CROSS_BUTTON_DOWN;
            case 2:
                return COMMAND_TYPE.CROSS_BUTTON_LEFT;
            case 3:
                return COMMAND_TYPE.CROSS_BUTTON_RIGHT;
            default:
                break;
        }
        return COMMAND_TYPE.CROSS_BUTTON_UP;
        ////////////////�����O////////////////////////////////
        //return (COMMAND_TYPE)Random.Range((int)COMMAND_TYPE.CROSS_BUTTON_UP, 4);
    }

    //�R�}���h�����������ꍇ�̏���
    public void SuccessCommand()
    {
        ////////////////�����O////////////////////////////////
        ////�R�}���h�̍폜
        //nextCommand.Dequeue();

        ////�R�}���h����萔�ɕۂ���
        //KeepCommand();

        ////�R�}���h�摜������
        //SetCommandImage();
    }

    //�R�}���h����萔�ɕۂ���
    public void KeepCommand()
    {
        //�R�}���h�����鏈��
        for (int i = commandList.Count; i < COMMAND_SIZE_MAX; i++)
        {
            //�����_���ɃR�}���h������
            Tuple<COMMAND_TYPE, COMMAND_STATE> tmp = new(RandCommand(), COMMAND_STATE.DEFAULT);
            commandList.Add(tmp);
        }

        ////////////////�����O////////////////////////////////
        ////�R�}���h�̐��������Ă��鎞
        //for (int i = nextCommand.Count; i < COMMAND_SIZE_MAX; i++)
        //{
        //    //�����_���ɃR�}���h������
        //    nextCommand.Enqueue(RandCommand());
        //}
    }

    //�R�}���h�摜������
    public void SetCommandImage()
    {
        int num = 0;
        foreach (var item in commandList)
        {
            //�{�^���̏�Ԃ��f�t�H���g�Ȃ�
            if(item.Item2 == COMMAND_STATE.DEFAULT)
            {
                //�摜�����ւ���
                nextCommandImageList[num].sprite = newCommandImageList[((int)COMMAND_STATE.DEFAULT * 4) + (int)item.Item1];
            }
            //�{�^���̏�Ԃ��l�N�X�g�Ȃ�
            if (item.Item2 == COMMAND_STATE.NEXT)
            {
                //�摜�����ւ���
                nextCommandImageList[num].sprite =newCommandImageList[((int)COMMAND_STATE.NEXT * 4) + (int)item.Item1];
            }
            //�{�^���̏�Ԃ��T�N�Z�X�Ȃ�
            if (item.Item2 == COMMAND_STATE.SUCCESS)
            {
                //�摜�����ւ���
                nextCommandImageList[num].sprite = newCommandImageList[((int)COMMAND_STATE.SUCCESS * 4) + (int)item.Item1];
            }
            //�{�^���̏�Ԃ��~�X�Ȃ�
            if (item.Item2 == COMMAND_STATE.MISS)
            {
                //�摜�����ւ���
                nextCommandImageList[num].sprite = newCommandImageList[((int)COMMAND_STATE.MISS * 4) + (int)item.Item1];
            }

            num++;
        }

        ////////////////�����O////////////////////////////////
        //int num = 0;
        //foreach (COMMAND_TYPE item in nextCommand)
        //{
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_UP)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_UP];
        //    }
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_DOWN)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_DOWN];
        //    }
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_LEFT)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_LEFT];
        //    }
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_RIGHT)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_RIGHT];
        //    }
        //    num++;
        //}
    }

    // �����������ɌĂ΂��֐�
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            buttonCount = 0;
        }

        
    }

    //�R�}���h�̏�Ԃ���萔�ɕۂ���
    public void KeepStateCommand(COMMAND_RESULT input)
    {
        bool isFirst = true;
        bool isAllSuccess = true;

        //���O�ɃR�}���h���͂ɐ������Ă�����
        for (int i = 0; i < commandList.Count; i++)
        {
            if (input == COMMAND_RESULT.SUCCESS && commandList[i].Item2 != COMMAND_STATE.SUCCESS)
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.SUCCESS);
                break;
            }
        }

        //�R�}���h�̏�Ԃ�ύX
        for (int i = 0; i < commandList.Count; i++)
        {  
            if(commandList[i].Item2 == COMMAND_STATE.SUCCESS)
            {
            }
            else if(isMiss && isFirst)
            {
                //isMiss��0.1�b��ɉ�������֐�
                Invoke(nameof(SetFalseMiss), 0.1f);
                isFirst = false;
                isAllSuccess = false;
            }
            else if(input == COMMAND_RESULT.MISS && isFirst)
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.MISS);
                isMiss = true;
                isFirst = false;
                isAllSuccess = false;
            }
            else if(isFirst)
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.NEXT);
                isAllSuccess = false;
                isFirst = false;

                nextCommandButton = commandList[i].Item1;
            }
            else
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.DEFAULT);
                isAllSuccess = false;
                isFirst = false;
            }
        }

        //�R�}���h���S��SUCCESS�Ȃ�
        if (isAllSuccess)
        {
            //���X�g�̒��g������
            commandList.Clear();

            //�R�}���h��ǉ�
            KeepCommand();

            //�R�}���h�̏�Ԃ�ǉ�
            KeepStateCommand(COMMAND_RESULT.NONE);
        }
    }

    //�Z�b�^�[
    public void SetFalseMiss()
    {
        isMiss = false;
    }
}