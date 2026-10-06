using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTL : MonoBehaviour
{
    [SerializeField] float tempsDeVie = 2;
    float tempsInitial;

    void Start()
    {
        tempsInitial = Time.time;
    }

    // Simple système supprimant un objet après un certain temps
    void Update()
    {
        if(Time.time > tempsInitial + tempsDeVie)
        {
            Destroy(gameObject);
        }
    }
}
