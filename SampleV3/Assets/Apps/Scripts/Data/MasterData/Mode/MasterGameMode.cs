using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterGameMode : MasterModel
{
    public int Order { get; set; }
    public long MoneyMin { get; set; }
    public long MoneyMax { get; set; }
    public long Step { get; set; }
    public TypeOfGameMode GameType { get; set; }
}
