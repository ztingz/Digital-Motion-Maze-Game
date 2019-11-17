using UnityEngine;
using UnityEngine.EventSystems;

public class CellView : BaseCellView
                        , IBeginDragHandler, IDragHandler, IEndDragHandler
// ,IPointerDownHandler
{
    //数据
    public int RowNum;
    public int ColNum;

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

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData) { }
}
