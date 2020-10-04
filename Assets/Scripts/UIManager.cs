using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Player play;

    public GameObject mainMenu;
    public GameObject firstMenu;
    public GameObject tutoMenu;

    private GameObject actualObjet;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        play = ReInput.players.GetPlayer(0);
        actualObjet = mainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutoMenu.activeInHierarchy)
        {
            if (play.GetButtonDown("UICancel"))
            {
                mainMenu.SetActive(true);
                tutoMenu.SetActive(false);
                actualObjet = mainMenu;
                EventSystem.current.SetSelectedGameObject(firstMenu);
            }
        }
    }

    public void OnClickPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickTuto()
    {
        mainMenu.SetActive(false);
        tutoMenu.SetActive(true);
        actualObjet = tutoMenu;
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
