using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarrtBall : MonoBehaviour
{

    [SerializeField] private List<Transform> tyekkuPoint = new List<Transform>();
    private Transform tyekkuPointPos;
    private Transform nextTyekkuPointPos;
    private Rigidbody rb;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        tyekkuPointPos = tyekkuPoint[0];
        nextTyekkuPointPos = tyekkuPointPos;
        startPos = transform.position;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.AddForce(new Vector3(0, 0.01f, 0));

        //äCÇÊÇËâ∫Ç…åæÇ¡ÇΩÇÁÉäÉXÉ|Å[Éì
        if (transform.position.y <= -1.5)
        {
            transform.position = new Vector3(tyekkuPointPos.position.x,tyekkuPointPos.position.y + 1.5f, 22.85f);
            rb.velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Goal" && !GameManager.nowMiniGameManager.IsFinish())
        {
            ((CarryToTheGoalGameManager)GameManager.nowMiniGameManager).isGoal = true;
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }

        if (collision.gameObject.tag == "TyekkuPoint" && nextTyekkuPointPos.position.y > collision.gameObject.transform.position.y)
        {
            tyekkuPointPos = nextTyekkuPointPos;
            nextTyekkuPointPos = collision.gameObject.transform;
        }
    }
}