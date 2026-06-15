using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaMove : MonoBehaviour
{
    public int sceneBuildIndex;
    public Vector2 playerPosition;
    public VectorValue playerPermanence;

    // Area move zoned enter, if collider is a player
    // Move game to another scene
    public void OnTriggerEnter2D(Collider2D other)
    {
        print("Trigger Entered");

        // Could use other.GetComponent<Player>() to see if the game object has a Player component
        // Tags work too. Maybe some players have different script components?
        if (other.CompareTag("Player") && !other.isTrigger) 
        {
            // Player entered, so move level
            print("Switching Scene to " + sceneBuildIndex);
            playerPermanence.initialValue = playerPosition;
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}