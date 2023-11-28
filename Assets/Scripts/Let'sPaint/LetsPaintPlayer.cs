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
    [SerializeField]  private ChildCol col;


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
        //�J�n���Ă��Ȃ����I�����Ă���̂Ȃ炱�̐揈�����Ȃ�
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

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

        //�����_�����O���[�h�ς���
        Material material = this.GetComponent<MeshRenderer>().material;
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
      
        //�F�ς���
        Color r2 = material.color;
        r2.a = 0.6f;
        material.color = r2;

        //�̓I�t
        rBody.velocity = Vector3.zero;

        //������
        this.gameObject.layer = 7;
        col.isMuteki = true;

        //�R���[�`��
        StartCoroutine(ReStart(1.0f));
    }

    //�X�^�[�g
    IEnumerator ReStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //���X�|�[�g
        isRespawn = false;

        if (col.hitObj == null)
            ReturnAlpha();
    }

    public void ReturnAlpha()
    {
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

        //�����_�����O���[�h�ς���
        Material material = this.GetComponent<MeshRenderer>().material;
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 2000;

        //���ɖ߂�
        Color r2 = material.color;
        r2.a = 1.0f;
        material.color = r2;

        col.isMuteki = false;
        this.gameObject.layer = 6;
        col.hitObj = null;
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
