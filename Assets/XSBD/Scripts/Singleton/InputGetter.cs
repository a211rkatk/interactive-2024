using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGetter : MonoBehaviour
{
    [SerializeField] VideoTree _tree;
    [SerializeField] KeyCode _keyA = KeyCode.A;
    [SerializeField] KeyCode _keyB = KeyCode.B;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (VideoPlayerManager.GetPlayer().isLooping || Timer._choice)
        {
            if (Input.GetKeyDown(_keyA))
            {
                _tree.Proceed(true);
            }
            if (Input.GetKeyDown(_keyB))
            {
                _tree.Proceed(false);
            }

            if(Timer._autoChoice)
            {
                _tree.RandomProceed();
            }
        }
    }
}
