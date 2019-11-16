using UnityEngine;
using fraction;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellView : MonoBehaviour
                        ,IBeginDragHandler, IDragHandler, IEndDragHandler
                        // ,IPointerDownHandler
{
    //数据
    private Fraction _fraction;
    public Fraction Fraction
    {
        get { return _fraction; }
        set { _fraction = value; _text.text = _fraction.DecimalValue.ToString(); }
    }
    public int RowNum;
    public int ColNum;

    //视图
    private Text _text;

    private void Awake()
    {
        _text = gameObject.GetComponentInChildren<Text>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.hovered.Count != 0)
        {
            foreach (GameObject o in eventData.hovered)
                if (o.name.Contains("[") || o.name.Contains("Canvas"))
                {
                    Debug.Log("from " + gameObject.name + " to " + o.name);
                    RoundxController.Instance.HandleEvent(gameObject, o);
                    return;
                }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log(string.Format("My name is {0}, and my position is {1}", gameObject.name, gameObject.transform.position));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
