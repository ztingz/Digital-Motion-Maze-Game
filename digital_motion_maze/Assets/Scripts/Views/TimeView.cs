using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeView : MonoBehaviour
{
    //数据
    private const int MAX_SECOND_VALUE = 100;
    private double _second;
    public double Second
    {
        get { return _second; }
        set
        {
            if (value > MAX_SECOND_VALUE)
                _showText.text = MAX_SECOND_TEXT;
            else _showText.text = value.ToString("0");
            _second = value;
        }
    }
    private bool _isRunning = true;

    //视图
    public GameObject ShowText;
    private Text _showText;
    private const string MAX_SECOND_TEXT = "99+";

    private void Awake()
    {
        _showText = ShowText.GetComponent<Text>();
        Second = 0;
    }

    private void Update()
    {
        if (_isRunning) Second += Time.deltaTime;
    }

    public void Pause() { _isRunning = true; }

    public void GoOn() { _isRunning = true; }
}
