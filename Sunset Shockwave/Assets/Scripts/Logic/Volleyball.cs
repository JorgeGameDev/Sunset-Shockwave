using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for managing the volley ball, and it's properties, velocity, you know the drill.
public class Volleyball : MonoBehaviour
{
    [Header("Volleybal Proprieties")]
    public float ballForce;

    [Header("Shadows")]
    public Sprite[] shadowSprites;
    public GameObject ballShadow;
    private int _currentSprite;
    private SpriteRenderer _ballShadowRenderer;

    // Internal.
    private Rigidbody2D _rigidbody;
    private bool _respawned;

    // Use this for pre-render initialization
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Use this for initalization
    void Start()
    {
        _ballShadowRenderer = ballShadow.GetComponent<SpriteRenderer>();
    }

    // Used for reseting the ball state.
    public void ResetState()
    {
        _respawned = false;
    }

    // Update is called every frame
    void Update()
    {
        BallShadows();
    }

    // Does the ball's dynamic shadows. This is the future.
    void BallShadows()
    {
        if(transform.position.y > 1 && _currentSprite != 1)
        {
            _currentSprite = 1;
            _ballShadowRenderer.sprite = shadowSprites[0];
        }
        else if(transform.position.y < 1 && transform.position.y > -2.5f && _currentSprite != 2)
        {
            _currentSprite = 2;
            _ballShadowRenderer.sprite = shadowSprites[1];
        }
        else if(transform.position.y < -2.5f && _currentSprite != 3)
        {
            _currentSprite = 3;
            _ballShadowRenderer.sprite = shadowSprites[2];
        }

        // Sets the shadow to follow the ball.
        ballShadow.transform.position = new Vector3(transform.position.x, ballShadow.transform.position.y, 0);
    }

    // Used for setting the body type from outside.
    public void SetBodyType(RigidbodyType2D bodyType)
    {
        _rigidbody.bodyType = bodyType;
    }

    // Used for setting the velocity and direction of the ball from outside.
    public void ApplyBallForce(Vector2 force)
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(force * ballForce * 100f);
    }

    // Sets the ball gravity.
    public void SetGravity(float gravity)
    {
        _rigidbody.gravityScale = gravity;
    }

    // Used for setting the velocity and direction of the ball from outside.
    public void ApplyBallForce(Vector2 force, float strenght)
    {
        _rigidbody.velocity = Vector2.zero;
        SetGravity(1.48f);
        _rigidbody.AddForce(force * ballForce * strenght);
    }

    // Checks if the ball has entered contact with the floor.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Checks if the ball hasn't touched the floor yet.
        if(!_respawned && collision.gameObject.CompareTag("Ground"))
        {
            // Shows that the ball has touched the floor.
            _respawned = true;

            // Checks which side of the ground the ball hit.
            if(transform.position.x < 0)
            {
                GameManager.gameManager.AddTeamScore(PlayerTeam.BikiniBosses);
            }
            else if(transform.position.x > 0)
            {
                GameManager.gameManager.AddTeamScore(PlayerTeam.SuperSpeedo);
            }
            else // Fail Safe.
            {
                GameManager.gameManager.AddTeamScore(PlayerTeam.None);
            }
        }
    }

    // Gets the state of the ball.
    public bool ReturnBallState()
    {
        return _respawned;
    }
}
