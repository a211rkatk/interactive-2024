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
        if (_instance == null) _instance = this;
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

    public static VideoPlayer GetPlayer()
    {
        return _instance._player;
    }

    public static float GetRemaining()
    {
        return (float)(_instance._player.clip.length - _instance._player.time);
    }

    public static void SetPlaySpeed(float speed)
    {
        _instance._player.playbackSpeed = speed;
        _instance._audioSource.pitch = speed;
    }

    public static void SetVideo(VideoClip clip)
    {
        _instance._player.clip = clip;

        Timer.NewVideo();
    }

    public static void ListenToSetVideo(UnityAction action)
    {
        _onLoadNewVideo.AddListener(action);
    }
}
