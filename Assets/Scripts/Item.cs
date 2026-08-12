using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item
{
    // アイテム番号
    public int no { get; private set; }
    // アイテム名
    public string name { get; private set; }
    // アイテム使用時の攻撃力
    public int attackBuff { get; private set; }
    // アイテム使用時のスピード
    public float speedBuff { get; private set; }
    // アイテム使用時の回復力
    public int heel { get; private set; }
    // アイテム発動可能時間
    public int buffTime { get; private set; }
    // アイテム使用可能回数
    public int buffCount { get; private set; }
    public Item(int no, string name, int attackBuff, float speedBuff, int heel, int buffTime, int buffCount)
    {
        this.no = no;
        this.name = name;
        this.attackBuff = attackBuff;
        this.speedBuff = speedBuff;
        this.heel = heel;
        this.buffTime = buffTime;
        this.buffCount = buffCount;
    }
}
