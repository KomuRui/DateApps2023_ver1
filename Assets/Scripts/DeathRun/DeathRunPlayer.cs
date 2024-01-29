using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathRunPlayer : MonoBehaviour
{

    //�A�j���[�V�����ɕK�v
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    Rigidbody rb;

    public Animator animator;
    public int damType;

    private Material faceMaterial;

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

    private bool isGoal = false;

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
        //�J�n���Ă��Ȃ����I����Ă���̂Ȃ�
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;
        if (isGoal) return;

        //����
        Move();

        //�W�����v�@
        //Jump();

        //�����X�N���[��
        ForcedScrolling();

        //��ԍX�V
        StateUpdata();

        //�d��
        SetLocalGravity();
    }

    private void SetLocalGravity()
    {
        rb.AddForce(new Vector3(0,-3,0), ForceMode.Acceleration);
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
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        rb.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        //rb.velocity = -moveDirection;

        //transform.position.x = Math.Clamp(transform.position.x, -3.5f, 3.5f);

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

    //�����X�N���[��
    public void ForcedScrolling()
    {
        Debug.Log("Cube Screen" + Camera.main.WorldToScreenPoint(this.transform.position));

        //�X�N���[�����W�ɕϊ�
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.transform.position);

        //��ʂ���o�Ȃ��悤��
        if (screenPoint.y < 0)
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, 0, screenPoint.z));
            transform.position = new Vector3(vec.x, vec.y,vec.z);
        }
        else if (screenPoint.y > Screen.height)
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, Screen.height, screenPoint.z));
            transform.position = new Vector3( vec.x, vec.y, vec.z);
        }

        //�����d�͂��K�p����Ă��Ȃ��̂Ȃ��ɂ����Ȃ��悤��
        if(!rb.useGravity)
            transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
    }

    void OnTriggerStay(Collider other)
    {
        //�S�[���ɐG�ꂽ��
        if (other.gameObject.tag == "Goal")
        {
            isGoal = true;
            GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);
            ((DeathRunGameManager)GameManager.nowMiniGameManager).goalPlayer.Add(this.GetComponent<PlayerNum>().playerNum);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //�e�ɓ���������
        if (collision.gameObject.tag == "Bullet")
        {
            FallPlayer();
        }

        //�A���J�[�ɓ��������玞��
        if (collision.gameObject.tag == "Anchor")
        {
            FallPlayer();
        }
    }

    //�v���C���[����
    public void FallPlayer()
    {
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.useGravity = true;
        Invoke("DestroyPlayer", 1.0f);
    }

    void DestroyPlayer()
    {
        // �~�j�Q�[���Ɏ��񂾂��Ƃ�`����
        if(TutorialManager.isTutorialFinish)
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
        
        GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);

        //�I�u�W�F�N�g���폜
        Destroy(this.gameObject);
    }
}