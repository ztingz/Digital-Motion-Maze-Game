using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BGMSwitchView : MonoBehaviour, IPointerClickHandler
{
    public GameObject GlobalPrefab;
    public Sprite PlaySprite;
    public Sprite PauseSprite;

    private Image _image;

    private void Awake()
    {
        _image = gameObject.GetComponent<Image>();

        GameObject globleObj = GameObject.Find(GlobalPrefab.name);
        if (globleObj == null)
        {
            globleObj = Instantiate(GlobalPrefab);
            globleObj.name = GlobalPrefab.name;
        }
    }

    private void Start()
    {
        if (GlobalController.BGMPlay) _image.sprite = PlaySprite;
        else _image.sprite = PauseSprite;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (GlobalController.BGMPlay)
        {
            GlobalController.BGMPlay = false;
            _image.sprite = PauseSprite;
        }
        else
        {
            GlobalController.BGMPlay = true;
            _image.sprite = PlaySprite;
        }
    }
}
