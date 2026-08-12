using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderEnemyGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject leaderEnemyType;

    [SerializeField]
    float appearNextTime;
    [SerializeField]
    int maxNumOfEnemys;
    private int numberOfEnemys;
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
        elapsedTime = 20f;
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
        if (numberOfEnemys >= maxNumOfEnemys)
        {
            Enemys.RemoveAll(item => item == null);
            return;
        }
        elapsedTime += Time.deltaTime;

        if (elapsedTime > appearNextTime)
        {
            elapsedTime = 0f;

            AppearItemEnemy();
        }
    }


    public void AppearItemEnemy()
    {
        var randomRotationY = Random.value * 360f;
        var randomPosX = Random.Range(minValue, maxValue);
        var randomPosZ = Random.Range(minValue, maxValue);
        var pos = transform.position;
        pos.x = transform.position.x + randomPosX;
        pos.z = transform.position.z + randomPosZ;

        GameObject enemy = Instantiate(leaderEnemyType, pos, Quaternion.Euler(0f, randomRotationY, 0f)).gameObject;
        Enemys.Add(enemy);
        elapsedTime = 0f;
    }
}
