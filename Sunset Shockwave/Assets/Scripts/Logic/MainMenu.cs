using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

// Used for inputs on the main menu, like announcing you are inputed.
public class MainMenu : MonoBehaviour {

    [Header("Canvas")]
    public GameObject palmTrees;

    [Header("Text")]
    public Text startText;
    public Text firstPlayerText;
    public Text secondPlayerText;

    [Header("Audio Source")]
    public AudioClip joinSound;
    public AudioClip selectSound;

    // Interal.
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _firstPlayerReady;
    private bool _secondPlayerReady;
    private bool _requiredControlsPlugged;
    private bool _startSequence;

	// Use this for initialization
	void Start ()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        CheckControllers();
	}

    // Used to check the numbers of plugged controls.
    void CheckControllers()
    {
        // Checks if there are at least two controls plugged.
        if ((XCI.GetNumPluggedCtrlrs() >= 2) && !_requiredControlsPlugged)
        {
            _requiredControlsPlugged = true;
            startText.text = "Press START to Shockwave!";
            firstPlayerText.gameObject.SetActive(true);
            secondPlayerText.gameObject.SetActive(true);
        }
        else if((XCI.GetNumPluggedCtrlrs() < 2) && _requiredControlsPlugged)
        {
            _requiredControlsPlugged = false;
            startText.text = "Please Connect Two Xbox Controllers to Shockwave!";
            firstPlayerText.gameObject.SetActive(false);
            secondPlayerText.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!_startSequence)
        {
            // Checked for plugged controlers and ready.
            CheckControllers();
            if (_requiredControlsPlugged)
            {
                ReadyNotReady();
            }

            // Checks if both players have pressed start.
            if (_firstPlayerReady && _secondPlayerReady)
            {
                // Sets the start sequence.
                _audioSource.PlayOneShot(selectSound);
                _startSequence = true;

                // Starts the menu animation.
                _animator.SetTrigger("Begin");
            }
        }

        // Checks if the player want to quit the game.
        if (XCI.GetButtonDown(XboxButton.Back, XboxController.First) || XCI.GetButtonDown(XboxButton.Back, XboxController.Second) || Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    // Used for moving the palm trees.
    public void MovePalmTrees()
    {
        palmTrees.GetComponent<Animator>().enabled = true;
    }

    // Sets the animator to hide the loog.
    public void HideLogo()
    {
        // Hides the logo.
        _animator.SetTrigger("HideLogo");
    }

    // Loads the game scene.
    public void StartGame()
    {
        GameManager.gameManager.BeginGameSection();
    }

    // Checks if a player is or is not ready.
    void ReadyNotReady()
    {
        // Checks if the first player is ready.
        if(XCI.GetButtonDown(XboxButton.Start, XboxController.First) || XCI.GetButtonDown(XboxButton.A, XboxController.First))
        {
            _audioSource.PlayOneShot(joinSound);
            if(!_firstPlayerReady)
            {
                _firstPlayerReady = true;
                firstPlayerText.text = "Super Speedo\n<color=#2ecc71>Ready!</color>";
            }
            else
            {
                _firstPlayerReady = false;
                firstPlayerText.text = "Super Speedo\n<color=#e74c3c>Not Ready</color>";
            }
        }

        // Checks if the second player is ready.
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.Second) || XCI.GetButtonDown(XboxButton.A, XboxController.Second))
        {
            _audioSource.PlayOneShot(joinSound);
            if (!_secondPlayerReady)
            {
                _secondPlayerReady = true;
                secondPlayerText.text = "Bikini Boss\n<color=#2ecc71>Ready!</color>";
            }
            else
            {
                _secondPlayerReady = false;
                secondPlayerText.text = "Bikini Boss\n<color=#e74c3c>Not Ready</color>";
            }
        }
    }
}
