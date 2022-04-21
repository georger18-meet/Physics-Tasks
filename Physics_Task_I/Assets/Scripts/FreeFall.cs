using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FreeFall : MonoBehaviour
{
    public GameObject StartPanel;
    public Text GravityAccelerationText, InitialVelocityText, DropHeightText, TimeToHitText, CurrentHeightText, CurrentVelocityText, CurrentTimeText;

    public GameObject Ball;
    public GameObject BallGFX;
    public float Gravity = 10;
    public float InitVelocity = 0;
    public float Height = 5;

    private float _timeToHit;
    private float _currentTime;
    private float _currentVelocity;
    private float _currentHeight;

    private bool _initiateSimulation = false;
    private bool _ranSim = false;
    private bool _hitGround = false;

    // Start is called before the first frame update
    void Start()
    {
        StartPanel.SetActive(true);
        _initiateSimulation = false;
        _ranSim = false;
        _hitGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_initiateSimulation)
        {
            // Run Once Started
            if (!_ranSim)
            {
                Ball.transform.position = new Vector3(Ball.transform.position.x, Height, Ball.transform.position.z);
                CalculateTime();
                GravityAccelerationText.text = "Gravity Acceleration: " + Gravity + "m/s²";
                InitialVelocityText.text = "Initial Velocity: " + InitVelocity + "m/s";
                DropHeightText.text = "Drop Height: " + Height + "m";
                TimeToHitText.text = "Time To Hit: " + _timeToHit + "s";
                _currentHeight = Height;
                _ranSim = true;
            }

            // Keep Running After Start
            Fall();
            CalculateCurrentVelocity();
            CalculateCurrentHeight();
            MoveBall();
            CurrentHeightText.text = "Current Height: " + _currentHeight + "m";
            CurrentVelocityText.text = "Current Velocity: " + _currentVelocity + "m/s";
            CurrentTimeText.text = "Current Time: " + _currentTime + "s";
        }
    }

    // Physics Methods
    // ---------------
    private void CalculateTime()
    {
        float sqrtpart = Mathf.Abs(InitVelocity * InitVelocity - 4 * -(Gravity / 2) * Height);
        _timeToHit = (-InitVelocity + Mathf.Sqrt(sqrtpart)) / Gravity; /*(InitVelocity + Mathf.Sqrt((InitVelocity * InitVelocity) - 4 * -Gravity / 2 * Height)) / Gravity;*/ /*Mathf.Sqrt(Height / (Gravity / 2));*/
    }

    private void Fall()
    {
        if (_currentTime < _timeToHit)
        {
            _currentTime += Time.deltaTime;
        }
        else
        {
            _hitGround = true;
        }
    }

    private void CalculateCurrentVelocity()
    {
        _currentVelocity = InitVelocity + Gravity * _currentTime;
    }

    private void CalculateCurrentHeight()
    {
        _currentHeight = Height - ((InitVelocity * _currentTime) + ((Gravity / 2) * (_currentTime * _currentTime)));
    }

    private void MoveBall()
    {
        if (!_hitGround)
        {
            Ball.transform.position = new Vector3(Ball.transform.position.x, _currentHeight, 0);
            BallGFX.transform.Rotate(new Vector3(0, 0, -(_currentVelocity / 100)));
        }
    }


    // Game Methods
    // ---------------
    public void StartSim()
    {
        StartPanel.SetActive(false);
        _initiateSimulation = true;
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

    public void InputG(string s)
    {
        Gravity = int.Parse(s);
    }
    public void InputV(string s)
    {
        InitVelocity = int.Parse(s);
    }
    public void InputH(string s)
    {
        Height = int.Parse(s);
    }
}
