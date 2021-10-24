using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject game;
    public GameObject Menu;
    public GameObject turretsParent;
    public Transform disabledTurretsParent;
    public GameObject car;
    public Transform healthDropParent;
    public Transform ammoDropParent;
    public TMPro.TMP_Dropdown dropdown;
    public TMPro.TMP_Text turretsRemaining;
    public TMPro.TMP_Text timeRemaining;
    public GameObject accelerator;
    public Text ammo;
    public Toggle autoAccelerate;
    public GameObject pausedMenu;
    private bool isPauseGame = false;
    private int timeLimit = 30;
    private float timer = 30;
    private int seconds;
    private bool gameStarted = false;
    bool updateTimer = true;
    bool gameOver = false;
    private Vector3 carDefaultPosi = new Vector3(-4.71999979f, 0f, -23.7999992f);


    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        Menu.SetActive(true);
        game.SetActive(false);
    }


    void Update()
    {
        if(gameStarted && timeRemaining.gameObject.activeInHierarchy && timer > 0 && updateTimer)
        {
            timer -= Time.deltaTime;
            seconds = Mathf.FloorToInt(timer % 60);
            timeRemaining.text = "Time left : " + seconds + " s";
        }
        if(gameStarted && seconds == 0 && updateTimer)
        {
            updateTimer = false;
            pausedMenu.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Game Over, time up !";
            pausedMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            pausedMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            pauseGame();
        }
        if(gameStarted)
        {
            turretsRemaining.text = "Turrets left : " + turretsParent.transform.childCount;
            if(turretsParent.transform.childCount == 0 && !gameOver)
            {
                gameOver = true;
                pausedMenu.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Game Over, you won !!!";
                pausedMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                pausedMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                pauseGame();
            }
        }
    }

    public void pauseGame()
    {
        if(isPauseGame)
        {
            isPauseGame = false;
            Time.timeScale = 1; 
            pausedMenu.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Game Paused";
            pausedMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            pausedMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            pausedMenu.SetActive(false);
        }
        else
        {
            isPauseGame = true;
            Time.timeScale = 0;
            pausedMenu.SetActive(true);
        }

    }

    public void playerDead()
    {
        pausedMenu.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Game Over, you lost !";
        pausedMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        pausedMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
        pauseGame();
    }

    public void setTimeLimit()
    {
        updateTimer = true;
        switch (dropdown.value)
        {
            case 0:
                timeLimit = 30;
                timer = timeLimit;
                break;
            case 1:
                timeLimit = 60;
                timer = timeLimit;
                break;
            case 2:
                timeLimit = -1;
                updateTimer = false;
                timeRemaining.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void startGame()
    {
        game.SetActive(true);
        setTimeLimit();
        if(autoAccelerate.isOn)
        {
            accelerator.SetActive(false);
        }
        else
        {
            accelerator.SetActive(true);
        }
        resetGame();
        Menu.SetActive(false);
        gameStarted = true;
    }

    public void exit()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        gameStarted = false;
        timer = timeLimit;
    }

    private void resetGame()
    {
        if (disabledTurretsParent.childCount != 0)
        {
            for(int i = 0; i < disabledTurretsParent.childCount; i++)
            {
                disabledTurretsParent.GetChild(i).gameObject.SetActive(true);
                disabledTurretsParent.GetChild(i).parent = turretsParent.transform;
            }
        }
        car.transform.localPosition = Vector3.zero;
        car.transform.localEulerAngles = Vector3.zero;
        Rigidbody carRigidbody= car.GetComponent<Rigidbody>();
        carRigidbody.isKinematic = true;
        StartCoroutine(stopCar(carRigidbody));
        setDrops();
    }

    void setDrops()
    {
        for(int i = 0; i < healthDropParent.childCount; i++)
        {
            healthDropParent.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < ammoDropParent.childCount; i++)
        {
            ammoDropParent.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            healthDropParent.GetChild(Random.Range(0, healthDropParent.childCount)).gameObject.SetActive(true);
            ammoDropParent.GetChild(Random.Range(0, ammoDropParent.childCount)).gameObject.SetActive(true);
        }
    }

    public void changeDrop(bool ammo)
    {
        if(ammo)
            ammoDropParent.GetChild(Random.Range(0, ammoDropParent.childCount)).gameObject.SetActive(true);
        else
            healthDropParent.GetChild(Random.Range(0, healthDropParent.childCount)).gameObject.SetActive(true);
    }

    IEnumerator stopCar(Rigidbody car)
    {
        yield return new WaitForSeconds(0.2f);
        car.isKinematic = false;
    }
}
