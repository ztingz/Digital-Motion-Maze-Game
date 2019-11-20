using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellView : BaseCellView,
                        IBeginDragHandler, IDragHandler, IEndDragHandler,
                        IPointerEnterHandler, IPointerExitHandler
{
    //数据
    public int RowNum;
    public int ColNum;

    public Image Background;
    public Text Text;
    private LineRenderer _line;
    private static Color _defaultColor;
    private static Color _activeColor = new Color(82.0F / 255, 97.0F / 255, 106.0F / 255);

    private new void Awake()
    {
        base.Awake();
        _line = GetComponent<LineRenderer>();
        _defaultColor = Background.color;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Background.color = _activeColor;
        _line.enabled = true;
        _line.SetPosition(0, transform.position);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Background.color = _activeColor;
        Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vector.z = 90;
        _line.SetPosition(1, vector);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.hovered.Count != 0)
        {
            foreach (GameObject o in eventData.hovered)
                if (o.name.Contains("["))
                {
                    _line.enabled = false;
                    Background.color = Color.black;
                    Debug.Log("from " + gameObject.name + " to " + o.name);
                    RoundxController.Instance.HandleEvent(gameObject, o);
                    return;
                }
        }
        _line.enabled = false;
        Background.color = Color.black;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Background.color = _activeColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Background.color = Color.black;
    }
}
