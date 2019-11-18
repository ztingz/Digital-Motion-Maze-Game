using System.Collections.Generic;

public class Record
{
    /// <summary>
    /// 
    /// </summary>
    public int RoundIndex;
    /// <summary>
    /// 
    /// </summary>
    public int StarNumber = 0;
    /// <summary>
    /// 
    /// </summary>
    public int Time;
    /// <summary>
    /// 
    /// </summary>
    public int Step;
    /// <summary>
    /// 
    /// </summary>

    public Record(int index, int star, int time, int step) { RoundIndex = index; StarNumber = star; Time = time; Step = step; }
    public Record(int index) { RoundIndex = index; }
    public Record() { }
}