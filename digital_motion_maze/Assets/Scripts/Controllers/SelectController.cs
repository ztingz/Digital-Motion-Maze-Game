using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    //数据
    private int _roundIndex;
    public static int SelectNum = 1;

    //场景物体
    public GameObject RoundRoot;

    //关卡格子预制体
    public GameObject RoundNumberPFB;

    private void Awake() { _roundIndex = ConfigManager.LoadRoundNumber(); }

    private void Start() { Draw(); }

    #region 方法
    private void Draw()
    {
        GameObject g;
        for (int i = 0; i < _roundIndex; ++i)
        {
            g = Instantiate(RoundNumberPFB, RoundRoot.GetComponent<RectTransform>());
            int num = i + 1;
            g.name = (num).ToString();
            g.GetComponent<BaseCellView>().Fraction = new fraction.Fraction(num);
            g.GetComponent<Button>().onClick.AddListener(() => ToRound(num));
        }
    }
    public void ToRound(int num)
    {
        Debug.Log(num);
        SelectNum = num;
        SceneManager.LoadScene("Roundx");
    }
    #endregion
}
