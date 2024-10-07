using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public float timeBtwSpawn = 1.5f;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform bossSpawnPoint; // Nuevo punto de spawn del jefe
    public List<GameObject> enemyPrefabs;
    public int score = 0;
    public Text scoreText;
    public int enemiesPerWave = 10; // Número de enemigos por oleada
    //public int currentWave = 1;
    //public Text waveText; // Texto que muestra la oleada
    private int enemiesSpawned = 0; // Número de enemigos que se han generado en la oleada
    private float timer = 0f;


    // Variables nuevas y necesarias para el control de oleadas
    public int enemiesToKill = 3; // Número de enemigos que debes matar para pasar de oleada (empezando con 3)
    private int enemiesKilled = 0; // Contador de enemigos derrotados
    public int currentWave = 2; // Oleada actual
    public Text waveText; // Texto para mostrar la oleada en la pantalla


    // Variables del jefe final
    public GameObject bossPrefab; // Prefab del jefe
    public Transform bossSpawnPosition; // Posición del jefe (centrada en la parte superior)
    private bool bossSpawned = false; // Para evitar que el jefe se spawnee varias veces
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Asegura el patrón Singleton
        }
    }

    void Start()
    {
        scoreText.text = "SCORE: " + score;
        waveText.text = "WAVE: " + currentWave;
    }

    void Update()
    {
        if (!bossSpawned)
        {
            Debug.Log("Enemies Spawned: " + enemiesSpawned + " / " + (enemiesPerWave * currentWave)); // Para verificar el estado de los enemigos
            if (enemiesSpawned < enemiesPerWave * currentWave)
            {
                SpawnEnemy();
            }
            else if (currentWave >= 3 && !bossSpawned)
            {
                Debug.Log("Spawning boss"); // Para verificar si está intentando spawnear al jefe
                SpawnBoss();
            }
            else if (enemiesSpawned >= enemiesPerWave * currentWave)
            {
                NextWave();
            }
        }
    }


    void SpawnEnemy()
    {
        if (timer < timeBtwSpawn)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            float x = Random.Range(leftPoint.position.x, rightPoint.position.x);
            int enemy = Random.Range(0, enemyPrefabs.Count);
            Vector3 newPos = new Vector3(x, transform.position.y, transform.position.z);
            Instantiate(enemyPrefabs[enemy], newPos, Quaternion.Euler(0, 180, 0));
            enemiesSpawned++;
        }
    }

    // Método para spawnear al jefe final
    void SpawnBoss()
    {
        if (!bossSpawned) // Evitar que el jefe se spawnee varias veces
        {
            // Establece la posición en las coordenadas que mencionas (11.72, -6.34)
            Vector3 bossPosition = new Vector3(0.3f, 2.47f, 0f); // z en 0 para 2D

            Instantiate(bossPrefab, bossPosition, Quaternion.identity); // Spawnea al jefe en la posición deseada
            bossSpawned = true;
            waveText.text = "BOSS WAVE!"; // Muestra en pantalla que es la oleada del jefe
        }
    }




    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "SCORE: " + score;
    }
    // Método para incrementar el número de enemigos derrotados
    public void EnemyKilled()
    {
        enemiesKilled++; // Incrementa el contador de enemigos muertos

        // Verifica si hemos matado suficientes enemigos para pasar a la siguiente oleada
        if (enemiesKilled >= enemiesToKill)
        {
            NextWave();
        }
    }
    // Método para pasar a la siguiente oleada
    void NextWave()
    {
        currentWave++; // Incrementa la oleada
        waveText.text = "WAVE: " + currentWave; // Actualiza el texto en pantalla

        // Reinicia el contador de enemigos
        enemiesKilled = 0;

        // Aumenta la cantidad de enemigos a matar según la oleada actual
        if (currentWave == 2)
        {
            enemiesToKill = 2; // En la oleada 2 debes matar 5 enemigos
        }
        else if (currentWave == 3)
        {
            enemiesToKill = 4; // En la oleada 3 debes matar 7 enemigos
        }
        else if (currentWave == 4)
        {
            // Aquí empieza la oleada del jefe, puedes definir lo que suceda
            SpawnBoss();
            // Aquí implementarías la lógica del jefe.
        }
    }


}
