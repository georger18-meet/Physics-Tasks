using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProjectileMotion : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject TimeScaleSlider;
    public GameObject TrackingPanel;
    public Text TimeScaleText;
    public Text GravityAccelerationText, InitialVelocityText, InitialHeightText, LaunchAngleText, LaunchDegreeText;
    public Text InitialVelocityVxText, InitialVelocityVyText, TimeOfFlightText, TotalDistanceText, MaximumHeightText, CurrentHeightText, CurrentDistanceText, CurrentAngleText, CurrentTimeText;
    public GameObject Arrow;

    // Variables
    [Header("Variables")]
    public float GravityAcceleration = 10;
    public float InitialVelocity = 10;
    public float InitialHeight = 0;
    [Range(0, 90)]
    public int LaunchAngle = 45;

    // External Factors Start
    [Header("External Factors")]
    public Text WindVelocityText;
    public Text WindAngleText;
    public GameObject WindAngleIndicator;
    [Range(0, 360)]
    public int WindAngle = 0;
    public float WindVelocity = 0;

    private float _windVelocityVx;
    private float _windVelocityVy;
    // External Factors End

    // Gamificacion Start
    [Header("Gamificacion")]
    public bool MiniGameMode = false;
    public Text GameModeButtonText, WinText, TargetDistanceText;
    public GameObject Target, WinPanel;
    public Vector3 TargetOffset;
    public int MaxDistanceRange;
    public bool TargetHitWin;

    private int _targetDistance;
    private bool _targetSet;
    // Gamificacion End

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

    private float _timeScaleVal;

    private bool _settingUpSimulation = true;
    private bool _ranSim = false;
    private bool _reachedEndOfFlight;


    // Start is called before the first frame update
    void Start()
    {
        // Setting The Slider UI & WinPanel False
        LaunchDegreeText.transform.parent.GetComponent<Slider>().value = LaunchAngle;
        WinPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Checking If MiniGame Or PlayGround
        CheckGameMode();

        // Keeps Running While Setting Up Simulation Parameters (Gravity, Velocity, Angle, etc...)
        if (_settingUpSimulation)
        {
            // Refreshing Arrow Location According To LaunchAngle & InitialHeight
            _currentAngle = LaunchAngle;
            Arrow.transform.rotation = Quaternion.Euler(0, 0, _currentAngle);
            _currentHeight = InitialHeight;
            Arrow.transform.position = new Vector3(0, _currentHeight, 0);
            
            // Refreshing Initial Values UI
            RefreshInitText();

            // External Factors
            RefreshWindIndicator();
        }
        // Keeps Running After Clicking The 'Start' Button On The Sim
        else
        {
            // Run Only Once When Sim Is Started (Variables To Calculate ONCE)
            if (!_ranSim)
            {
                CalculateInitialVxVy();
                CalculateTotalTime();
                CalculateTotalDistance();
                CalculateMaxHeight();
                RefreshCurrentTextOnce();

                _ranSim = true;
            }

            // Keep Running After Start (Variables To Calculate As Long As The Arrow Is Flying)
            CalculateCurrentTime();
            CalculateCurrentVxVy();
            CalculateCurrentHeight();
            CalculateCurrentDistance();
            CalculateAngle();
            RefreshCurrentTextAlways();
            RefreshArrowVisuals();

            // Ending & Winning Checkers
            CheckReachedEnd();
            DidItHit();
        }
    }

    // Physics Methods
    // ---------------
    private void CalculateInitialVxVy()
    {
        // Calculate Wind VxVy First To Add To InitVelocity
        CalculateWindVxVy();

        // Calculating InitVx -- OGFormula: Vx = V * cos(a)
        if (LaunchAngle == 90)
        {
            _initVelocityVx = 0;
        }
        else
        {
            _initVelocityVx = InitialVelocity * Mathf.Cos(LaunchAngle * Mathf.Deg2Rad) + _windVelocityVx;
        }

        // Calculating InitVy -- OGFormula: Vy = V * sin(a)
        _initVelocityVy = InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) + _windVelocityVy;
    }

    private void CalculateTotalTime()
    {
        // Calculating The SquareRoot Part Separately
        float sqrtpart = Mathf.Abs(((InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) + _windVelocityVy) * (InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) + _windVelocityVy)) + 2 * GravityAcceleration * InitialHeight);

        // Time Of Flight Math -- OGFormula: t = [V? * sin(?) + ?((V? * sin(?))² + 2 * g * h)] / g
        if (LaunchAngle == 0 && GravityAcceleration == 0)
        {
            _timeOfFlight = float.PositiveInfinity;
        }
        else
        {
            _timeOfFlight = (InitialVelocity * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) + _windVelocityVy + Mathf.Sqrt(sqrtpart)) / GravityAcceleration;
        }
    }

    private void CalculateTotalDistance()
    {
        // Calculating Total X Distance -- OGFormula: x = Vx * t
        _totalDistance = _initVelocityVx * _timeOfFlight;
    }

    private void CalculateMaxHeight()
    {
        // Calculating Max Height On Y Axes -- OGFormula: hMax = hStart + Vy² / 2g
        if (LaunchAngle == 0 && GravityAcceleration == 0)
        {
            _maxHeight = InitialHeight;
        }
        else
        {
            _maxHeight = InitialHeight + (_initVelocityVy * _initVelocityVy)/*(InitialVelocity * InitialVelocity) * (Mathf.Sin(LaunchAngle * Mathf.Deg2Rad) * Mathf.Sin(LaunchAngle * Mathf.Deg2Rad))*/ / (2 * GravityAcceleration);
        }
    }

    private void CalculateCurrentTime()
    {
        // Calculating Current Time
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
        // Calculating Current VxVy
        // Vx Is Constant Since No External Factors Affect It (Acceleration = 0)
        _currentVelocityVx = _initVelocityVx;
        // Vy Is Affected By Gravity Across Time
        _currentVelocityVy = _initVelocityVy - GravityAcceleration * _currentTime;
    }

    private void CalculateCurrentHeight()
    {
        // Calculating Current Height On Y -- OGFormula: y = Vy * t - g / 2 * t²
        _currentHeight = InitialHeight + _initVelocityVy * _currentTime - GravityAcceleration / 2 * (_currentTime * _currentTime);
        if (_currentHeight < 0)
        {
            _currentHeight = 0;
        }
    }

    private void CalculateCurrentDistance()
    {
        // Calculating Current Distance On X -- OGFormula: x = Vx * t
        _currentDistance = _initVelocityVx * _currentTime;
    }

    private void CalculateAngle()
    {
        // Calculating Current Angle Of Projectile (Using the Tanget of currentVx & currentVy)
        if (LaunchAngle == 0)
        {
            _currentAngle = 0;
        }
        else
        {
            _currentAngle = 90 - Mathf.Atan2(_currentVelocityVx, _currentVelocityVy) * Mathf.Rad2Deg;
        }
    }

    private void CheckReachedEnd()
    {
        // Checking If Reached End Of Flight
        if (_currentDistance == _totalDistance && _currentTime == _timeOfFlight)
        {
            _reachedEndOfFlight = true;
        }
    }

    // External Factors
    private void CalculateWindVxVy()
    {
        // Calculating Wind Vx & Vy (Using WindVelocity & WindAngle)
        if (WindAngle == 360 || WindAngle == 180)
        {
            _windVelocityVx = WindVelocity * Mathf.Cos(WindAngle * Mathf.Deg2Rad);
            _windVelocityVy = 0;
        }
        else if (WindAngle == 90 || WindAngle == 270)
        {
            _windVelocityVx = 0;
            _windVelocityVy = WindVelocity * Mathf.Sin(WindAngle * Mathf.Deg2Rad);
        }
        else
        {
            _windVelocityVx = WindVelocity * Mathf.Cos(WindAngle * Mathf.Deg2Rad);
            _windVelocityVy = WindVelocity * Mathf.Sin(WindAngle * Mathf.Deg2Rad);
        }
    }


    // Visualiser Methods
    // ---------------------
    private void RefreshInitText()
    {
        // Refreshing Initial Values Text
        GravityAccelerationText.text = "Gravity Acceleration: " + GravityAcceleration + "m/s²";
        InitialVelocityText.text = "Initial Velocity: " + InitialVelocity + "m/s";
        InitialHeightText.text = "Initial Height: " + InitialHeight + "m";
        LaunchAngleText.text = "Launch Angle: " + LaunchAngle + "°";
        LaunchDegreeText.text = _currentAngle + "°";

        // External Factors
        WindVelocityText.text = "Wind Velocity: " + WindVelocity + "m/s";
        WindAngleText.text = "Wind Angle: " + WindAngle + "°";

        // Gamificacion
        TargetDistanceText.text = "Target Distance: " + _targetDistance + "m";
    }

    private void RefreshCurrentTextOnce()
    {
        // Refreshing Current Text For Values That Get Calculated Once
        InitialVelocityVxText.text = "Initial Velocity Vx: " + _initVelocityVx + "m/s";
        InitialVelocityVyText.text = "Initial Velocity Vy: " + _initVelocityVy + "m/s";
        TimeOfFlightText.text = "Time Of Flight: " + _timeOfFlight + "s";
        TotalDistanceText.text = "Total Distance: " + _totalDistance + "m";
        MaximumHeightText.text = "Maximum Height: " + _maxHeight + "m";
    }

    private void RefreshCurrentTextAlways()
    {
        // Refreshing Current Text For Always Calculated Values As Long As Projectile Flies
        CurrentHeightText.text = "Current Height: " + _currentHeight + "m";
        CurrentDistanceText.text = "Current Distance: " + _currentDistance + "m";
        CurrentAngleText.text = "Current Angle: " + _currentAngle + "°";
        CurrentTimeText.text = "Current Time: " + _currentTime + "s";
    }

    private void RefreshTimeScaleText(float tScale)
    {
        // Time Scale Text Refresh
        Time.timeScale = tScale;
        _timeScaleVal = tScale;
        TimeScaleText.text = _timeScaleVal.ToString("F2") + "x";
    }

    private void RefreshArrowVisuals()
    {
        // Refreshing Projectile Position & Rotation To Visualise Motion
        Arrow.transform.position = new Vector3(_currentDistance, _currentHeight, 0);
        Arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _currentAngle));
    }

    // External Factors
    private void RefreshWindIndicator()
    {
        // Refreshing Wind Angle Indicator UI
        WindAngleIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, WindAngle));
    }


    // Gamificacion
    // ---------------
    public void ToggleGameMode()
    {
        // Accessed From ToggelGameMode Button
        if (MiniGameMode)
        {
            GameModeButtonText.text = "PLAYGROUND\nMODE";
            MiniGameMode = false;
        }
        else
        {
            GameModeButtonText.text = "MINIGAME\nMODE";
            MiniGameMode = true;
        }
    }

    private void CheckGameMode()
    {
        // What Happens With Specific GameMode
        if (MiniGameMode)
        {
            Target.SetActive(true);
            TargetDistanceText.gameObject.SetActive(true);
            SendTargetToDestination();
        }
        else
        {
            _targetSet = false;
            Target.SetActive(false);
            TargetDistanceText.gameObject.SetActive(false);
        }
    }

    private void SendTargetToDestination()
    {
        // Responsible Of Setting Target To Random Location According To Max Distance
        if (!_targetSet)
        {
            _targetDistance = Random.Range(5, MaxDistanceRange + 1);
            Vector3 targetDest = new Vector3(_targetDistance, 0, 0) + TargetOffset;
            Target.transform.position = targetDest;
            _targetSet = true;
        }
    }

    private void DidItHit()
    {
        // Using New True Collision
        if (MiniGameMode && _reachedEndOfFlight && Arrow.GetComponent<CustomBoxCollider>().WasTriggered && Arrow.GetComponent<CustomBoxCollider>().ObjCollidedWithRef.gameObject == Target)
        {
            TargetHitWin = true;
            WinPanel.SetActive(true);
            WinText.color = Color.green;
            WinText.text = "MATH WIZARD!?";
        }
        else if (MiniGameMode && _reachedEndOfFlight && !TargetHitWin)
        {
            WinPanel.SetActive(true);
            WinText.color = Color.red;
            WinText.text = "GIT GUD";
        }

        // Using Old "Fake Collision"
        //if (MiniGameMode && _reachedEndOfFlight && _currentDistance >= _targetDistance - 3 && _currentDistance <= _targetDistance + 3)
        //{
        //    TargetHitWin = true;
        //    WinPanel.SetActive(true);
        //    WinText.color = Color.green;
        //    WinText.text = "MATH WIZARD!?";
        //}
        //else if (MiniGameMode && _reachedEndOfFlight && !TargetHitWin)
        //{
        //    WinPanel.SetActive(true);
        //    WinText.color = Color.red;
        //    WinText.text = "GIT GUD";
        //}
    }


    // Game Methods
    // ---------------
    public void StartSim()
    {
        TimeScaleSlider.transform.SetParent(TimeScaleSlider.transform.parent.parent);
        StartPanel.SetActive(false);
        TrackingPanel.SetActive(true);
        _settingUpSimulation = false;
    }

    public void Restart()
    {
        RefreshTimeScaleText(1f);
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

    // UI Inputs
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

    // External Factors UI Inputs
    public void WindAngleInput(float d)
    {
        WindAngle = (int)d;
    }
    public void WindAngleInput(string s)
    {
        WindAngle = int.Parse(s);
    }
    public void InputWindV(string s)
    {
        WindVelocity = float.Parse(s);
    }

    // Gamificacion UI Inputs
    public void InputMaxTargetDistance(string d)
    {
        MaxDistanceRange = (int)float.Parse(d);
    }

    public void TimeScaleInput(float t)
    {
        RefreshTimeScaleText(t);
    }
}
