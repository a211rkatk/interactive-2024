using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTree : MonoBehaviour
{
    [Tooltip("Starts with the video stored in the folder directly under Resources which has this name\nResources ���� �ٷ� �Ʒ� �ִ� ������ �� �� �̸��� ���� �������� ����")]
    [SerializeField] string _beginAt = "";
    [SerializeField] KeyCode _key1;
    [SerializeField] KeyCode _key2;

    string _path;
    [SerializeField] VideoClip _currentClip;

    [Space(30f)]
    [SerializeField] bool _branchA;
    [SerializeField] bool _branchB;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartFromNewRoot(_beginAt);
    }

    // Update is called once per frame
    void Update()
    {
        if (_branchA)
        {
            _branchA = _branchB = false;
            Proceed(true);
        }
        if (_branchB)
        {
            _branchA = _branchB = false;
            Proceed(false);
        }
    }

    public void Proceed(bool AB)
    {
        //traverse to folder first
        Traverse(AB);

        //if there is a video, play that, and if not, get instructions.
        if (GetNextClip()) VideoPlayerManager.SetVideo(_currentClip);
        else DoAsTextSays();
    }

    bool GetNextClip()
    {
        VideoClip[] _clips = Resources.LoadAll<VideoClip>(_path);
        if (null == _clips && _clips.Length == 0) return false;

        _currentClip = _clips[^1];
        return true;
    }

    void Traverse(bool AB)
    {
        if (!Branch("AB"))
        {
            string next = (AB) ? "A" : "B";
            if (!Branch(next))
            {
                if (!RetraverseBranch())
                {
                    Debug.LogError("Tree met a dead end. Reloading Scene.");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    bool Branch(string next)
    {
        if (FolderExistsUnderResources(_path + "/" + next)) return false;

        _path += "/" + next;
        return true;
    }

    bool RetraverseBranch()
    {
        Object[] objects = Resources.LoadAll(ParentDirectory(_path) + "/Y");
        if(objects == null || objects.Length == 0) return false;

        _path = ParentDirectory(_path) + "/Y";
        return true;
    }

    void DoAsTextSays()
    {
        TextAsset[] texts = Resources.LoadAll<TextAsset>(_path);
        if (texts == null || texts.Length == 0) return;

        TextAsset text = texts[^1];
        Instruction instruction = JsonUtility.FromJson<Instruction>(text.text);

        if (instruction._loadScene) SceneManager.LoadScene(instruction._name);
        else StartFromNewRoot(instruction._name);
    }

    void StartFromNewRoot(string newRoot)
    {
        Debug.Log("Root set to: Resources/" + (_path = newRoot));

        if (GetNextClip()) VideoPlayerManager.SetVideo(_currentClip);
        else Debug.LogError("Couldn't get new sequence");
    }

    static string ParentDirectory(string path)
    {
        return path.Substring(0, path.LastIndexOf("/"));
    }

    /*
    string GetFileOfExtensionAtPath(string extension) // returns file name without extension at _path.
    {
        string[] paths = Directory.GetFiles(_resourcesPath + _path, "*." + extension);

        try { return Path.GetFileNameWithoutExtension(paths[0]); }
        catch { return ""; }
    }
    */

    bool FolderExistsUnderResources(string path)
    {
        TextAsset[] texts = Resources.LoadAll<TextAsset>(path);
        
        if (texts == null || texts.Length == 0) return false;
        else return true;
    }
}
