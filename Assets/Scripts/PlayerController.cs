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
    private Vector2 collisionSide;

    [Header("Dash")]
    public float dashDuration = 1f;
    public float dashSpeed = 30f;
    private float dashCountdown = 1f;
    private Vector2 dashDir;
    private bool isDashing = false;

    [Header("Target")]
    public GameObject target;
    private Vector2 orientTarget = Vector2.right;

    public bool isDebugMode;
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
        if (isDashing)
        {
            UpdateDash();
        }
        else
        {
            float dirX = player.GetAxis("HorizontalMove");
            Vector2 move = new Vector2(dirX, 0);
            Move(move);

            float moveX = player.GetAxis("HorizontalTarget");
            float moveY = player.GetAxis("VerticalTarget");
            Vector2 moveTarget = new Vector2(moveX, moveY);
            UpdateTarget(moveTarget);

            if (dirX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                target.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (dirX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                target.transform.localScale = new Vector3(1, 1, 1);
            }
        }        
    }

    private void OnGUI()
    {
        if (!isDebugMode)
        {
            return;
        }

        GUILayout.BeginVertical();
        GUILayout.Label("" + orientTarget);
        GUILayout.Label("" + orientTarget.normalized);
    }

    private void Move(Vector2 moveDir)
    {
        if (moveDir != Vector2.zero)
        {
            speed += moveDir * acceleration * Time.deltaTime;
            if (speed.sqrMagnitude >= speedMax * speedMax)
            {
                speed = speed.normalized * speedMax;
            }
            if(!AudioManager.audioManager.IsPlaying("footstep"))
                AudioManager.audioManager.Play("footstep");
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


        if (Mathf.Sign(speed.x) == Mathf.Sign(collisionSide.x) && collisionSide.x != 0)
        {
            speed.x = 0;
        }

        _rigidbody.velocity = new Vector3(speed.x, speed.y, 0);
    }

    private void UpdateTarget(Vector2 orientDir)
    {
        if (orientDir != Vector2.zero)
        {
            orientTarget = orientDir;
        }

        float angle = Vector2.SignedAngle(Vector2.right, orientTarget);
        Vector3 eulerAngles = target.transform.eulerAngles;
        eulerAngles.z = angle;
        target.transform.eulerAngles = eulerAngles;

        //calcul du vecteur d'orienta

    }

    private void UpdateDash()
    {
        if (!isDashing)
        {
            return;
        }

        dashCountdown -= Time.deltaTime;
        if(dashCountdown <= 0)
        {
            isDashing = false;
        }
        else
        {
            _rigidbody.velocity = new Vector3(dashDir.x * dashSpeed, dashDir.y * dashSpeed, 0);
        }
    }

    public void Dash()
    {
        dashDir = orientTarget.normalized;
        dashCountdown = dashDuration;
        isDashing = true;
        AudioManager.audioManager.Play("dash");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (Mathf.Sign(speed.x) == Mathf.Sign(collision.GetContact(0).point.x - transform.position.x))
            {
                if(Mathf.Sign(speed.x) > 0)
                {
                    collisionSide = Vector2.right;
                }
                else
                {
                    collisionSide = Vector2.left;
                }
                speed.x = 0;
                _rigidbody.velocity = new Vector3(0, speed.y, 0);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            collisionSide = Vector2.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))
        {
            // Mort du personnage
            // SFX, VFX, Particules, etc...
            GameManager.instance.Respawn();
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (Mathf.Sign(speed.x) == Mathf.Sign(collision.GetContact(0).point.x - transform.position.x))
            {
                if (Mathf.Sign(speed.x) > 0)
                {
                    collisionSide = Vector2.right;
                }
                else
                {
                    collisionSide = Vector2.left;
                }
                speed.x = 0;
                _rigidbody.velocity = new Vector3(speed.x, speed.y, 0);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.CompareTag("Finish"))
        {
            AudioManager.audioManager.Play("ending");
            GameManager.instance.FinishLevel();
        }
    }
}
