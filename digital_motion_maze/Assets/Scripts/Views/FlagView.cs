using UnityEngine;
using UnityEngine.EventSystems;

public class FlagView : MonoBehaviour,
                        IBeginDragHandler, IDragHandler, IEndDragHandler,
                        IPointerEnterHandler, IPointerExitHandler
{
    //数据
    public Dimension Dimension;
    public FlagPosition FlagPosition;
    public int Index;

    private void Start()
    {
    }

    //开始拖曳
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("开始拖曳");
        GameObject beginObj = eventData.hovered[0];
        // Debug.Log("OnBeginDrag pointerDrag:" + eventData.pointerDrag.name);
    }

    //拖曳中
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("拖曳中");
        // Debug.Log("OnDrag pointerDrag:" + eventData.pointerDrag.name);
    }

    //结束拖曳
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.hovered.Count != 0)
        {
            foreach (GameObject o in eventData.hovered)
                if (o.name.Contains("["))
                {
                    Debug.Log("from " + gameObject.name + " to " + o.name);
                    RoundxController.Instance.HandleEvent(gameObject, o);
                    return;
                }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
