using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CountDownUI : MonoBehaviour
{
    [SerializeField]
    GameObject stageScene = null;
    [SerializeField]
    GameObject timeLimit = null;
    [SerializeField]
    GameObject[] EnemyGenerators = null;
    [SerializeField]
    GameObject[] ItemEnemyGenerators = null;
    Animator animator;
    JoinPlayer joinPlayer = new JoinPlayer();
    static readonly int countDownId = Animator.StringToHash("CountDown");

    private void Awake()
    {
        foreach(var enemyGenerator in EnemyGenerators)
        {
            enemyGenerator.GetComponent<OneEnemyGenerator>().enabled = false;
            enemyGenerator.GetComponent<TwoEnemyGenerator>().enabled = false;
            enemyGenerator.GetComponent<LeaderEnemyGenerator>().enabled = false;
        }
        ItemEnemyGenerators[0].GetComponent<ItemEnemyGenerator>().enabled = false;
        ItemEnemyGenerators[1].GetComponent<ItemEnemyGenerator>().enabled = false;
        
        timeLimit.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StopPlayerScripts(GameObject player)
    {
        player.GetComponent<Player>().enabled = false;
    }

    public void CountStart(GameObject user01,GameObject user02)
    {
        StartCoroutine(OnCountStart(user01,user02));
    }

    IEnumerator OnCountStart(GameObject user01, GameObject user02)
    {
        animator.SetTrigger(countDownId);
        yield return new WaitForSeconds(4);

        foreach (var enemyGenerator in EnemyGenerators)
        {
            enemyGenerator.GetComponent<OneEnemyGenerator>().enabled = true;
            enemyGenerator.GetComponent<TwoEnemyGenerator>().enabled = true;
            enemyGenerator.GetComponent<LeaderEnemyGenerator>().enabled = true;
        }
        ItemEnemyGenerators[0].GetComponent<ItemEnemyGenerator>().enabled = true;
        ItemEnemyGenerators[1].GetComponent<ItemEnemyGenerator>().enabled = true;
        
        timeLimit.SetActive(true);
        joinPlayer.StartPlayerScripts(GameObject.FindGameObjectsWithTag("PlayerObject"));
        joinPlayer.HidePlayerUI(user01,user02);
    }
}
