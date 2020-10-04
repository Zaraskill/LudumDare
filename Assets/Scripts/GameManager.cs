using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Dash")]
    public float dashTime;
    [SerializeField] private float _dashTimeLeft;

    [Header("SpawnPoint")]
    public PlayerController player;
    private Transform _playerSpawnpoint;

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
        Init();
    }

    public void Init()
    {
        _dashTimeLeft = dashTime;

        /* Spawn du joueur
        _playerSpawnpoint = GameObject.FindGameObjectWithTag("Spawnpoint").transform;
        Instantiate(player, _playerSpawnpoint.position, Quaternion.identity); */
    }

    // Update is called once per frame
    void Update()
    {
        if (DashTimer())
        {
            ResetDashTimer();
        }
    }

    public void FinishLevel()
    {
        //Anim de fin à lancer?
        _dashTimeLeft = 1000000000;
        StartCoroutine(WaitAnimChangeLevel());
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
        return false;
    }

    private void ResetDashTimer()
    {
        Debug.Log("J'sors le RS");
        _dashTimeLeft = dashTime;
    }

    IEnumerator WaitAnimChangeLevel()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Init();            
        }
        else
        {
            //Finis le jeu
        }
    }
}
