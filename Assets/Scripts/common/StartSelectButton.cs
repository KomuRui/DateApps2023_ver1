using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StartSelectButton : MonoBehaviour
{

    [SerializeField] private Button startSelectButton;

    // Start is called before the first frame update
    void Start()
    {
        startSelectButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
