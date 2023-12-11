using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButton : MonoBehaviour
{

    [SerializeField] GimmickBase targetGimmick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Abutton1"))
            targetGimmick.SetIsInput(true);
    }
}
