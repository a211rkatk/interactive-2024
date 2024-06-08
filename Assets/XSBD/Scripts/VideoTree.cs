using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class VideoTree : MonoBehaviour
{
    [System.Serializable] class Node
    {
        [SerializeField] VideoClip _video;
        [Tooltip("Left goes first")][SerializeField] Node[] _next;

        public static VideoPlayer _player;
        static Node _root;
        static Node _current;

        public static VideoClip Proceed(bool lr)
        {
            switch (_current._next.Length)
            {
                case 0:
                    {
                        _current = _root;
                        return _current._video;
                    }
                case 1:
                    {
                        _current = _current._next[0];
                        return _current._video;
                    }
                default:
                    {
                        if (lr)
                        {
                            _current = _current._next[0];
                            return _current._video;
                        }
                        else
                        {
                            _current = _current._next[1];
                            return _current._video;
                        }
                    }
            }
        }

        public static void SetRoot(Node root)
        {
            _root = root;
            _current = root;
        }

        public static VideoClip GetRootClip()
        {
            return _root._video;
        }
    }
    [SerializeField] VideoPlayer _player;
    [SerializeField] Node _tree;
    [Space(30)]
    [SerializeField][Range(0.1f, 10)] float _slowDown;
    [SerializeField] KeyCode _keyLeft = KeyCode.LeftArrow;
    [SerializeField] KeyCode _keyRight = KeyCode.RightArrow;

    double _length;
    float _inverseSlowDown;

    // Start is called before the first frame update
    void Start()
    {
        Node.SetRoot(_tree);
        _inverseSlowDown = 1 / _slowDown;
        _player.clip = Node.GetRootClip();
        _length = _player.clip.length;
        _player.Play();
    }

    private void OnEnable()
    {
        _player.playbackSpeed = 1;
        _player.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if(_length - _player.clockTime < _slowDown)
        {
            _player.playbackSpeed = (float)(_length - _player.clockTime) * _inverseSlowDown;
            Proceed();
        }
    }

    void Proceed()
    {
        if (Input.GetKeyDown(_keyLeft))
        {
            _player.clip = Node.Proceed(true);
            _length = _player.clip.length;
            _player.playbackSpeed = 1;
            _player.Play();
        }
        else if(Input.GetKeyDown(_keyRight))
        {
            _player.clip = Node.Proceed(false);
            _length = _player.clip.length;
            _player.playbackSpeed = 1;
            _player.Play();
        }
    }
}
