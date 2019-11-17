using fraction;
using UnityEngine;
using UnityEngine.UI;

public class BaseCellView : MonoBehaviour
{
    //数据
    protected Fraction _fraction;
    public Fraction Fraction
    {
        get { return _fraction; }
        set { _fraction = value; _text.text = _fraction.DecimalValue.ToString(); }
    }

    //视图
    protected Text _text;

    protected void Awake()
    {
        _text = gameObject.GetComponentInChildren<Text>();
    }
}
