using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotFallHolePlayer : MonoBehaviour
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
    [SerializeField] private float jumpPower = 0.05f;         // �W�����v��
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    [SerializeField] private Vector3 localGravity;
    private bool isJump;
    private bool isJumpInvoke;
    private Rigidbody rBody;
    private Transform mainCameraTransform; // ���C���J������Transform
    private Ray ray; // Ray�𐶐�

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        isJump = false;
        isJumpInvoke = false;
        rBody = this.GetComponent<Rigidbody>();
        playerNum = this.GetComponent<PlayerNum>().playerNum;
    }

    //��̃e�N�X�`���ݒ�
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //��ԍX�V
        StateUpdata();

        //����
        Move();

        //�W�����v
        Jump();
    }

    private void FixedUpdate()
    {
        SetLocalGravity(); //�d�͂�AddForce�ł����郁�\�b�h���ĂԁBFixedUpdate���D�܂����B
    }

    private void SetLocalGravity()
    {
        rBody.AddForce(localGravity, ForceMode.Acceleration);
    }

    //�ړ�
    private void Move()
    {
        //�J�n���Ă��Ȃ����I����Ă���̂Ȃ�
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

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
       // ChangeStateTo(SlimeAnimationState.Walk);

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
        if (!Input.GetButtonDown("Abutton" + playerNum) || isJump) return;

        //�W�����v��ԂɕύX    
        rBody.AddForce(Vector3.up * jumpPower);
        isJump = true;
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

    void SetResetJump() { isJump = false; isJumpInvoke = false; }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Floor" && !other.transform.parent.GetComponent<FallRotateFloor>().isRotate && !isJumpInvoke && isJump)
        {
            isJumpInvoke = true;
            Invoke("SetResetJump", 0.3f);
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (!(other.transform.tag != "Player")) return;

        RaycastHit hit;
        ray = new Ray(transform.position, Vector3.up);
        if (rBody.velocity.y < 0 && Physics.Raycast(ray, out hit, 10000) && hit.transform.tag == "Player" && hit.transform.gameObject != this.gameObject)
        {
            isJumpInvoke = true;
            rBody.AddForce(Vector3.up * jumpPower);
            Invoke("SetResetJump", 0.3f);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Sea")
        {
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
            GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);
        }
    }

}
