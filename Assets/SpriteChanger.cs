using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    // This public variable will show up in the Inspector.
    // We will drag the new sprite we want the player to have into this slot.
    public Sprite newPlayerSprite;

    // This function runs automatically when another collider enters our trigger zone.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // First, we check if the object that entered is the Player.
        // We use CompareTag for this, as it's very efficient.
        if (other.CompareTag("Player"))
        {
            // If it is the player, we get the FinalPlayerController script from it.
            FinalPlayerController playerController = other.GetComponent<FinalPlayerController>();

            // A quick safety check to make sure the script was found.
            if (playerController != null)
            {
                // --- THIS IS THE LOGIC ---

                // 1. Call the public "ChangeSprite" function on the player's script.
                playerController.ChangeSprite(newPlayerSprite);
                
                // 2. Call the new public "EnableShooting" function on the player's script.
                playerController.EnableShooting();
                
                // 3. Now that the power-up has been used, we destroy this square.
                Destroy(gameObject);
            }
        }
    }
}