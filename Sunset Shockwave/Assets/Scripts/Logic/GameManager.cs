using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

// Manages Game Rules and how the game should play out.
public class GameManager : MonoBehaviour {

    // Static Reference to the Game Manager.
    public static GameManager gameManager;

    [Header("Paused")]
    [ReadOnly]
    public bool isTitle = true;
    [ReadOnly]
    public bool isPaused;
    [ReadOnly]
    public bool gameFinished;
    public GameObject pauseMenu;
    public GameObject restartMenu;

    [Header("Reference Objects")]
    public GameObject speedoGuy;
    public GameObject bikiniGal;

    [Header("Volleyball")]
    public GameObject volleyBall;
    [ReadOnly]
    public Volleyball volleyballScript;

    [Header("Particle Systems")]
    public GameObject speedoParticles;
    public GameObject bikiniParticles;

    [Header("Scores")]
    public int necessaryScore;
    [ReadOnly]
    public int speedoScore;
    [ReadOnly]
    public int bikiniScore;

    [Header("Audio Sources")]
    public AudioSource helperSource;
    public AudioClip scoreSound;
    public AudioClip winSound;
    public AudioClip scoreAnnounce;
    public AudioClip speedoWinAnnounce;
    public AudioClip bikiniWinAnnounce;

    [Header("Other")]
    public float joystickDeadzone;

    #region Canvas Elements
    [Header("Canvas Elements")]
    public GameObject countdown;
    public Text speedoScoreText;
    public Text bikiniScoreText;
    public GameObject speedoScoreNotification;
    public GameObject bikiniScoreNotification;
    public GameObject speedoWinNotification;
    public GameObject bikiniWinNotification;
    public Text shortsWinText;
    public Text bikiniWinText;
    #endregion

    // Internal
    private AudioSource _audioSource;
    private Vector3 _volleyStart;

    // Use this for pre-render initialization
    void Awake ()
    {
        // Sets this as the Game Manager singleton.
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start ()
    {
        // Gets Components.
        _audioSource = GetComponent<AudioSource>();
        _volleyStart = volleyBall.transform.position;
        volleyballScript = volleyBall.GetComponent<Volleyball>();
	}

    // Update is called every frame
    void Update()
    {
        // Checks if the game state.
        if(!isPaused && !isTitle)
        {
            // Checks if the game should be paused.
            if(XCI.GetButtonDown(XboxButton.Start, XboxController.First) || XCI.GetButtonDown(XboxButton.Start, XboxController.Second) || Input.GetButtonDown("Cancel"))
            {
                Time.timeScale = 0;
                isPaused = true;
                pauseMenu.SetActive(true);
            }
        }
        else
        {
            // Checks if the player wants to resume the game.
            if (XCI.GetButtonDown(XboxButton.Start, XboxController.First) || XCI.GetButtonDown(XboxButton.Start, XboxController.Second) || 
                XCI.GetButtonDown(XboxButton.B, XboxController.First) || XCI.GetButtonDown(XboxButton.B, XboxController.Second))
            {
                Time.timeScale = 1;
                isPaused = false;
                pauseMenu.SetActive(false);
            }
            
            // Checks if the player want to quit the game.
            if(XCI.GetButtonDown(XboxButton.Back, XboxController.First) || XCI.GetButtonDown(XboxButton.Back, XboxController.Second) || Input.GetButtonDown("Cancel"))
            {

            }
        }
        
        // Check if the player is on the restart screen.
        if(gameFinished && isTitle)
        {
            if (XCI.GetButtonDown(XboxButton.Start, XboxController.First) || XCI.GetButtonDown(XboxButton.Start, XboxController.Second))
            {
                // Reloads the scene.
                SceneManager.LoadSceneAsync(0);
            }
        }
    }

    // Begins the game. Should start after the beggining animation.
    public void BeginGameSection()
    {
        // Starts a countdown animation before beggining the game.
        countdown.SetActive(true);
    }

    // Starts the actual game after the countdown.
    public void StartGame()
    {
        // Actually starts the game.
        isTitle = false;
        countdown.SetActive(false);
        RespawnBall(PlayerTeam.None);
    }

    // Respawns the ball at the middle position again.
    public void RespawnBall(PlayerTeam pastGoalTeam)
    {
        // Sets the ball position on the beggining.
        volleyBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        volleyBall.transform.position = _volleyStart;

        // Picks a team to be considered for RNG.
        if(pastGoalTeam == PlayerTeam.None)
        {
            int randomTeam = Random.Range(0, 2);
            if(randomTeam == 0)
            {
                // The ball goes toward's shorts side.
                pastGoalTeam = PlayerTeam.SuperSpeedo;
            }
            else
            {
                // The ball goes toward's bikini side.
                pastGoalTeam = PlayerTeam.BikiniBosses;
            }
        }

        // Depending on the team that won or the one picked does a different throw.
        if (pastGoalTeam == PlayerTeam.SuperSpeedo)
        {
            // Throws the ball to the bikini side. 
            volleyballScript.ApplyBallForce(Vector2.right, Random.Range(25f, 45f));
        }
        else if(pastGoalTeam == PlayerTeam.BikiniBosses)
        {
            // Throws the ball to the shorts side.
            volleyballScript.ApplyBallForce(Vector2.left, Random.Range(25f, 45f));
        }

        // Resets the ball state.
        volleyballScript.ResetState();
    }
	
	// Adds a score the specific team.
    public void AddTeamScore(PlayerTeam team)
    {
        // Adds a score to a certain team.
        if(team == PlayerTeam.SuperSpeedo)
        {
            speedoScore++;
            speedoScoreText.text = speedoScore + " - " + bikiniScore;

            // Checks if this teams wins the match.
            if (speedoScore < necessaryScore)
            {
                PlayHelperAudio(scoreAnnounce);
                _audioSource.PlayOneShot(scoreSound);
                speedoScoreNotification.SetActive(true);
            }
            else
            {
                // Sets the game as finished and the characther animations.
                gameFinished = true;
                shortsWinText.text = speedoScore + " - " + bikiniScore;
                speedoGuy.GetComponent<PlayerController>().SetWinLoseAnimation(true);
                bikiniGal.GetComponent<PlayerController>().SetWinLoseAnimation(false);
                _audioSource.PlayOneShot(winSound);
                PlayHelperAudio(speedoWinAnnounce);
                speedoParticles.SetActive(true);
                speedoWinNotification.SetActive(true);
            }
        }
        else if(team == PlayerTeam.BikiniBosses)
        {
            bikiniScore++;
            bikiniScoreText.text = speedoScore + " - " + bikiniScore;

            // Checks if this teams wins the match.
            if (bikiniScore < necessaryScore)
            {
                PlayHelperAudio(scoreAnnounce);
                _audioSource.PlayOneShot(scoreSound);
                bikiniScoreNotification.SetActive(true);
            }
            else
            {

                // Sets the game as finished and the characther animations.
                gameFinished = true;
                bikiniWinText.text = speedoScore + " - " + bikiniScore;
                speedoGuy.GetComponent<PlayerController>().SetWinLoseAnimation(false);
                bikiniGal.GetComponent<PlayerController>().SetWinLoseAnimation(true);
                _audioSource.PlayOneShot(winSound);
                PlayHelperAudio(bikiniWinAnnounce);
                bikiniParticles.SetActive(true);
                bikiniWinNotification.SetActive(true);
            }
        }
    }

    // Shows the reset menssage at the end of the game.
    public void ShowRestartMessage()
    {
        isTitle = true;
        restartMenu.SetActive(true);
    }

    // Plays an helper sound.
    public void PlayHelperAudio(AudioClip audio)
    {
        helperSource.PlayOneShot(audio);
    }
}

// Defines the player team. Don't ask regarding the names.
public enum PlayerTeam
{
    None,
    SuperSpeedo,
    BikiniBosses,
}
