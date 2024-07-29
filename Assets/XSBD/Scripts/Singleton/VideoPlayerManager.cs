using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField] VideoPlayer _player;
    [SerializeField] AudioSource _audioSource;

    static UnityEvent _onLoadNewVideo;
    static VideoPlayerManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            print(name);
        }
        else
        {
            Debug.LogError("Error: Multiple instances of VideoPlayerManager has been spotted!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static VideoPlayerManager Instance()
    {
        if (_instance != null) return _instance;
        else
        {
            Debug.LogError("NRE: no instance of videoplayermanager has been created");
            return null;
        }
    }

    public static VideoPlayer GetPlayer()
    {
        return Instance()._player;
    }

    public static float GetRemaining()
    {
        return (float)(Instance()._player.clip.length - Instance()._player.clockTime);
    }

    public static void SetPlaySpeed(float speed)
    {
        Instance()._player.playbackSpeed = speed;
        Instance()._audioSource.pitch = speed;
    }

    public static void SetVideo(VideoClip clip)
    {
        Instance()._player.clip = clip;

        Timer.NewVideo();
    }

    public static void ListenToSetVideo(UnityAction action)
    {
        _onLoadNewVideo.AddListener(action);
    }
}
