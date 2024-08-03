using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class CreateInstructionFile : MonoBehaviour
{
    [SerializeField] bool _loadScene = false;
    [SerializeField] string _destinationName;
    [SerializeField] string _instructionDirectory;
    [Space(30f)]
    [SerializeField] bool _generate;

    // Update is called once per frame
    void Update()
    {
        if (_generate)
        {
            _generate = false;
            PrintFile(new Instruction(_loadScene, _destinationName));
        }
    }

    private void PrintFile(Instruction instruction)
    {
        string path = (_instructionDirectory == "") ? Application.dataPath + "/Resources" : _instructionDirectory;
        string name = ((_loadScene) ? "Load_" : "Restart_At_") + _destinationName;

        File.WriteAllText(Path.Combine(path, name + ".txt"), JsonUtility.ToJson(instruction));
    }
}
