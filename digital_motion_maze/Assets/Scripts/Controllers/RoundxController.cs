using System;
using System.Collections.Generic;
using fraction;
using UnityEngine;

public enum FlagPosition : byte { L = 0, R = 1, U = 2, D = 3 }

public class RoundxController : MonoSingleton<RoundxController>
{
    #region 属性
    //关卡数据
    private Round _round;

    //布局信息
    public const int ELEMENTS_PANEL_SIZE = 800;//面板大小
    public const float ELEMENT_RATIO = 0.92f;//占总的比率
    private int _cellSize;//大小
    private int _flagSize;//行列标记大小
    private int _gapSize;//间隙大小
    protected int _layoutOrder;//布局数量=Order

    //场景
    private RectTransform _root;//根结点
    private const string ROOT_NAME = "Root";//根结点名称
    private RectTransform _numRoot;//乘数根结点
    private const string NUM_ROOT_NAME = "NumRoot";//乘数根结点名称
    public GameObject CellPFB;//格子预制体
    public GameObject FlagPFB;//行列标记预制体

    public const string CELL_KEY = "[{0},{1}]";//e.g.[0,0]
    private Dictionary<string, GameObject> _cells;//格子字典
    public const string FLAG_KEY = "{0}[{1}]";//e.g.R[0]
    private Dictionary<string, GameObject> _flags;//标记字典
    public const string NUM_KEY = "N[{0}]";//e.g.R[0]
    protected List<GameObject> _nums;//乘数列表
    #endregion

    protected virtual void Awake()
    {
        _root = GameObject.Find(ROOT_NAME).GetComponent<RectTransform>();
        _numRoot = GameObject.Find(NUM_ROOT_NAME).GetComponent<RectTransform>();
        _round = ConfigManager.LoadRoundFromJsonFile(4);
        _cells = new Dictionary<string, GameObject>();
        _flags = new Dictionary<string, GameObject>();
        _nums = new List<GameObject>();
    }

    private void Start()
    {
        InitLayout(_round.Matrix.Order);
        DrawMainLayout(true);
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
    }

    #region 方法
    public void DrawMainLayout(bool instantiate)
    {
        //移动步长
        int move_unit = _cellSize + 2 * _gapSize;
        //初始偏移量
        float i_d = (_layoutOrder / 2) * move_unit;
        Vector2 initial_distance = new Vector2(-i_d, i_d);
        //大小向量
        Vector2 size = new Vector2(_cellSize, _cellSize);

        foreach (FlagPosition flagPosition in Enum.GetValues(typeof(FlagPosition)))
        {
            for (int j = 0; j < _layoutOrder; ++j)
            {
                string flag_name = string.Format(FLAG_KEY, Enum.GetName(flagPosition.GetType(), flagPosition), j);
                if (instantiate) _flags.Add(flag_name, Instantiate(FlagPFB, _root));
                _flags[flag_name].name = flag_name;

                if (flagPosition == FlagPosition.L || flagPosition == FlagPosition.R)
                    _flags[flag_name].GetComponent<FlagView>().Dimension = Dimension.ROW;
                else _flags[flag_name].GetComponent<FlagView>().Dimension = Dimension.COL;

                _flags[flag_name].GetComponent<FlagView>().FlagPosition = flagPosition;
                _flags[flag_name].GetComponent<FlagView>().Index = j;
            }
        }

        //绘制布局
        for (byte i = 0; i < _layoutOrder; ++i)
        {
            for (byte j = 0; j < _layoutOrder; ++j)
            {
                string cellKey = string.Format(CELL_KEY, i, j);
                if (instantiate) _cells.Add(cellKey, Instantiate(CellPFB, _root));//实例化预制体
                _cells[cellKey].name = cellKey;

                //设置格子大小
                foreach (RectTransform rt in _cells[cellKey].GetComponentsInChildren<RectTransform>())
                    rt.sizeDelta = size;
                _cells[cellKey].GetComponent<CellView>().Fraction = new Fraction(_round.Matrix.Content[i, j]);//设置格子内容
                _cells[cellKey].GetComponent<CellView>().RowNum = i;
                _cells[cellKey].GetComponent<CellView>().ColNum = j;

                // DrawElementView(k, i, j);

                //设置格子位置
                Vector2 vector = new Vector2(j * move_unit, -i * move_unit) + initial_distance;
                if (_layoutOrder % 2 == 0) vector += new Vector2(move_unit / 2, -move_unit / 2);
                _cells[cellKey].GetComponent<Transform>().localPosition = vector;

                //需要显示Flag
                // if (i == 0 || j == 0) DrawElementsFlagView(k, i, j);

                if (j == 0)//Left
                    _flags[string.Format(FLAG_KEY, FlagPosition.L.ToString(), i)].GetComponent<Transform>().localPosition
                    = vector - new Vector2((_cellSize + _flagSize) / 2 + _gapSize, 0);
                else if (j == _layoutOrder - 1)//Right
                    _flags[string.Format(FLAG_KEY, FlagPosition.R.ToString(), i)].GetComponent<Transform>().localPosition
                    = vector + new Vector2((_cellSize + _flagSize) / 2 + _gapSize, 0);
                if (i == 0)//Up
                    _flags[string.Format(FLAG_KEY, FlagPosition.U.ToString(), j)].GetComponent<Transform>().localPosition =
                    vector + new Vector2(0, (_cellSize + _flagSize) / 2 + _gapSize);
                else if (i == _layoutOrder - 1)//Down
                    _flags[string.Format(FLAG_KEY, FlagPosition.D.ToString(), j)].GetComponent<Transform>().localPosition =
                    vector - new Vector2(0, (_cellSize + _flagSize) / 2 + _gapSize);
            }
        }
    }

    public void DrawNums()
    {
        foreach (double m in _round.Multiplier)
        {
            GameObject temp = Instantiate(CellPFB, _numRoot);
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
            { MatrixTransform.Transpose(_cells, _layoutOrder); return; }//转置

            else if (pa.position == pb.position && pa.index != pb.index)
            { MatrixTransform.Swap(_cells, _layoutOrder, pa.dimension, pa.index, pb.index); return; }//互换
        }

        else if (_flags.ContainsValue(a) && _cells.ContainsValue(b))
        {
            var pa = PraseFlag(a);
            var pb = PraseCell(b);

            int targetIndex = (pa.dimension == Dimension.ROW ? pb.row : pb.col);

            if (pa.index != targetIndex)
            { MatrixTransform.AddIn(_cells, _layoutOrder, pa.dimension, pa.index, targetIndex); return; }//叠加
        }

        else if (_nums.Contains(a) && _flags.ContainsValue(b))
        {
            var pb = PraseFlag(b);
            MatrixTransform.Multiply(_cells, _layoutOrder, pb.dimension, pb.index, a);//倍乘
        }
        else if (_flags.ContainsValue(a) && _nums.Contains(b))
        {
            var pa = PraseFlag(a);
            MatrixTransform.Multiply(_cells, _layoutOrder, pa.dimension, pa.index, b);//倍乘
        }

        else if (_cells.ContainsValue(a) && _flags.ContainsValue(b))//展开?
        {
            var pa = PraseCell(a);
            var pb = PraseFlag(b);

            //可以行展开
            if (pb.dimension == Dimension.ROW && MatrixTransform.CanRowExpande(_cells, _layoutOrder, pb.index))
            {
                MatrixTransform.Cofactor(_cells, _layoutOrder, pa.row, pa.col);
                --_layoutOrder;
                DrawMainLayout(false);
            }
            //可以列展开
            else if (pb.dimension == Dimension.COL && MatrixTransform.CanColExpande(_cells, _layoutOrder, pb.index))
            {
                MatrixTransform.Cofactor(_cells, _layoutOrder, pa.row, pa.col);
                --_layoutOrder;
                DrawMainLayout(false);
            }
        }
    }
    #endregion
}
