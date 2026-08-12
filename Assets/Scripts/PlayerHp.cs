using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHp : MonoBehaviour
{
    [SerializeField]
    PlayerHpUI p1=null;
    [SerializeField]
    PlayerHpUI p2 = null;

    public TextMeshProUGUI hpUI;
    [SerializeField]
    private MoveBehaviour stan;
    public int Hp = 5;
    bool Invincibility = false;
    //private int playerus = 1;
    private int player = 0;
    private void Awake()
    {
        hpUI.enabled = false;
    }
    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //var Mu=transform.root.GetComponent<Player>().MultiType;
        //HPÇ™0Ç…Ç»Ç¡ÇΩÇÁMoveBehaviourÇÃÉXÉ^ÉìÇåƒÇ—èoÇ∑
        if (Hp <= 0)
        {
            hpUI.enabled = true;
            Decision();
            stan.Stan();
            Invoke(nameof(HpHeel), 3.0f);
        }
    }

    public void HpHeel()
    {
        var Type = transform.root.GetComponent<Player>().MultiType;
        hpUI.enabled = false;
        Hp = 5;
        if (Type == 1)
        {
            p1.Heel();
        }
        if (Type == 2)
        {
            p2.Heel();
        }
        Invoke(nameof(Cancellation), 1.0f);
    }

    public void Decision()
    {
        Invincibility = true;

    }
    public void Cancellation()
    {
        Invincibility = false;
    }

    public void PlayerDownHp(int damage)
    {
        if (Invincibility == false)
        {
            var Type = transform.root.GetComponent<Player>().MultiType;
            //â£ÇÁÇÍÇΩÇÁHPÇå∏ÇÁÇ∑
            Hp -= damage;
            if (Type == 1)
            {
                p1.Damage(damage);
            }
            if(Type == 2)
            {
                p2.Damage(damage);
            }
        }
    }
}
