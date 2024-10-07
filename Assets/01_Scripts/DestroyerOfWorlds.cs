using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerOfWorlds : MonoBehaviour
{
    public float maxHealth = 100f; // Vida máxima, modificable desde el Inspector
    private float currentHealth; // Vida actual del jefe

    public GameObject[] bossWeapons; // Las armas o comportamientos diferentes del jefe

    public GameObject bulletPrefab; // Prefab de las balas del jefe
    public Transform player; // Posición del jugador
    public float health = 10f; // Vida del jefe
    public float bulletSpeed = 5f; // Velocidad de las balas
    public float attackInterval = 1f; // Intervalo entre ataques

    private float currentAttackInterval;
    private bool isAttacking = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isAttacking)
        {
            CheckHealthAndAttack();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject); // Destruye el jefe cuando su vida llegue a 0
    }
    void CheckHealthAndAttack()
    {
        if (health > 80f)
        {
            BaseAttack(); // Ataque básico (balas dirigidas)
        }
        else if (health <= 80f && health > 50f)
        {
            Phase1Attack(); // Fase 1: ráfagas de balas rápidas
        }
        else if (health <= 50f && health > 20f)
        {
            Phase2Attack(); // Fase 2: lluvia de balas
        }
        else if (health <= 20f)
        {
            Phase3Attack(); // Fase 3: gran proyectil dirigido
        }
    }
    void BaseAttack()
    {
        isAttacking = true;

        // Dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Instanciamos la bala
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        currentAttackInterval = attackInterval; // Reiniciar el tiempo entre ataques
        Invoke(nameof(ResetAttack), currentAttackInterval); // Espera para volver a atacar
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void Phase1Attack()
    {
        isAttacking = true;

        int numberOfBullets = 8; // Cantidad de balas en ráfaga

        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * (360f / numberOfBullets); // Calcula el ángulo para cada bala
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }

        currentAttackInterval = attackInterval * 0.8f; // Ráfagas más rápidas
        Invoke(nameof(ResetAttack), currentAttackInterval);
    }
    void Phase2Attack()
    {
        isAttacking = true;

        int numberOfBullets = 10;
        float xStart = -10f; // Empezamos desde la izquierda de la pantalla
        float xEnd = 10f; // Y terminamos a la derecha

        for (int i = 0; i < numberOfBullets; i++)
        {
            float xPos = Mathf.Lerp(xStart, xEnd, (float)i / (numberOfBullets - 1)); // Distribuye las balas a lo largo de la parte superior
            Vector2 spawnPosition = new Vector2(xPos, 6f); // Ajusta la coordenada Y según tu escena

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * bulletSpeed; // Dispara hacia abajo
        }

        currentAttackInterval = attackInterval * 1.5f; // Ataques más lentos pero masivos
        Invoke(nameof(ResetAttack), currentAttackInterval);
    }
    void Phase3Attack()
    {
        isAttacking = true;

        GameObject bigBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bigBullet.transform.localScale = new Vector3(3f, 3f, 1f); // Aumentamos el tamaño del proyectil

        StartCoroutine(FollowPlayer(bigBullet)); // Empieza a seguir al jugador

        currentAttackInterval = attackInterval * 2f; // Ataques más lentos pero poderosos
        Invoke(nameof(ResetAttack), currentAttackInterval);
    }

    IEnumerator FollowPlayer(GameObject bullet)
    {
        float followTime = 5f; // El tiempo que la bala seguirá al jugador

        while (followTime > 0)
        {
            Vector2 direction = (player.position - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * (bulletSpeed / 2); // Sigue al jugador lentamente
            followTime -= Time.deltaTime;
            yield return null;
        }

        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Deja de seguir después de un tiempo
    }

}
