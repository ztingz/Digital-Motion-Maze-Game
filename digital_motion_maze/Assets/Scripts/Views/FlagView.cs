using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlagView : MonoBehaviour,
                        IBeginDragHandler, IDragHandler, IEndDragHandler,
                        IPointerEnterHandler, IPointerExitHandler
{
    //数据
    public Dimension Dimension;
    public FlagPosition FlagPosition;
    public int Index;

    public Image Background;
    private LineRenderer _line;
    private static Color _defaultColor;
    private static Color _activeColor = Color.black;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _defaultColor = Background.color;
    }

    //开始拖曳
    public void OnBeginDrag(PointerEventData eventData)
    {
        Background.color = _activeColor;
        _line.enabled = true;
        _line.SetPosition(0, transform.position);
        // Debug.Log("开始拖曳 " + transform.position);
    }

    //拖曳中
    public void OnDrag(PointerEventData eventData)
    {
        Background.color = _activeColor;
        Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vector.z = 90;
        _line.SetPosition(1, vector);
        // Debug.Log("拖曳中 " + vector);
    }

    //结束拖曳
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.hovered.Count != 0)
        {
            foreach (GameObject o in eventData.hovered)
                if (o.name.Contains("["))
                {
                    _line.enabled = false;
                    Background.color = _defaultColor;

                    Debug.Log("from " + gameObject.name + " to " + o.name);
                    RoundxController.Instance.HandleEvent(gameObject, o);
                    return;
                }
        }
        _line.enabled = false;
        Background.color = _defaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Background.color = _activeColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Background.color = _defaultColor;
    }
}
