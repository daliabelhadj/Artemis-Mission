using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    public NPCDialogue npcDialogue;
    public string[] lines;
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            npcDialogue.ShowNPC(lines);
        }
    }
}
