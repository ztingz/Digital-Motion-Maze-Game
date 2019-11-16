using System;
using System.Collections.Generic;
using fraction;
using UnityEngine;

public enum FlagPosition : byte { L = 0, R = 1, U = 2, D = 3 }

public class RoundxController : MonoSingleton<RoundxController>
{
    #region 属性

    //布局信息
    private const int ELEMENTS_PANEL_SIZE = 800;//面板大小
    private const float ELEMENT_RATIO = 0.92f;//占总的比率
    private int _layoutOrder;//布局数量=Order
    private int _cellSize;//大小
    private int _flagSize;//行列标记大小
    private int _gapSize;//间隙大小
    private int _moveUnit;//移动步长
    private Vector2 _initDistance;//初始偏移量

    //场景物体
    public GameObject Title;
    public GameObject Time;
    public GameObject Step;
    public GameObject CellRoot;
    public GameObject FlagRoot;
    public GameObject NumRoot;

    //预制体
    public GameObject CellPFB;//格子预制体
    public GameObject FlagPFB;//行列标记预制体

    //关卡数据
    private Round _round;//只用于初始化关卡, 不更新
    public const string CELL_KEY = "[{0},{1}]";//e.g.[0,0]
    public const string FLAG_KEY = "{0}[{1}]";//e.g.R[0]
    public const string NUM_KEY = "N[{0}]";//e.g.R[0]
    private Dictionary<string, GameObject> _cells;//格子字典
    private Dictionary<string, GameObject> _flags;//标记字典
    protected List<GameObject> _nums;//乘数列表
    #endregion

    protected virtual void Awake()
    {
        _round = ConfigManager.LoadRoundFromJsonFile(3);
        _cells = new Dictionary<string, GameObject>();
        _flags = new Dictionary<string, GameObject>();
        _nums = new List<GameObject>();
    }

    private void Start()
    {
        Draw(_round.Matrix.Order, true);
        DrawNums();
    }

    //计算布局
    private void InitLayout(int order)
    {
        _layoutOrder = order;
        float one = ELEMENTS_PANEL_SIZE / _layoutOrder;
        _cellSize = (int)Math.Round(one * ELEMENT_RATIO);
        _gapSize = (int)Math.Round((one - _cellSize) / 2);
        _flagSize = (int)FlagPFB.GetComponentInChildren<RectTransform>().sizeDelta.x;
        _flagSize = Math.Min(_flagSize, _cellSize / 2);
        _moveUnit = _cellSize + 2 * _gapSize; ;
        _initDistance = new Vector2(-(_layoutOrder / 2) * _moveUnit, (_layoutOrder / 2) * _moveUnit);
    }

    #region 方法
    private void Draw(int order, bool needInstantiate)
    {
        InitLayout(order);
        DrawFlags(needInstantiate);
        DrawCells(needInstantiate);
    }

    private void DrawCells(bool needInstantiate)
    {
        for (byte i = 0; i < _layoutOrder; ++i)
        {
            for (byte j = 0; j < _layoutOrder; ++j)
            {
                string cellKey = string.Format(CELL_KEY, i, j);
                if (needInstantiate)
                {
                    _cells.Add(cellKey, Instantiate(CellPFB, CellRoot.GetComponent<RectTransform>()));//实例化预制体
                    _cells[cellKey].name = cellKey;

                    _cells[cellKey].GetComponent<CellView>().Fraction = new Fraction(_round.Matrix.Content[i, j]);//设置格子内容
                    _cells[cellKey].GetComponent<CellView>().RowNum = i;
                    _cells[cellKey].GetComponent<CellView>().ColNum = j;
                }

                //设置格子大小
                foreach (RectTransform rt in _cells[cellKey].GetComponentsInChildren<RectTransform>())
                    rt.sizeDelta = new Vector2(_cellSize, _cellSize);

                //设置格子位置
                Vector2 vector = new Vector2(j * _moveUnit, -i * _moveUnit) + _initDistance;
                if (_layoutOrder % 2 == 0) vector += new Vector2(_moveUnit / 2, -_moveUnit / 2);
                _cells[cellKey].GetComponent<Transform>().localPosition = vector;
            }
        }
    }

    private void DrawFlags(bool needInstantiate)
    {
        if (needInstantiate)
            foreach (FlagPosition flagPosition in Enum.GetValues(typeof(FlagPosition)))
            {
                for (int j = 0; j < _layoutOrder; ++j)
                {
                    string flagKey = string.Format(FLAG_KEY, Enum.GetName(flagPosition.GetType(), flagPosition), j);
                    _flags.Add(flagKey, Instantiate(FlagPFB, FlagRoot.GetComponent<RectTransform>()));
                    _flags[flagKey].name = flagKey;

                    if (flagPosition == FlagPosition.L || flagPosition == FlagPosition.R)
                        _flags[flagKey].GetComponent<FlagView>().Dimension = Dimension.ROW;
                    else _flags[flagKey].GetComponent<FlagView>().Dimension = Dimension.COL;

                    _flags[flagKey].GetComponent<FlagView>().FlagPosition = flagPosition;
                    _flags[flagKey].GetComponent<FlagView>().Index = j;
                    foreach (RectTransform rt in _flags[flagKey].GetComponentsInChildren<RectTransform>())
                        rt.sizeDelta = new Vector2(_flagSize, _flagSize);
                }
            }

        for (byte i = 0; i < _layoutOrder; ++i)
        {
            for (byte j = 0; j < _layoutOrder; ++j)
            {
                //设置位置
                Vector2 vector = new Vector2(j * _moveUnit, -i * _moveUnit) + _initDistance;
                if (_layoutOrder % 2 == 0) vector += new Vector2(_moveUnit / 2, -_moveUnit / 2);

                if (j == 0)//Left
                    _flags[string.Format(FLAG_KEY, FlagPosition.L.ToString(), i)].GetComponent<Transform>().localPosition
                    = vector - new Vector2((_cellSize + _flagSize) / 2 + _gapSize, 0);
                if (j == _layoutOrder - 1)//Right
                    _flags[string.Format(FLAG_KEY, FlagPosition.R.ToString(), i)].GetComponent<Transform>().localPosition
                    = vector + new Vector2((_cellSize + _flagSize) / 2 + _gapSize, 0);
                if (i == 0)//Up
                    _flags[string.Format(FLAG_KEY, FlagPosition.U.ToString(), j)].GetComponent<Transform>().localPosition
                    = vector + new Vector2(0, (_cellSize + _flagSize) / 2 + _gapSize);
                if (i == _layoutOrder - 1)//Down
                    _flags[string.Format(FLAG_KEY, FlagPosition.D.ToString(), j)].GetComponent<Transform>().localPosition
                    = vector - new Vector2(0, (_cellSize + _flagSize) / 2 + _gapSize);
            }
        }
    }

    private void DrawNums()
    {
        foreach (double m in _round.Multiplier)
        {
            GameObject temp = Instantiate(CellPFB, NumRoot.GetComponent<RectTransform>());
            temp.name = string.Format(NUM_KEY, m);
            temp.GetComponent<CellView>().Fraction = new Fraction(m);
            _nums.Add(temp);
        }
    }

    private (Dimension dimension, FlagPosition position, int index) PraseFlag(GameObject g)
    {
        return (g.GetComponent<FlagView>().Dimension, g.GetComponent<FlagView>().FlagPosition, g.GetComponent<FlagView>().Index);
    }

    private (int row, int col) PraseCell(GameObject g)
    {
        return (g.GetComponent<CellView>().RowNum, g.GetComponent<CellView>().ColNum);
    }

    public void HandleEvent(GameObject a, GameObject b)
    {
        if (a == b) return;

        if (_flags.ContainsValue(a) && _flags.ContainsValue(b))
        {
            var pa = PraseFlag(a);
            var pb = PraseFlag(b);

            if (pa.index == pb.index && pa.dimension != pb.dimension)
            {
                MatrixTransform.Transpose(_cells, _layoutOrder);
                Step.GetComponent<StepView>().increase();
                return;
            }//转置

            else if (pa.position == pb.position && pa.index != pb.index)
            {
                MatrixTransform.Swap(_cells, _layoutOrder, pa.dimension, pa.index, pb.index);
                Step.GetComponent<StepView>().increase();
                return;
            }//互换
        }

        else if (_flags.ContainsValue(a) && _cells.ContainsValue(b))
        {
            var pa = PraseFlag(a);
            var pb = PraseCell(b);

            int targetIndex = (pa.dimension == Dimension.ROW ? pb.row : pb.col);

            if (pa.index != targetIndex)
            {
                MatrixTransform.AddIn(_cells, _layoutOrder, pa.dimension, pa.index, targetIndex);
                Step.GetComponent<StepView>().increase();
                return;
            }//叠加
        }

        else if (_nums.Contains(a) && _flags.ContainsValue(b))
        {
            var pb = PraseFlag(b);
            MatrixTransform.Multiply(_cells, _layoutOrder, pb.dimension, pb.index, a);//倍乘
            Step.GetComponent<StepView>().increase();
            return;
        }
        else if (_flags.ContainsValue(a) && _nums.Contains(b))
        {
            var pa = PraseFlag(a);
            MatrixTransform.Multiply(_cells, _layoutOrder, pa.dimension, pa.index, b);//倍乘
            Step.GetComponent<StepView>().increase();
            return;
        }

        else if (_cells.ContainsValue(a) && _flags.ContainsValue(b))//展开?
        {
            var pa = PraseCell(a);
            var pb = PraseFlag(b);

            //可以行展开
            if (pb.dimension == Dimension.ROW && MatrixTransform.CanRowExpande(_cells, _layoutOrder, pb.index))
            {
                MatrixTransform.Cofactor(_cells, _flags, _layoutOrder, pa.row, pa.col);
                Draw(_layoutOrder - 1, false);
                Step.GetComponent<StepView>().increase();
                return;
            }
            //可以列展开
            else if (pb.dimension == Dimension.COL && MatrixTransform.CanColExpande(_cells, _layoutOrder, pb.index))
            {
                MatrixTransform.Cofactor(_cells, _flags, _layoutOrder, pa.row, pa.col);
                Draw(_layoutOrder - 1, false);
                Step.GetComponent<StepView>().increase();
                return;
            }
        }

        // else if (_cells.ContainsValue(a) && b.name.Contains("Canvas"))//展开?
        // {
        //     var pa = PraseCell(a);

        //     //可以行展开
        //     if (MatrixTransform.CanRowExpande(_cells, _layoutOrder, pa.col))
        //     {
        //         MatrixTransform.Cofactor(_cells, _flags, _layoutOrder, pa.row, pa.col);
        //         Draw(_layoutOrder - 1, false);
        //     }
        //     //可以列展开
        //     else if (MatrixTransform.CanColExpande(_cells, _layoutOrder, pa.row))
        //     {
        //         MatrixTransform.Cofactor(_cells, _flags, _layoutOrder, pa.row, pa.col);
        //         Draw(_layoutOrder - 1, false);
        //     }
        // }
    }
    #endregion
}
