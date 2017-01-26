using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

// Used for defining the player's information and control behaviour.
public class PlayerController : MonoBehaviour {

    [Header("Player Information")]
    public XboxController playerController;
    public PlayerTeam playerTeam;

    [Header("Movement & Forces")]
    public GameObject stepGameObject;
    public GameObject flashParticles;
    public float speed;
    public float psychoRange;
    public float JumpForce;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Shockwave!")]
    public float maxForce = 2f;
    public float maxGravity = 2.48f;
    public float minForce = 1f;
    public float minGravity = 1.48f;

    [Header("Volleyball & Telekenisis")]
    public LayerMask volleyMask;
    public GameObject shadow;

    [Header("Audio Files")]
    public AudioClip jumpSound;
    public AudioClip shockWaveSound;
    public AudioClip telekenisisSound;
    public AudioClip landSound;
    public AudioClip readySound;

    // Internal GameObject Rotation.
    private GameObject _psychoCircle;
    private GameObject _rotationSprite;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _ballDirection;
    private Animator _animator;

    // Internal Telekenisis.
    private GameObject _heldBall;
    private bool _triggerHolding;
    private bool _overdrive;
    private bool _hasJumped;
    private int _steps;

    // Others
    private AudioSource _audioSource;
    private float _finalForce;
    private float _finalGravity;

    // Use this for initialization
    void Start ()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _psychoCircle = transform.GetChild(0).gameObject;
        _rotationSprite = _psychoCircle.transform.GetChild(0).gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!GameManager.gameManager.isPaused && !GameManager.gameManager.isTitle 
        && !GameManager.gameManager.gameFinished)
        {
            if (!_triggerHolding)
            {
                JoystickMovement();
            }
            AnimatePlayer();
            JoystickRotation();
            TriggerInput();
        }
	}

    // Late Update is called at the end of the frame.
    void LateUpdate()
    {
        ClampPosition();
    }

    // Used for player movement by joystick input.
    void JoystickMovement ()
    {
        // Gets the axis input by the player.
        Vector2 inputAxis = Vector2.zero;
        inputAxis.x = XCI.GetAxis(XboxAxis.LeftStickX, playerController);
        inputAxis.y = XCI.GetAxis(XboxAxis.LeftStickY, playerController);

        // Applies to the body.
        _rigidbody2D.velocity = new Vector2(inputAxis.x * speed, _rigidbody2D.velocity.y);

        // Checks if the player should jump if the trigger is high enough.
        if(!_hasJumped)
        {
            // Checks if the player is grounded.
            if (IsGrounded())
            {
                // Adds to the steps if the player has inputed on the X axis.
                if (inputAxis.x != 0)
                {
                    CreateFootSteps();
                }

                // Checks if the player has jumped.
                if (inputAxis.y > 0.6f || XCI.GetButtonDown(XboxButton.A, playerController))
                {
                    _hasJumped = true;
                    _animator.SetBool("Grounded", false);
                    _audioSource.PlayOneShot(jumpSound);
                    _rigidbody2D.AddForce(new Vector2(0, JumpForce * 100f));
                }
            }
        }
        else
        {
            if(_rigidbody2D.velocity.y == 0 && IsGrounded())
            {
                _audioSource.PlayOneShot(landSound);
                _animator.SetBool("Grounded", true);
                _hasJumped = false;
            }
        }
    }

    // Creates footsteps on the sand.
    private void CreateFootSteps()
    {
        // Adds to the steps of footsteps.
        _steps += 1;

        // Checks if the steps are enough to create some footsteps.
        if (_steps >= 10)
        {
            // Resets the steps and places the footstep objects.
            _steps = 0;
            _audioSource.PlayOneShot(landSound);
            Vector3 stepPosition = new Vector3(transform.position.x, transform.position.y - 1, 0);
            GameObject footsteps = Instantiate(stepGameObject, stepPosition, Quaternion.identity);

            // Checks the team to flip footsteps.
            if(playerTeam == PlayerTeam.BikiniBosses)
            {
                footsteps.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    // Animates the player.
    void AnimatePlayer()
    {
        // Sets movement floats based on moving and walking speed.
        _animator.SetFloat("MoveSpeed", _rigidbody2D.velocity.x == 0 ? 0f : 1f);
        _animator.SetFloat("JumpSpeed", _rigidbody2D.velocity.y < 0 ? -1f : 1f);
    }

    // Checks if the player is grounded.
    bool IsGrounded()
    {
        // Does a physics check to check if the player is touching the ground.
        return Physics2D.CircleCast(groundCheck.position, 0.01f, Vector2.right, 0f, groundMask);
    }

    // Used for rotating the direction the ball should be fired at.
    void JoystickRotation()
    {
        // Gets the position of the player's joystick.
        Vector2 rotationInput = Vector2.zero;
        rotationInput.x = XCI.GetAxis(XboxAxis.RightStickX, playerController);
        rotationInput.y = XCI.GetAxis(XboxAxis.RightStickY, playerController);

        // Returns the position in angles.
        if (rotationInput.magnitude < GameManager.gameManager.joystickDeadzone)
        {
            rotationInput = Vector2.zero;
        }
        else
        {
            rotationInput = rotationInput.normalized * ((rotationInput.magnitude - GameManager.gameManager.joystickDeadzone) / (1 - GameManager.gameManager.joystickDeadzone));
            float angle = Mathf.Atan2(rotationInput.y, rotationInput.x) * Mathf.Rad2Deg;
            _rotationSprite.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.time);
        }
    }

    // Clamps the Player's Position depending on their team.
    void ClampPosition()
    {
        // Get's the current player position.
        Vector2 currentPosition = transform.position;

        // Clamps the X position depending on the player team.
        if(playerTeam == PlayerTeam.SuperSpeedo)
        {
            currentPosition.x = Mathf.Clamp(currentPosition.x, -7f, -0.5f);
        }
        else
        {
            currentPosition.x = Mathf.Clamp(currentPosition.x, 0.5f, 7f);
        }

        // Clamps the player to the horizontal field.
        currentPosition.y = Mathf.Clamp(currentPosition.y, -6f, 6f);
        transform.position = currentPosition;

        // Sets the shadow to follow the player.
        shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y, 0);

        // Removes the held ball from the player if he is holding.
        if(_heldBall != null && GameManager.gameManager.volleyballScript.ReturnBallState())
        {
            _heldBall = null;
        }
    }

    // Checks if the player has the the volley ball on their range to hold it.
    void TriggerInput()
    {
        // Checks if the player is holding any of the triggers.
        if (!_triggerHolding && !_overdrive)
        {
            if (XCI.GetAxisRaw(XboxAxis.LeftTrigger, playerController) != 0 || XCI.GetAxisRaw(XboxAxis.RightTrigger, playerController) != 0)
            {
                StartTelekenisis();
            }
        }
        else if(_triggerHolding)
        {
            // Effects that are applied during holding the effect.
            ApplyTelekenisis();

            // Checks if the player has unhold the trigger.
            if (XCI.GetAxisRaw(XboxAxis.LeftTrigger, playerController) == 0 && XCI.GetAxisRaw(XboxAxis.RightTrigger, playerController) == 0)
            {
                StopTelekenisis(false);
            }
        }
    }

    // Used for starting holding the effect.
    void StartTelekenisis()
    {
        // Makes the player's body static and stores the trigger as being held.
        _triggerHolding = true;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _audioSource.PlayOneShot(telekenisisSound);
        _animator.SetBool("Telekenisis", true);

        // Starts the timer for the telekenisis to grow.
        psychoRange = 1f;
        _psychoCircle.SetActive(true);
        StartCoroutine(TelekenisisTimer());
    }

    // Used during the telekenisis effect.
    void ApplyTelekenisis()
    {
        // Does Raycasting to check if there's a ball on the player's range.
        RaycastHit2D ballHit = Physics2D.CircleCast(_psychoCircle.transform.position, psychoRange, Vector2.right, 0, volleyMask);
        if (ballHit.collider != null && !GameManager.gameManager.volleyballScript.ReturnBallState())
        {
            _heldBall = ballHit.collider.gameObject;
            GameManager.gameManager.volleyballScript.SetBodyType(RigidbodyType2D.Static);
        }
    }

    // Used for stoping the telekenisis.
    void StopTelekenisis(bool forced)
    {
        // Sets the body back to original properties.
        _triggerHolding = false;   
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _psychoCircle.SetActive(false);
        _audioSource.PlayOneShot(shockWaveSound);
        _animator.SetBool("Telekenisis", false);
        StartCoroutine(OverdriveTimer());

        // Checks if the player is holding a ball.
        if (_heldBall != null)
        {
            // Sets the ball back to dynamic.
            GameManager.gameManager.volleyballScript.SetBodyType(RigidbodyType2D.Dynamic);

            // Ignores giving the ball force if the player failed to throw the ball on time.
            if (!forced)
            {
                Debug.Log("Applied Modifiers! | <color=red>Gravity " + _finalGravity + "</color> | <color=blue>Force " + _finalForce + "</color>");
                GameManager.gameManager.volleyballScript.SetGravity(_finalGravity);
                GameManager.gameManager.volleyballScript.ApplyBallForce(_rotationSprite.transform.right * _finalForce);
            }

            // Clears ball.
            _heldBall = null;
        }
    }

    // Timer used for the growth of the circle range.
    IEnumerator TelekenisisTimer()
    {
        // Sets the values at the beggining of telekenisis.
        _finalForce = maxForce;
        _finalGravity = maxGravity;

        // Calculates the decrements.
        float forceDecrement = (maxForce - minForce) / 10f;
        float gravityDecrement = (maxGravity - minGravity) / 10f;

        // Starts couting the time until three seconds have passed.
        for (float i = 0; i < 1.5f; i += 0.1f)
        {
            // Breaks the coroutine if holding as stopped.
            if(!_triggerHolding)
            {
                break;
            }

            // Adds to the psycho range until a limit.
            if(psychoRange < 2f)
            {
                psychoRange += 0.2f;
            }

            // Removes the extra force each third of a second.
            if(i < 1f)
            {
                _finalForce -= forceDecrement;
                _finalGravity -= gravityDecrement;
            }

            // Stops timer for a second.
            yield return new WaitForSeconds(0.1f);
        }

        // Timer has overflown. Time to stop.
        if(_triggerHolding)
        {
            StopTelekenisis(true);
        }
    }

    // Timer used for cooling down the player's action.
    IEnumerator OverdriveTimer()
    {
        // Begins the overdrive effect.
        _overdrive = true;
        flashParticles.SetActive(true);
        _audioSource.PlayOneShot(readySound);
        yield return new WaitForSeconds(1.2f);
        flashParticles.SetActive(false);
        _overdrive = false;
    }

    // Set Win and Lose Animations for the player.
    public void SetWinLoseAnimation(bool win)
    {
        // Checks if the player won to toggle the animator.
        if(win)
        {
            _animator.SetBool("Victory", true);
        }

        // Sets animator to finished property.
        _animator.SetBool("Finished", true);
    }

    // Debug gizmos to know the player's range and direction.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, psychoRange);
    }
}
