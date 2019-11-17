using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BGMSwitchView : MonoBehaviour, IPointerClickHandler
{
    public GameObject GlobalPrefab;
    public Sprite PlaySprite;
    public Sprite PauseSprite;

    private Image _image;
    private AudioSource _bgm;

    private void Awake()
    {
        _image = gameObject.GetComponent<Image>();

        GameObject globleObj = GameObject.Find(GlobalPrefab.name);
        if (globleObj == null)
        {
            globleObj = Instantiate(GlobalPrefab);
            globleObj.name = GlobalPrefab.name;
        }
        _bgm = globleObj.GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_bgm.isPlaying) _image.sprite = PlaySprite;
        else _image.sprite = PauseSprite;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (_bgm.isPlaying)
        {
            _bgm.Pause();
            _image.sprite = PauseSprite;
        }
        else
        {
            _bgm.Play();
            _image.sprite = PlaySprite;
        }
    }
}
