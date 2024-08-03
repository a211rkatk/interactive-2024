using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Timer : MonoBehaviour
{
    [SerializeField] float _slowDownTime;
    [Space(10f)]
    [SerializeField] Material _chrabb;
    [SerializeField] float _maxAbberation;
    [Space(10f)]
    [SerializeField] Image _bar;
    [SerializeField] float _fadeTime;
    [Space(10f)]
    [SerializeField] float _minimumSpeed;
    [SerializeField] float _autoTraverseThreshold;

    public static Timer _instance;
    public static bool _choice = false;
    public static bool _autoChoice = false;

    const float _inverseHalfPI = 0.63661977236f;
    float _startSlowdown; //Measured from end of the clip
    float _inverseStartSlowdown;
    float _invOneMinusATT;
    bool _notLoop;

    public static bool _timeForSelection;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startSlowdown = _slowDownTime * _inverseHalfPI;
        _inverseStartSlowdown = 1f / _startSlowdown;
        _invOneMinusATT = 1f / (1 - _autoTraverseThreshold);

        _notLoop = !VideoPlayerManager.GetPlayer().isLooping;
    }

    // Update is called once per frame
    void Update()
    {
        if(_notLoop) Countdown(GetRemaining());
    }

    float GetRemaining()
    {
        float remaining = VideoPlayerManager.GetRemaining();

        if (remaining < _startSlowdown) return remaining * _inverseStartSlowdown;
        else return 1;
    }

    void SetVideoSpeed(float remainRate)
    {
        float x = 1 - remainRate;
        x = 1 - x * x;
        VideoPlayerManager.SetPlaySpeed((x < _minimumSpeed ? _minimumSpeed : x));
    }

    void SetAbberation(float remainRate)
    {
        _chrabb.SetFloat("_Abberation", (1 - remainRate) * _maxAbberation);
    }

    void SetBarFill(float remainRate)
    {
        _bar.fillAmount = (remainRate - _autoTraverseThreshold) * _invOneMinusATT;
    }

    void Countdown(float remaining)
    {
        if (_choice)
        {
            SetVideoSpeed(remaining);
            SetAbberation(remaining);
            SetBarFill(remaining);
            
            if(remaining < _autoTraverseThreshold) _autoChoice = true;
        }
        else if (remaining < 1)
        {
            _choice = true;
        }
    }

    public static void NewVideo()
    {
        _instance.SetVideoSpeed(1.0f);
        _instance.SetAbberation(1.0f);
        _instance.SetBarFill(1.0f);
        _choice = false;
        _autoChoice = false;
        _instance._bar.CrossFadeAlpha(0, _instance._fadeTime, false);
    }
}