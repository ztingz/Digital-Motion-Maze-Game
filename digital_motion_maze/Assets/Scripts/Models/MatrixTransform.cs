using System.Collections.Generic;
using fraction;
using UnityEngine;

//维度枚举
public enum Dimension : byte { ROW = 0, COL = 1 }

public static class MatrixTransform
{
    //交换两个元素
    public static void ElementSwap(GameObject a, GameObject b)
    {
        //交换元素对应的数值
        fraction.Fraction value_temp = a.GetComponent<CellView>().Fraction;
        a.GetComponent<CellView>().Fraction = b.GetComponent<CellView>().Fraction;
        b.GetComponent<CellView>().Fraction = value_temp;
    }

    //行(列)转置
    public static void Transpose(Dictionary<string, GameObject> cells, int order)
    {
        Debug.Log("Transpose");
        for (int i = 0; i < order; ++i)
            for (int j = 0; j < i; ++j)
            {
                string aKey = string.Format(RoundxController.CELL_KEY, i, j);
                string bKey = string.Format(RoundxController.CELL_KEY, j, i);
                ElementSwap(cells[aKey], cells[bKey]);
            }
    }

    // 行(列)交换
    public static Fraction Swap(Dictionary<string, GameObject> cells, int order, Dimension dimension, int sourceIndex, int targetIndex)
    {
        Debug.Log("Swap");
        if (sourceIndex >= order || targetIndex >= order)
            throw new System.IndexOutOfRangeException("RCSwap的行(列)标超过阶数");

        if (dimension == Dimension.ROW)
            for (int j = 0; j < order; ++j)
            {
                string aKey = string.Format(RoundxController.CELL_KEY, sourceIndex, j);
                string bKey = string.Format(RoundxController.CELL_KEY, targetIndex, j);
                ElementSwap(cells[aKey], cells[bKey]);
            }
        else if (dimension == Dimension.COL)
            for (int i = 0; i < order; ++i)
            {
                string aKey = string.Format(RoundxController.CELL_KEY, i, sourceIndex);
                string bKey = string.Format(RoundxController.CELL_KEY, i, targetIndex);
                ElementSwap(cells[aKey], cells[bKey]);
            }
        return new Fraction(-1);
    }

    //行(列)叠加
    public static void AddIn(Dictionary<string, GameObject> cells, int order, Dimension dimension, int sourceIndex, int targetIndex)
    {
        Debug.Log("AddIn");
        if (sourceIndex >= order || targetIndex >= order)
            throw new System.IndexOutOfRangeException("RCSwap的行(列)标超过阶数");

        if (dimension == Dimension.ROW)
            for (int j = 0; j < order; ++j)
            {
                string aKey = string.Format(RoundxController.CELL_KEY, sourceIndex, j);
                string bKey = string.Format(RoundxController.CELL_KEY, targetIndex, j);
                cells[bKey].GetComponent<CellView>().Fraction += cells[aKey].GetComponent<CellView>().Fraction;
            }
        else if (dimension == Dimension.COL)
            for (int i = 0; i < order; ++i)
            {
                string aKey = string.Format(RoundxController.CELL_KEY, i, sourceIndex);
                string bKey = string.Format(RoundxController.CELL_KEY, i, targetIndex);
                cells[bKey].GetComponent<CellView>().Fraction += cells[aKey].GetComponent<CellView>().Fraction;
            }
    }


    //行(列)倍乘
    public static void Multiply(Dictionary<string, GameObject> cells, int order, Dimension dimension, int index, GameObject num)
    {
        Debug.Log("Multiply");
        if (index >= order)
            throw new System.IndexOutOfRangeException("multiply的行(列)标超过阶数");
        if (dimension == Dimension.ROW)
            for (int j = 0; j < order; ++j)
            {
                string aKey = string.Format(RoundxController.CELL_KEY, index, j);
                cells[aKey].GetComponent<CellView>().Fraction *= num.GetComponent<CellView>().Fraction;
            }
        else if (dimension == Dimension.COL)
            for (int i = 0; i < order; ++i)
            {
                string aKey = string.Format(RoundxController.CELL_KEY, i, index);
                cells[aKey].GetComponent<CellView>().Fraction *= num.GetComponent<CellView>().Fraction;
            }
    }

    public static bool CanRowExpande(Dictionary<string, GameObject> cells, int order, int index)
    {
        Debug.Log("CanRowExpande");
        int count = 0;
        for (int j = 0; j < order; ++j)
        {
            string cellKey = string.Format(RoundxController.CELL_KEY, index, j);
            if (cells[cellKey].GetComponent<CellView>().Fraction != new Fraction(0)) ++count;
            if (count > 1) return false;
        }
        return true;
    }

    public static bool CanColExpande(Dictionary<string, GameObject> cells, int order, int index)
    {
        Debug.Log("CanColExpande");
        int count = 0;
        for (int j = 0; j < order; ++j)
        {
            string cellKey = string.Format(RoundxController.CELL_KEY, j, index);
            if (cells[cellKey].GetComponent<CellView>().Fraction != new Fraction(0)) ++count;
            if (count > 1) return false;
        }
        return true;
    }

    //行(列)展开
    // public static bool Expande(Dictionary<string, GameObject> cells, int order, Dimension dimension, int index)
    // {
    //     int i = 0, temp = -1;
    //     for (int j = 0; j < order; ++j)
    //     {
    //         string cellKey = (dimension == Dimension.ROW) ? string.Format(RoundxController.CELL_KEY, index, j) : string.Format(RoundxController.CELL_KEY, j, index);
    //         if (cells[cellKey].GetComponent<CellView>().Fraction != new Fraction(0))
    //         { ++i; temp = j; }
    //         if (i > 1) return false;
    //     }
    //     if (dimension == Dimension.ROW && temp != -1)
    //         MatrixTransform.Cofactor(cells, order, index, temp);
    //     else if (dimension == Dimension.COL && temp != -1)
    //         MatrixTransform.Cofactor(cells, order, temp, index);
    //     return true;
    // }

    //代数余子式
    public static Fraction Cofactor(Dictionary<string, GameObject> cells, int order, int row, int col)
    {
        Debug.Log("Cofactor");
        if (row >= order || col >= order)
            throw new System.IndexOutOfRangeException("Cofactor的行(列)标超过阶数");

        string cellKey = string.Format(RoundxController.CELL_KEY, row, col);
        Fraction cellFraction = cells[cellKey].GetComponent<CellView>().Fraction;

        for (int i = 0; i < order; ++i)
            for (int j = 0; j < order; ++j)
                if (i == row || j == col)
                {
                    string cellRemoveKey = string.Format(RoundxController.CELL_KEY, i, j);
                    GameObject.Destroy(cells[cellRemoveKey]);
                    cells.Remove(cellRemoveKey);
                }

        for (int i = 0; i < order - 1; ++i)
            for (int j = 0; j < order - 1; ++j)
            {
                string thisKey = string.Format(RoundxController.CELL_KEY, i, j);
                if (cells.ContainsKey(thisKey)) continue;
                if (!cells.ContainsKey(thisKey))
                {
                    string offsetKey = string.Format(RoundxController.CELL_KEY, i, j + 1);
                    cells.Add(thisKey, cells[offsetKey]);
                    cells.Remove(offsetKey);
                }
            }

        //计算代数, 并返回结果
        if ((row + col) % 2 == 0) return cellFraction;
        return new Fraction(-1) * cellFraction;
    }
}