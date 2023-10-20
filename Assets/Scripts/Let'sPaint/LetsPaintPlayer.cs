using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsPaintPlayer : MonoBehaviour
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
    [SerializeField] private float flashingTime;              //�_�Ŏ���
    [SerializeField] private bool isHorizontalInput = true;   // ���̓��͋����邩
    [SerializeField] private bool isVerticalInput = true;     // �c�̓��͋����邩
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // �v���C���[�ԍ�
    [SerializeField] private Vector3 localGravity;
    private Rigidbody rBody;
    private Transform mainCameraTransform; // ���C���J������Transform

    private Vector3 initializPos;   //�����ʒu
    private bool isRespawn = false; //���݃��X�|�[�������ǂ���
    private Tweener tweener;        //Dotween�p


    // Start is called before the first frame update
    void Start()
    {
        //�����ʒu�ݒ�
        initializPos = transform.position;

        // ���C���J�������擾
        mainCameraTransform = Camera.main.transform;

        //���W�b�g�{�f�B�擾
        rBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
           // ChangeStateTo(SlimeAnimationState.Idle);
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
        rBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //���X�|�[���X�^�[�g
    IEnumerator StartRespawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�����ʒu����
        transform.position = initializPos;

        //�d�͂��~������
        rBody.isKinematic = true;

        //���b�V�������_���[���擾(�_��)
        MeshRenderer r = GetComponent<MeshRenderer>();
        tweener = r.material.DOFade(0.3f, flashingTime).SetLoops(-1, LoopType.Yoyo);

        //�R���[�`��
        StartCoroutine(ReStart(3.0f));
    }

    //�X�^�[�g
    IEnumerator ReStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�d�͂𕜊�
        rBody.isKinematic = false;

        //���X�|�[�g
        isRespawn = false;

        //�_�Ŏ~�߂�
        tweener.Restart();
        tweener.Pause();
    }

    //�����Ɠ����������ɌĂ΂��֐�
    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Sea" && !isRespawn)
        {
            isRespawn = true;
            StartCoroutine(StartRespawn(2.0f));
        }
    }
}
