using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButton : MonoBehaviour
{

    [SerializeField] GimmickBase targetGimmick;

    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Abutton1") && isHit)
            targetGimmick.SetIsInput(true);
    }

    void OnTriggerStay(Collider other)
    {
        isHit = true;
    }

    void OnTriggerExit(Collider other)
    {
        isHit = false;
    }
}
