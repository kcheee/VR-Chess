using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_EnemyHP : MonoBehaviour
{
    int hp;
    public int maxHP = 1;
    public int HP
    {
        get { return hp; }
        set { hp = value; } 
    }

    private void Awake()
    {
        HP = maxHP;
    }

}
