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
        // Round round = ConfigManager.LoadRoundFromJsonFile(1);
        // Debug.Log("round.Multiplier=" + round.Multiplier);
        // fraction.Fraction fraction = new fraction.Fraction(2.5);
        // Debug.Log(fraction.Numerator);
        // Debug.Log(fraction.Denominator);
        // Debug.Log(fraction.DecimalValue);
        Debug.Log("R[1]".Split('[')[0]);
        Debug.Log("R[1]".Split(']')[0].Split('[')[1]);
        Debug.Log("R[1]" == "R[1]");

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
