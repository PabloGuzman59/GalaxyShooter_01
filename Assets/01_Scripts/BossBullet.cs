using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    public float damage;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
       /*ANETT - CAMBIO*/
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
        }
    }

}
