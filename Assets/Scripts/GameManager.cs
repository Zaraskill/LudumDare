using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Dash")]
    public float dashTime;
    [SerializeField] private float _dashTimeLeft;

    [Header("SpawnPoint")]
    public PlayerController player;
    private Transform _playerSpawnpoint;

    [Header("UI Jauge de Dash")]
    public GameObject Jauge1;
    public GameObject Jauge2;
    public GameObject Jauge3;

    private bool isPaused;
    public GameObject canvasPause;

    [Header("End Level")]
    public GameObject displayEndlevel;
    private Player play;

    public GameObject deathVFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        play = ReInput.players.GetPlayer(0);
        Init();
    }

    public void Init()
    {
        _dashTimeLeft = dashTime;

        _playerSpawnpoint = GameObject.FindGameObjectWithTag("Spawnpoint").transform;
        PlayerController play = Instantiate(player);
        play.transform.position = _playerSpawnpoint.position;
        AudioManager.audioManager.Play("enter");
        canvasPause = GameObject.FindGameObjectWithTag("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (DashTimer())
        {
            ResetDashTimer();
        }
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            if (displayEndlevel.activeInHierarchy)
            {
                if (play.GetButtonDown("Validate"))
                {
                    SceneManager.LoadScene(0);
                    Destroy(AudioManager.audioManager.gameObject);
                }
            }
        }
        if (play.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                canvasPause.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                canvasPause.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void FinishLevel()
    {
        //Anim de fin à lancer?
        _dashTimeLeft = 10000000000000;
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Init();
        }
        else
        {
            displayEndlevel.SetActive(true);
        }
    }

    public void Respawn()
    {
        _dashTimeLeft = dashTime;
        Instantiate(deathVFX, FindObjectOfType<PlayerController>().transform.position, Quaternion.identity);
        FindObjectOfType<PlayerController>().transform.position = _playerSpawnpoint.position;
    }

    private bool DashTimer()
    {
        _dashTimeLeft -= Time.deltaTime;

        if (_dashTimeLeft <= 0)
        {
            FindObjectOfType<PlayerController>().Dash();
            Debug.Log("Le J c'est le S");
            return true;
        }
        else if(_dashTimeLeft <= 3 && _dashTimeLeft > 2)
        {
            Jauge1.SetActive(true);
            Jauge2.SetActive(false);
            Jauge3.SetActive(false);
        }
        else if (_dashTimeLeft <= 2 && _dashTimeLeft > 1)
        {
            Jauge1.SetActive(false);
            Jauge2.SetActive(true);
            Jauge3.SetActive(false);
        }
        else if (_dashTimeLeft <= 1 && _dashTimeLeft > 0)
        {
            Jauge1.SetActive(false);
            Jauge2.SetActive(false);
            Jauge3.SetActive(true);
        }
        return false;
    }

    private void ResetDashTimer()
    {
        Debug.Log("J'sors le RS");
        _dashTimeLeft = dashTime;
    }
}
