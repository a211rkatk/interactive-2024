using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGetter : MonoBehaviour
{
    [SerializeField] VideoTree _tree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _tree.Proceed(true);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            _tree.Proceed(false);
        }
    }
}
