using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThreePlayer : MonoBehaviour
{

    //�A�j���[�V�����ɕK�v
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }
    Rigidbody rb;
    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    private Material faceMaterial;

    GameObject obj;

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

    bool isDestroy;
    private Transform mainCameraTransform; // ���C���J������Transform

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        isDestroy = false;
        rb = this.GetComponent<Rigidbody>();  // rigidbody���擾

        obj = GameObject.Find("GameManager");
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

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), Mathf.Clamp(transform.position.y, -13.5f, -0.8f), Mathf.Clamp(transform.position.z, -10.5f, -5.5f));
        
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
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

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
        
        
    //transform.position.x = Math.Clamp(transform.position.x, -3.5f, 3.5f);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    ////�W�����v
    //private void Jump()
    //{
    //    //A�{�^����������ĂȂ��̂Ȃ炱�̐揈�����Ȃ�
    //    if (!Input.GetButtonDown("Abutton" + +playerNum)) return;

    //    //�W�����v��ԂɕύX    
    //    ChangeStateTo(SlimeAnimationState.Jump);
    //}

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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Debug.Log("��������!");
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.useGravity = true;
            Invoke("DestroyPlayer",1.0f);

            this.enabled = false;



        }
    }


    void DestroyPlayer()
    {
        Destroy(this.gameObject);
    }



    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, -30, 0),ForceMode.Acceleration);
    }
}