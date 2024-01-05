using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryToTheGoalPlayer : MonoBehaviour
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
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    [SerializeField] private Vector3 localGravity;
    [SerializeField] private float jumpPower = 0.05f;         // �W�����v��
    private Rigidbody rBody;
    private Transform mainCameraTransform; // ���C���J������Transform
    private bool isDamege = false;
    public bool isMuteki = false;          //��C�̋ʓ�����Ȃ����G���ǂ���
    private bool isJump = false; 
    private bool isJumpInvoke = false;
    private bool isJumpMuteki = false;     //�W�����v������Ȃ����G���ǂ���
    private bool isStan = false;
    private Vector3 initialScale;
    private Vector3 stanScale;

    void Start()
    {
        //�}�e���A���ݒ�
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        rBody = this.GetComponent<Rigidbody>();
        initialScale = transform.localScale;
        stanScale = transform.localScale;
        stanScale.y = 1.3f;
    }

    //��̃e�N�X�`���ݒ�
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //�J�n���Ă��Ȃ����I����Ă���̂Ȃ�
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish() || isDamege || isStan) return;

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

        // ���͂��擾�p
        float horizontalInput = 0;
        float verticalInput = 0;

        // ���͂��擾
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + this.GetComponent<PlayerNum>().playerNum);

        //���͂��Ȃ��̂Ȃ�
        if ((horizontalInput == 0 && verticalInput == 0) || isDamege)
        {
            //�ʏ��ԂɕύX
            ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        // �J�����̌�������Ƀv���C���[���ړ�
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y��������0�ɂ��邱�ƂŐ��������ɐ���

        // �ړ��������v�Z
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        //������ԂɕύX
        if (!isJump)
        {
            ChangeStateTo(SlimeAnimationState.Walk);
            rBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            ChangeStateTo(SlimeAnimationState.Idle);
            rBody.AddForce(moveDirection * moveSpeed * 10000 * Time.deltaTime);
        }

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //�W�����v
    private void Jump()
    {
        //A�{�^����������ĂȂ��̂Ȃ炱�̐揈�����Ȃ�
        if (!Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) || isJump) return;

        //�G�t�F�N�g�̔����ʒu�����߂�
        Vector3 efePos = transform.position;
        efePos.y += 0.2f;

        //�G�t�F�N�g
        ((CarryToTheGoalGameManager)GameManager.nowMiniGameManager).JumpEffect(efePos);

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

    //�_���[�W
    public void Damege()
    {
        isDamege = true;
        isMuteki = true;
        rBody.velocity = Vector3.zero;
        ChangeStateTo(SlimeAnimationState.Idle);
        StateUpdata();


        var children = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < children.Length; i++)
        {
            Color r = children[i].material.color;
            r.a = 0.6f;
            children[i].material.color = r;
        }

        var children2 = this.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < children2.Length; i++)
        {
            Color r = children2[i].material.color;
            r.a = 0.6f;
            children2[i].material.color = r;
        }

        StartCoroutine(UnLook(2.0f));
        StartCoroutine(UnLookMuteki(4.0f));
    }

    IEnumerator UnLook(float delay)
    {
        yield return new WaitForSeconds(delay);

        isDamege = false;
    }

    IEnumerator UnLookMuteki(float delay)
    {
        yield return new WaitForSeconds(delay);

        var children = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < children.Length; i++)
        {
            Color r = children[i].material.color;
            r.a = 1.0f;
            children[i].material.color = r;
        }

        var children2 = this.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < children2.Length; i++)
        {
            Color r = children2[i].material.color;
            r.a = 1.0f;
            children2[i].material.color = r;
        }

        isMuteki = false;
    }

    //���S
    public void Dead()
    {
        GameManager.nowMiniGameManager.PlayerFinish(this.gameObject.GetComponent<PlayerNum>().playerNum);
        GameManager.nowMiniGameManager.PlayerDead(this.gameObject.GetComponent<PlayerNum>().playerNum);
        Destroy(this.gameObject);
    }

    public void SetResetJump() { isJump = false; isJumpInvoke = false; }
    public void SetResetMuteki() { isJumpMuteki = false; }
    public void SetResetStan()
    {
        isStan = false;
        SetResetScale();
    }
    public void SetResetScale() { transform.localScale = initialScale; }
    public void SetOhNoScale() { transform.localScale = stanScale; }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "CarryStage" && isJump)
        {
            //�G�t�F�N�g�̔����ʒu�����߂�
            Vector3 efePos = transform.position;
            efePos.y += 0.2f;

            //�G�t�F�N�g
            ((CarryToTheGoalGameManager)GameManager.nowMiniGameManager).JumpEffect(efePos);

            //�W�����v�̃C���^�[�o���J�n
            isJumpInvoke = true;
            Invoke("SetResetJump", 0.3f);
        }

        if (other.transform.tag != "Player") return;

        //��i�W�����v�̏������������Ă���̂Ȃ�
        if (rBody.velocity.y < 100 && other.transform.position.y < transform.position.y && isJump)
        {
            //��i�W�����v����
            isJumpInvoke = true;
            rBody.AddForce(Vector3.up * (jumpPower * 0.8f));

            //�G�t�F�N�g�̔����ʒu�����߂�
            Vector3 efePos = other.contacts[0].point;
            efePos.x = transform.position.x;
            efePos.z = transform.position.z;

            //�G�t�F�N�g���Փˈʒu��
            ((CarryToTheGoalGameManager)GameManager.nowMiniGameManager).HitEffect(efePos);

            //���������v���C���[���擾
            CarryToTheGoalPlayer targetPlayer = other.transform.GetComponent<CarryToTheGoalPlayer>();

            //���G�Ȃ炱�̐揈�����Ȃ�
            if (targetPlayer.isJumpMuteki) return;
            targetPlayer.isJumpMuteki = true;
            targetPlayer.isStan = true;
            targetPlayer.Invoke("SetResetStan", 3.0f);
            targetPlayer.Invoke("SetResetMuteki", 4.0f);
            targetPlayer.SetOhNoScale();
        }
    }
}
