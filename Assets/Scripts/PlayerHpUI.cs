using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpUI : MonoBehaviour
{
    int box = 5;
    [SerializeField]
    private GameObject[] hpgauge;

    public void Damage(int damage)
    {
        box -= damage;
        if (box <= 0)
        {
            box = 0;
        }
        for (int i = 0; i < 5; i++)
        {
            if (i < box)
            {
                hpgauge[i].SetActive(true);
            }
            else
            {
                hpgauge[i].SetActive(false);
            }
        }
    }
    public void Heel()
    {
        for (int i = 0; i <= 4; i++)
        {
            hpgauge[i].SetActive(true);
        }
        box = 5;
    }
}
