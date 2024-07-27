using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class CreateInstructionFile : MonoBehaviour
{
    [SerializeField] bool _loadScene;
    [SerializeField] string _name;
    [SerializeField] string _directory;
    [Space(30f)]
    [SerializeField] bool _generate;

    // Update is called once per frame
    void Update()
    {
        if (_generate)
        {
            _generate = false;
            PrintFile(new Instruction(_loadScene, _name));
        }
    }

    private void PrintFile(Instruction instruction)
    {
        string path = (_directory == "") ? Application.dataPath + "/Resources" : _directory;
        string name = ((_loadScene) ? "Load_" : "Restart_At_") + _name;

        File.WriteAllText(Path.Combine(path, name + ".txt"), JsonUtility.ToJson(instruction));
    }
}
