using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int idPlayer = 0;
    private Player player;

    [Header("Move")]
    public float speedMax;
    public float acceleration;
    public float friction;

    private Vector2 speed = Vector2.zero;

    private Rigidbody _rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(idPlayer);
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = player.GetAxis("HorizontalMove");
        Vector2 move = new Vector2(dirX, 0);
        Move(move);
    }

    public void Move(Vector2 moveDir)
    {
        if (moveDir != Vector2.zero)
        {
            speed += moveDir * acceleration * Time.deltaTime;
            if (speed.sqrMagnitude >= speedMax * speedMax)
            {
                speed = speed.normalized * speedMax;
            }
        }
        else if (speed != Vector2.zero)
        {
            Vector2 frictionDir = speed.normalized;
            float frictionToApply = friction * Time.deltaTime;
            if (speed.sqrMagnitude <= frictionToApply * frictionToApply)
            {
                speed = Vector2.zero;
            }
            else
            {
                speed -= frictionToApply * frictionDir;
            }
        }

        _rigidbody.velocity = new Vector3(speed.x, speed.y, 0);

    }
}
