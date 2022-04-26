using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProjectileMotion : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject TrackingPanel;
    public Text GravityAccelerationText, InitialVelocityText, InitialHeightText, LaunchAngleText, LaunchDegreeText;
    public Text InitialVelocityVxText, InitialVelocityVyText, TimeOfFlightText, TotalDistanceText, MaximumHeightText, CurrentHeightText, CurrentDistanceText, CurrentAngleText, CurrentTimeText;

    public GameObject Arrow;
    public float GravityAcceleration = 10;
    public float InitialVelocity = 10;
    public float InitialHeight = 0;
    [Range(0, 90)]
    public int LaunchAngle = 45;

    private float _initVelocityVx;
    private float _initVelocityVy;    
    private float _currentVelocityVx;
    private float _currentVelocityVy;
    private float _timeOfFlight;
    private float _totalDistance;
    private float _maxHeight;
    private float _currentHeight;
    private float _currentDistance;
    private float _currentAngle;
    private float _currentTime;

    private bool _settingUpSimulation = true;
    private bool _ranSim = false;

    // Start is called before the first frame update
    void Start()
    {
        LaunchDegreeText.transform.parent.GetComponent<Slider>().value = LaunchAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (_settingUpSimulation)
        {
            _currentAngle = LaunchAngle;
            Arrow.transform.rotation = Quaternion.Euler(0, 0, _currentAngle);
            _currentHeight = InitialHeight;
            Arrow.transform.position = new Vector3(0,_currentHeight,0);
            RefreshInitText();
        }
        else
        {
            // Run Once Started
            if (!_ranSim)
            {
                CalculateInitialVxVy();
                CalculateTotalTime();
                CalculateTotalDistance();
                CalculateMaxHeight();
                RefreshCurrentTextOnce();
                _ranSim = true;
            }

            // Keep Running After Start
            CalculateCurrentTime();
            CalculateCurrentVxVy();
            CalculateCurrentHeight();
            CalculateCurrentDistance();
            CalculateAngle();
            RefreshCurrentTextAlways();
            RefreshArrowVisuals();
        }
    }

    // Physics Methods
    // ---------------
    private void CalculateInitialVxVy()
    {
        if (LaunchAngle == 90)
        {
            _initVelocityVx = 0;
        }
        else
        {
            _initVelocityVx = InitialVelocity * Mathf.Cos(LaunchAngle * Mathf.Deg2Rad);
        }
        _initVelocityVy = InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad);
    }

    private void CalculateTotalTime()
    {
        float sqrtpart = Mathf.Abs(((InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad)) * (InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad))) + 2 * GravityAcceleration * InitialHeight);
        _timeOfFlight = (InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) + Mathf.Sqrt(sqrtpart)) / GravityAcceleration;
    }

    private void CalculateTotalDistance()
    {
        _totalDistance = _initVelocityVx * _timeOfFlight;
    }

    private void CalculateMaxHeight()
    {
        _maxHeight = InitialHeight + (InitialVelocity * InitialVelocity) * (Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad)) / (2 * GravityAcceleration);
    }

    private void CalculateCurrentTime()
    {
        if (_currentTime < _timeOfFlight)
        {
            _currentTime += Time.deltaTime;
        }
        else
        {
            _currentTime = _timeOfFlight;
        }
    }

    private void CalculateCurrentVxVy()
    {
        _currentVelocityVx = InitialVelocity * Mathf.Cos(LaunchAngle * Mathf.Deg2Rad);
        _currentVelocityVy = InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) - GravityAcceleration * _currentTime;
    }

    private void CalculateCurrentHeight()
    {
        _currentHeight =InitialHeight + InitialVelocity * _currentTime * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) - GravityAcceleration / 2 * (_currentTime * _currentTime);
        if (_currentHeight < 0)
        {
            _currentHeight = 0;
        }
    }

    private void CalculateCurrentDistance()
    {
        _currentDistance = InitialVelocity * _currentTime * Mathf.Cos(LaunchAngle * Mathf.Deg2Rad);
    }

    private void CalculateAngle()
    {
        //Vector2 tempV2 = new Vector2(_currentVelocityVx,_currentVelocityVy).normalized;
        //float tempAngle = tempV2.y * Mathf.Rad2Deg;

        //if (tempAngle > 0)
        //{
        //    _currentAngle = tempAngle + 5;
        //}
        //else
        //{
        //    _currentAngle = tempAngle - 5;
        //}

        _currentAngle = LaunchAngle * 2 - Mathf.Atan2(_currentVelocityVx, _currentVelocityVy) * Mathf.Rad2Deg;
    }


    // Visualiser Methods
    // ---------------------
    private void RefreshInitText()
    {
        GravityAccelerationText.text = "Gravity Acceleration: " + GravityAcceleration + "m/s²";
        InitialVelocityText.text = "Initial Velocity: " + InitialVelocity + "m/s";
        InitialHeightText.text = "Initial Height: " + InitialHeight + "m";
        LaunchAngleText.text = "Launch Angle: " + LaunchAngle + "°";
        LaunchDegreeText.text = _currentAngle + "°";
    }

    private void RefreshCurrentTextOnce()
    {
        InitialVelocityVxText.text = "Initial Velocity Vx: " + _initVelocityVx + "m/s";
        InitialVelocityVyText.text = "Initial Velocity Vy: " + _initVelocityVy + "m/s";
        TimeOfFlightText.text = "Time Of Flight: " + _timeOfFlight + "s";
        TotalDistanceText.text = "Total Distance: " + _totalDistance +"m";
        MaximumHeightText.text = "Maximum Height: " + _maxHeight + "m";
    }

    private void RefreshCurrentTextAlways()
    {
        CurrentHeightText.text = "Current Height: " + _currentHeight + "m";
        CurrentDistanceText.text = "Current Distance: " + _currentDistance + "m";
        CurrentAngleText.text = "Current Angle: " + _currentAngle + "°";
        CurrentTimeText.text = "Current Time: " + _currentTime + "s";
    }

    private void RefreshArrowVisuals()
    {
        Arrow.transform.position = new Vector3(_currentDistance, _currentHeight, 0);
        Arrow.transform.rotation = Quaternion.Euler(new Vector3(0,0,_currentAngle));
    }

    // Game Methods
    // ---------------
    public void StartSim()
    {
        StartPanel.SetActive(false);
        TrackingPanel.SetActive(true);
        _settingUpSimulation = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitSim()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                 Application.Quit();
#endif
    }

    // Inputs
    // ---------------
    public void AngleInput(float d)
    {
        LaunchAngle = (int)d;
    }
    public void InputG(string s)
    {
        GravityAcceleration = float.Parse(s);
    }
    public void InputV(string s)
    {
        InitialVelocity = float.Parse(s);
    }
    public void InputH(string s)
    {
        InitialHeight = float.Parse(s);
    }
}
