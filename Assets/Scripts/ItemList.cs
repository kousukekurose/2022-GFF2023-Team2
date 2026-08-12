using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    TextAsset csvFile;
    // CSVの中身を入れるリスト;
    private List<string[]> DataList = new List<string[]>();

    public List<Item> Items { get ; private set; } = new List<Item>();

    void Start()
    {
        // Resouces下のCSV読み込み
        csvFile = Resources.Load("Item") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            DataList.Add(line.Split(',')); // , 区切りでリストに追加
        }

        // アイテムリストに格納
        for (int i = 0; i < DataList.Count; i++)
        {
            Items.Add(new Item(int.Parse(DataList[i][0]), DataList[i][1], int.Parse(DataList[i][2]),
                float.Parse(DataList[i][3]), int.Parse(DataList[i][4]),
                int.Parse(DataList[i][5]), int.Parse(DataList[i][6])));
        }
    }
}
