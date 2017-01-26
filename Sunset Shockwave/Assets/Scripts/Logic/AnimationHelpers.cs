using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for generic objects to call external functions.
public class AnimationHelpers : MonoBehaviour {

    // Respawns the player's ball in the world.
    void RespawnBall(PlayerTeam playerTeam)
    {
        GameManager.gameManager.RespawnBall(playerTeam);
        gameObject.SetActive(false);
    }

    // Tells the Main Menu to fade the logo.
    void HideLogoStartScene()
    {
        FindObjectOfType<MainMenu>().HideLogo();
    }

    // Tells the game maker to begin the game.
    void BeginGame()
    {
        GameManager.gameManager.StartGame();
    }

    // Show Restart Screen.
    void ShowRestartScreen()
    {
        gameObject.SetActive(false);
        GameManager.gameManager.ShowRestartMessage();
    }

    // Destroys the GameObject.
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    // Plays an helper sound.
    void PlayHelperSound(AudioClip audio)
    {
        GameManager.gameManager.PlayHelperAudio(audio);
    }
}
