using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Timer : MonoBehaviour
{
    [Space(15f)]
    [Header("수정요소")]
    [SerializeField] float _slowDownTime;
    [SerializeField] float _maxAbberation;
    [Header("-----------------------------------")]
    [Space(15f)]
    [SerializeField] Material _chrabb;
    
    [Space(10f)]
    [SerializeField] Image _bar;
    [SerializeField] float _fadeTime;
    [Space(10f)]
    [SerializeField] float _minimumSpeed;
    [SerializeField] float _autoTraverseThreshold;

    [SerializeField] GameObject chrabbControl;

    public float remaining2;//디버깅용

    public static Timer _instance;
    public static bool _choice = false;
    public static bool _autoChoice = false;

    const float _inverseHalfPI = 1f;//0.63661977236f; _왜?
    public float _startSlowdown; //Measured from end of the clip
    float _inverseStartSlowdown;
    float _invOneMinusATT;
    bool _notLoop;

    public static bool _timeForSelection;

    private void Awake()
    {
        _instance = this;
        chrabbControl = GameObject.Find("Chrabb");
        chrabbControl.SetActive(false);
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
        remaining2 = VideoPlayerManager.GetRemaining(); //디버깅용

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
        _bar.fillAmount = remainRate; //(remainRate - _autoTraverseThreshold) * _invOneMinusATT;
    }

    void Countdown(float remaining)
    {
        Debug.Log(_choice);
        if (_choice)
        {
            chrabbControl.SetActive(true);
            //SetVideoSpeed(remaining);
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
        //_instance._bar.CrossFadeAlpha(1, _instance._fadeTime, false);
        _instance._bar.fillAmount = 0f;
    }
}
