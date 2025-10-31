using System.Collections.Generic;
using UnityEngine;

public class RegisterAsEnemy : MonoBehaviour
{
    public static List<RegisterAsEnemy> allEnemys = new List<RegisterAsEnemy>();

    void OnEnable()
    {
        allEnemys.Add(this);
    }

    void OnDisable()
    {
        allEnemys.Remove(this);
    }
}
