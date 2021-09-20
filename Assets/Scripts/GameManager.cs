using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject levelFinishParrent;
    [SerializeField] private Text enemyCountText;
    private bool levelFinished = false;
    private int enemyCount;
    private int maxEnemyCount;
    private int playerHealth;
    private Health health;
    public bool GetLevelFinish
    {
        get
        {
            return levelFinished;
        }
    }
    private void Awake()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        maxEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        playerHealth = health.GetHealth;
        if (enemyCount <= 0 || playerHealth <= 0)
        {
            levelFinishParrent.gameObject.SetActive(true);
            levelFinished = true;
        }
        else
        {
            UpdateEnemyCount();
            levelFinishParrent.gameObject.SetActive(false);
            levelFinished = false;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateEnemyCount()
    {
        enemyCountText.text = "Enemies: " + enemyCount + "/" + maxEnemyCount;
    }
}