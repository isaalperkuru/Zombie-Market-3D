using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    private int currentHealth;

    public int GetHealth{
        get
        {
            return currentHealth;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void GiveDamage(int amount)
    {
        currentHealth -= amount;
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
    }
}
