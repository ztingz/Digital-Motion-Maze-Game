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
    }

    //拖曳中
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("拖曳中");
        //         //获取到鼠标的位置(鼠标水平的输入和竖直的输入以及距离)
        //         Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        //         //物体的位置，屏幕坐标转换为世界坐标
        //         Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //         //把鼠标位置传给物体
        //         transform.position = objectPosition;
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
