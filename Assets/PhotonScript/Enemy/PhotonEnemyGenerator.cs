using UnityEngine;
using Fusion;
using System.Runtime.Serialization;
using System.Collections.Generic;
using UnityEngine.Timeline;

public class PhotonEnemyGenerator : NetworkBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float appearNextTimer;

    [SerializeField] private int maxEnemys;
    private int numEnemys;
    private float elapsedTime = 20f;

    [SerializeField] private float minValue = -4.0f;
    [SerializeField] private float maxValue = 4.0f;

    public List<GameObject> Enemys { get; private set; } = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numEnemys = 0;
        elapsedTime = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        Appearance();
        if(maxEnemys == 0)
        {
            Enemys.RemoveAll(item => item == null);
            return;
        }
    }

    public void Appearance()
    {
        numEnemys = Enemys.Count;
        if(numEnemys >= maxEnemys)
        {
            Enemys.RemoveAll(item => item == null);
            return;
        }
        elapsedTime += Time.deltaTime;
        if(elapsedTime > appearNextTimer)
        {
            elapsedTime = 0f;
        }
    }

    public void AppearItemEnemy()
    {
        var randomRataionY = Random.value * 360f;
        var randomPosX = Random.Range(minValue,maxValue);
        var randomPosZ = Random.Range(minValue,maxValue);
        var pos = transform.position;
        pos.x = transform.position.x + randomPosX;
        pos.z = transform.position.z + randomPosZ;

        GameObject enemys = Instantiate(enemy,pos,Quaternion.Euler(0f,randomRataionY,0f)).gameObject;
        Enemys.Add(enemys);
        elapsedTime = 0f;
    }
}
