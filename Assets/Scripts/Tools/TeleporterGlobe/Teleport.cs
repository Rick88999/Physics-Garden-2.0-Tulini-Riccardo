using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Bot;
    [Header("Target")]
    [SerializeField] private Vector3 targetPositionPlayer;
    [SerializeField] private Vector3 targetPositionBot;

    private BotController botController;

    private void Awake()
    {
        if (Bot != null)
            botController = Bot.GetComponent<BotController>();
    }

    public void TeleportPlayer()
    {
        if (player != null)
            player.transform.position = targetPositionPlayer;
    }

    public void TeleportBot()
    {
        if (Bot == null) return;

        if (botController != null)
        {
            botController.TeleportTo(targetPositionBot);
            Debug.Log("Bot teleported to: " + targetPositionBot);
            botController.ContiuneFollow();
        }
        else
        {
            Bot.transform.position = targetPositionBot;
            Debug.Log("Bot teleported (no NavMesh) to: " + targetPositionBot);
        }
    }
}
