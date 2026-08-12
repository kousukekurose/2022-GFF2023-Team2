using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject boss = null;

    [SerializeField]
    Vector3 bossPos = new Vector3(0, 0, 0);

    [SerializeField]
    Vector3 bossRot = new Vector3(0,180,0);

    [SerializeField]
    Animator alertAnimator = null;

    [SerializeField]
    float waitTime = 5.0f;

    static readonly int alertId = Animator.StringToHash("Alert");

    public void BossGenerate()
    {
        StartCoroutine(OnBossGenerate());
    }
    IEnumerator OnBossGenerate()
    {
        alertAnimator.SetTrigger(alertId);
        yield return new WaitForSeconds(waitTime);
        Instantiate(boss, bossPos, Quaternion.Euler(bossRot));
    }
}
