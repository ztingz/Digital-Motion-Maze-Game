using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectController : MonoBehaviour
{
    //数据
    private List<Record> _roundRecords;
    private int _roundNum;

    //场景物体
    public RectTransform RoundRoot;
    private List<GameObject> _rounds;

    //关卡格子预制体
    public GameObject RoundNumberPFB;

    private void Awake()
    {
        _roundNum = ConfigManager.LoadRoundNumber();
        _roundRecords = ConfigManager.LoadRecords();
        _rounds = new List<GameObject>();
    }

    private void Start()
    {
        Draw();
    }

    #region 方法
    private void Draw()
    {
        int unLockIndex = 0;
        if (_roundRecords != null)
            //已解锁关卡
            foreach (Record record in _roundRecords)
            {
                _rounds.Add(Instantiate(RoundNumberPFB, RoundRoot));
                _rounds[_roundRecords.IndexOf(record)].name = (record.RoundIndex + 1).ToString();
                _rounds[_roundRecords.IndexOf(record)].GetComponent<RoundCellView>().Record = record;
                _rounds[_roundRecords.IndexOf(record)].GetComponent<Button>().onClick.AddListener(() => GlobalController.SToRound(record.RoundIndex));
                _rounds[_roundRecords.IndexOf(record)].GetComponent<RoundCellView>().Mask.SetActive(false);
                ++unLockIndex;
            }
        _rounds.Add(Instantiate(RoundNumberPFB, RoundRoot));
        _rounds[unLockIndex].name = (unLockIndex + 1).ToString();
        _rounds[unLockIndex].GetComponent<RoundCellView>().Record = new Record(unLockIndex);
        int index = unLockIndex;
        _rounds[unLockIndex].GetComponent<Button>().onClick.AddListener(() => GlobalController.SToRound(index));
        _rounds[unLockIndex].GetComponent<RoundCellView>().Mask.SetActive(false);
        ++unLockIndex;
        //未解锁关卡
        for (int i = unLockIndex; i < _roundNum; ++i)
        {
            _rounds.Add(Instantiate(RoundNumberPFB, RoundRoot));
            _rounds[i].name = (i + 1).ToString();
            _rounds[i].GetComponent<RoundCellView>().Record = new Record(i);
            _rounds[i].GetComponent<RoundCellView>().Mask.SetActive(true);
        }
    }
    #endregion
}
