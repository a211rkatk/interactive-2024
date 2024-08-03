using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTree : MonoBehaviour
{
    [Tooltip("Starts with the video stored in the folder directly under Resources which has this name\nResources 폴더 바로 아래 있는 폴더들 중 이 이름을 가진 폴더에서 시작")]
    [SerializeField] string _beginAt = "";
    [SerializeField] bool _resetRandomProcessionCountOnStart = false;

    [Space(30f)]
    //Debug buttons
    [SerializeField] bool _branchA;
    [SerializeField] bool _branchB;

    VideoClip _currentClip;
    string _path;
    static uint _randomProcessionCount = 0;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_resetRandomProcessionCountOnStart) _randomProcessionCount = 0;
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

    public void RandomProceed()
    {
        ++_randomProcessionCount;
        Proceed(Random.value > 0.5f);
    }

    public void Proceed(bool AB)
    {
        //traverse to folder first
        Traverse(AB);

        //if there is a video, play that, and if not, get instructions.
        if (GetNextClip()) VideoPlayerManager.SetVideo(_currentClip);
        else DoAsTextSays();
    }

    void HandleDeadEndException(string message = "Tree met a dead end. Reloading Scene.")
    {
        Debug.LogError(message);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                    HandleDeadEndException();
                }
            }
        }
    }

    bool GetNextClip()
    {
        VideoClip[] _clips = Resources.LoadAll<VideoClip>(_path);
        if (null == _clips || _clips.Length == 0) return false;

        _currentClip = _clips[^1];
        return true;
    }

    bool Branch(string next)
    {
        if (!FolderExistsUnderResources(_path + "/" + next)) return false;

        _path += "/" + next;
        return true;
    }

    bool RetraverseBranch()
    {
        if(!FolderExistsUnderResources(ParentDirectory(_path) + "/Y")) return false;

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
        else
        {
            HandleDeadEndException("New root could not be found under Assets/Resources! New Root: " + newRoot);
        }
    }

    static string ParentDirectory(string path)
    {
        int lastIndexOfSlash = path.LastIndexOf("/");
        return path.Substring(0, (lastIndexOfSlash == -1 ? 0 : lastIndexOfSlash));
    }

    bool FolderExistsUnderResources(string path)
    {
        Object[] objects = Resources.LoadAll(path);
        
        if (objects == null || objects.Length == 0) return false;
        else return true;
    }

    /*
    string GetFileOfExtensionAtPath(string extension) // returns file name without extension at _path.
    {
        string[] paths = Directory.GetFiles(_resourcesPath + _path, "*." + extension);

        try { return Path.GetFileNameWithoutExtension(paths[0]); }
        catch { return ""; }
    }
    */
}
