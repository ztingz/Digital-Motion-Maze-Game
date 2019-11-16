using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepView : MonoBehaviour
{
    //数据
    private const int MAX_STEP_VALUE = 100;
    private int _step;
    public int Step
    {
        get { return _step; }
        set
        {
            if (value > MAX_STEP_VALUE)
                _showText.text = MAX_STEP_TEXT;
            else _showText.text = value.ToString("0");
            _step = value;
        }
    }

    //视图
    public GameObject ShowText;
    private Text _showText;
    private const string MAX_STEP_TEXT = "99+";

    private void Awake()
    {
        _showText = ShowText.GetComponent<Text>();
        Step = 0;
    }

    public void increase() { ++Step; }
    public void decrease() { --Step; }
}
