using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction
{
    public bool _loadScene;
    public string _name;

    public Instruction(bool loadScene, string name)
    {
        _loadScene = loadScene;
        _name = name;
    }
}
