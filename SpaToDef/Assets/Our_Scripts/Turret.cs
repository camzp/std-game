﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour{

    //Atributos
    public float fireRate;
    private float fireCountdown = 0f;

    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    public float slowPct = .5f;


    //Unity
    private Transform target;
    private float range;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f; //Velocidade de 'girada' da torre.
    public GameObject bulletPrefab;
    public Transform fire;
    public TurretUI turretUI;

    private Enemy targetEnemy;
    

    private void Start()
    {
        range = GetComponent<TurretStats>().range;
        GameObject turretUIobject = GameObject.FindGameObjectWithTag("TurretUI");
        turretUI = turretUIobject.GetComponent<TurretUI>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        
    }





    void UpdateTarget() {
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
                targetEnemy = nearestEnemy.GetComponent<Enemy>();
            }
            else target = null;

        }
    }
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
            return;
        }
            //Achando um alvo.


        FindTarget();

        if (useLaser)
        {
            Laser();
        }

        else {
            if (fireCountdown <= 0){
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        } 

        fireCountdown -= Time.deltaTime;
    }

    void FindTarget()
    {
                Vector3 direction = target.position - transform.position; //Vetor que aponta para a direção do inimigo.
                Quaternion pointRotation = Quaternion.LookRotation(direction);
                Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,pointRotation, Time.deltaTime * turnSpeed).eulerAngles;
                partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f); //Só roda em torno do eixo y
    }

    void Laser()
    {
        targetEnemy.Slow(slowPct);

        if (!lineRenderer.enabled){
            lineRenderer.enabled = true;
        }
        lineRenderer.SetPosition(0, fire.position);
        lineRenderer.SetPosition(1, target.position);

    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, fire.position, fire.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Chase(target);
        }
    }


    void OnDrawGizmosSelected(){
        //Função que desenha um range de tiro para a torre.       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);    
    }

    private void OnMouseDown()
    {
        if(turretUI.getTarget() == gameObject)
        {
            turretUI.DeselectTurret();
                
            return;
        }
        Destroy(turretUI.circle);
        turretUI.setTarget(gameObject);
    }

}
