using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float dashTime;
    [SerializeField] private float _dashTimeLeft;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (DashTimer())
        {
            ResetDashTimer();
        }
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
}
