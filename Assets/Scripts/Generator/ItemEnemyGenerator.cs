using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEnemyGenerator : MonoBehaviour
{
    //　出現させる敵を入れておく
    [SerializeField]
    GameObject itemEnemyType;
    //　次に敵が出現するまでの時間
    [SerializeField]
    float appearNextTime;
    //　この場所から出現する敵の数
    [SerializeField]
    int maxNumOfEnemys;
    //　今何人の敵を出現させたか（総数）
    private int numberOfEnemys;
    //　待ち時間計測フィールド
    private float elapsedTime;
    [SerializeField]
    float minValue = -4.0f;
    [SerializeField]
    float maxValue = 4.0f;
    public List<GameObject> Enemys { get; private set; } = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        numberOfEnemys = 0;
        elapsedTime = 0f;
    }


    // Update is called once per frame
    void Update()
    {
            Appearance();

            if (maxNumOfEnemys == 0)
            {
                Enemys.RemoveAll(item => item == null);
                return;
            }
    }
    public void Appearance()
    {
        numberOfEnemys = Enemys.Count;
        //　この場所から出現する最大数を超えてたら何もしない
        if (numberOfEnemys >= maxNumOfEnemys)
        {
            Enemys.RemoveAll(item => item == null);
            return;
        }
        //　経過時間を足す
        elapsedTime += Time.deltaTime;

        //　経過時間が経ったら
        if (elapsedTime > appearNextTime)
        {
            elapsedTime = 0f;

            AppearItemEnemy();
        }
    }


    //　敵出現メソッド
    public void AppearItemEnemy()
    {
        //　敵の向きをランダムに決定
        var randomRotationY = Random.value * 360f;
        // 敵の位置をランダムに決定
        var randomPosX = Random.Range(minValue, maxValue);
        var randomPosZ = Random.Range(minValue, maxValue);
        var pos = transform.position;
        pos.x = transform.position.x + randomPosX;
        pos.z = transform.position.z + randomPosZ;

        GameObject enemy = Instantiate(itemEnemyType, pos, Quaternion.Euler(0f, randomRotationY, 0f)).gameObject;
        Enemys.Add(enemy);
        elapsedTime = 0f;
    }
}
