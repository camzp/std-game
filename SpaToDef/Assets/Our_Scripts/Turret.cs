﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour{
    private Transform target;
    public float range = 15f;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f; //Velocidade de 'girada' da torre.

    public float fireRate  = 1f;
    private float firecountdown = 0f; 


    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget() {
        //Metodo que procura um alvo, procura o mais próximo e checa se ele está no 'range'
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortDistance = Mathf.Infinity; //Guarda a menor distância
        GameObject nearestEnemy = null; //Inimigo mais próximo encontrado.
        foreach (GameObject enemy in enemies)
        {   //guarda a distancia
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortDistance){
                shortDistance = distanceToEnemy;
                nearestEnemy = enemy; 
            }
            if (nearestEnemy != null && shortDistance <= range)
            {
                target = nearestEnemy.transform;
            }
            else target = null;

        }
    }
    private void Update()
    {
        if (target == null) return;
        //Achando um alvo.
        Vector3 direction = target.position - transform.position; //Vetor que aponta para a direção do inimigo.
        Quaternion pointRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,pointRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); //Só roda em torno do eixo y
        


    }




    private void OnDrawGizmosSelected(){
        //Função que desenha um range de tiro para a torre.       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);    
    }

}
