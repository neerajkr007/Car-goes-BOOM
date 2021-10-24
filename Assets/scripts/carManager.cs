using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class carManager : MonoBehaviour
{
    [SerializeField]
    private float health;// { get; private set; }

    [SerializeField]
    private Slider slider;
    private playerTurretController playerTurretController;
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        health = 100f;
        slider.value = health / 100f;
        playerTurretController = transform.GetChild(3).GetComponent<playerTurretController>();
        slider = GameObject.FindGameObjectWithTag("health").GetComponent<Slider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            if(health > 0)
            {
                health -= 3f;
                slider.value = health / 100f;
            }
            else
            {
                gameManager.playerDead();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Bullet51"))
        {
            playerTurretController.increaseAmmo(1);
            gameManager.changeDrop(true);
            other.gameObject.SetActive(false);
        }
        if (other.name.Contains("Bullet52"))
        {
            playerTurretController.increaseAmmo(2);
            gameManager.changeDrop(true);
            other.gameObject.SetActive(false);
        }
        if (other.name.Contains("medkit"))
        {
            if (health <= 80)
            {
                health += 20;
            }
            else
            {
                health = 100;
            }
            slider.value = health / 100f;
            gameManager.changeDrop(false);
            other.gameObject.SetActive(false);
        }
        if (other.name.Contains("medkit2"))
        {
            if (health <= 50)
            {
                health += 50;
            }
            else
            {
                health = 100;
            }
            slider.value = health / 100f;
            gameManager.changeDrop(false);
            other.gameObject.SetActive(false);
        }

    }
}
