using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReachExtenderThreePlayer : MonoBehaviour
{

    //�A�j���[�V�����ɕK�v
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    private Material faceMaterial;

    private Vector3 move;

    [SerializeField] private float defeatedSpeed = 1f;

    private Vector3 gravity = new Vector3(0f, 0f, 0f);

    [SerializeField] private bool isStan;
    [SerializeField] private bool isInvincible = false;

    //Rigidbody rb;

    [SerializeField] private float moveSpeed = 5.0f;          // �v���C���[�̈ړ����x
    [SerializeField] private float rotationSpeed = 180.0f;    // �v���C���[�̉�]���x
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    private bool isMoving = false;
    private bool isDead = false;
    private float stanTime = 2f;
    [SerializeField] private float invincibleTime = 2;

    Rigidbody rb;

    private Transform mainCameraTransform; // ���C���J������Transform

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        rb = this.GetComponent<Rigidbody>();  // rigidbody���擾
    }

    //��̃e�N�X�`���ݒ�
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //����Ă�����
        if (isDead)
        {
            Defeated();
            return;
        }

        if (isStan && !isInvincible)
        {
            Invoke("StanCancellation", stanTime);
            isInvincible = true;
        }

        //�����Ă�����
        if (isMoving || isStan) return;

        //�{�^���������ƃp���`����
        Action();

        //����
        Move();

        //��ԍX�V
        StateUpdata();
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
        //A�{�^����������ĂȂ��̂Ȃ炱�̐揈�����Ȃ�
        if (!Input.GetButtonDown("Abutton" + +this.GetComponent<PlayerNum>().playerNum)) return;

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

    public void SetIsMoving(bool a)
    {
        isMoving = a;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public void Action()
    {
        //A�{�^����������ĂȂ��̂Ȃ炱�̐揈�����Ȃ�
        if (!Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum)) return;

        SetIsMoving(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vortex")
        {
        }
        else
        {
        }
    }

    public void SetMove(Vector3 dir)
    {
        move = dir;
    }

    public void SetIsDead(bool a)
    {
        isDead = a;
    }

    //�|���ꂽ���̏���
    public void Defeated()
    {
        Vector3 vecUp = Vector3.up * 1f - gravity;
        transform.position += (move.normalized + vecUp) * defeatedSpeed * Time.deltaTime;
        gravity.y += 0.002f;

        //Vector3 vecUp = Vector3.up * 1f - gravity;
        //rb.AddForce((move.normalized + vecUp) * defeatedSpeed * Time.deltaTime);
        //gravity.y += 0.002f;
    }

    public void SetStan(bool a)
    {
        isStan = a;
    }

    //�X�^������
    public void StanCancellation()
    {
        isStan = false;
        Invoke("InvincibleCancellation", invincibleTime);
    }

    //���G����
    public void InvincibleCancellation()
    {
        isInvincible = false;
    }

    public void SetInvincible(bool a)
    {
        isInvincible = a;
    }
}