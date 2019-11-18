using UnityEngine;
using UnityEngine.UI;

public class RoundCellView : BaseCellView
{
    private Record _record;
    public Record Record
    {
        get
        {
            return _record;
        }
        set
        {
            _record = value;
            if (_record.StarNumber == 3)
            {
                Star1.sprite = ActiveStar;
                Star2.sprite = ActiveStar;
                Star3.sprite = ActiveStar;
            }
            else if (_record.StarNumber == 2)
            {
                Star1.sprite = ActiveStar;
                Star2.sprite = ActiveStar;
            }
            else if (_record.StarNumber == 1)
            {
                Star1.sprite = ActiveStar;
            }

            Fraction = new fraction.Fraction(Record.RoundIndex + 1);
            return;
        }
    }

    public Image Star1;
    public Image Star2;
    public Image Star3;
    public Sprite ActiveStar;

    public GameObject Mask;
}
