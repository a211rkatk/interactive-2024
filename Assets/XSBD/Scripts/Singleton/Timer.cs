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

    public static Timer _instance;
    public static bool _choice = false;
    const float _inverseHalfPI = 0.63661977236f;
    float _startSlowdown; //Measured from end of the clip
    float _inverseStartSlowdown;

    public static bool _timeForSelection;

    private void Awake()
    {
        ApplySingleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startSlowdown = _slowDownTime * _inverseHalfPI;
        _inverseStartSlowdown = 1f / _startSlowdown;
    }

    // Update is called once per frame
    void Update()
    {
        Countdown(GetRemaining());
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
        VideoPlayerManager.SetPlaySpeed(1 - x * x);
    }

    void SetAbberation(float remainRate)
    {
        _chrabb.SetFloat("_Abberation", (1 - remainRate) * _maxAbberation);
    }

    void SetBarFill(float remainRate)
    {
        _bar.fillAmount = remainRate;
    }

    void Countdown(float remaining)
    {
        if (_choice)
        {
            SetVideoSpeed(remaining);
            SetAbberation(remaining);
            SetBarFill(remaining);
        }
        else if (remaining < 1)
        {
            _choice = true;
        }
    }

    void ApplySingleton()
    {
        if (_instance == null) _instance = this;
        else
        {
            Debug.LogError("Error: Multiple instances of Timer has been spotted!");
            Destroy(this);
        }
    }

    public static void NewVideo()
    {
        _instance.SetVideoSpeed(1.0f);
        _instance.SetAbberation(1.0f);
        _instance.SetBarFill(1.0f);
        _choice = false;
    }
}
