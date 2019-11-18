using UnityEngine;
using UnityEngine.UI;

public class AccountsDialogView : MonoBehaviour
{
    //数据
    public Record Record;

    public Image Star1;
    public Image Star2;
    public Image Star3;
    public Text DetailText;
    public Button NextOrEndButton;
    public Text NextOrEnd;
    public Button Reset;

    public Sprite ActiveStar;
    public Sprite DisActiveStar;

    private const string ACCOUNTS_DETAIL = "√ 通过关卡 +1★\n{0} {1}步内过关 +1★\n{2} {3}秒内过关 +1★";


    private void Awake()
    {
        gameObject.SetActive(false);
        Reset.onClick.AddListener(() => GlobalController.SToRound(RoundxController.SelectRoundIndex));
        if (RoundxController.SelectRoundIndex == ConfigManager.LoadRoundNumber())
        {
            NextOrEnd.text = "返回主菜单";
            NextOrEndButton.onClick.AddListener(() => GlobalController.SToSelect());
        }
        else NextOrEndButton.onClick.AddListener(() => GlobalController.SToRound(++RoundxController.SelectRoundIndex));
    }

    public void SetInfo(Round round, Record record)
    {
        DetailText.text = string.Format(
            ACCOUNTS_DETAIL, round.StarStep >= record.Step ? "√" : "×", round.StarStep, round.StarTime >= record.Time ? "√" : "×", round.StarTime);
        if (record.StarNumber == 3)
        {
            Star1.sprite = ActiveStar;
            Star2.sprite = ActiveStar;
            Star3.sprite = ActiveStar;
            return;
        }
        if (record.StarNumber == 2)
        {
            Star1.sprite = ActiveStar;
            Star2.sprite = ActiveStar;
            return;
        }
        if (record.StarNumber == 1)
        {
            Star1.sprite = ActiveStar;
            return;
        }
    }
}
