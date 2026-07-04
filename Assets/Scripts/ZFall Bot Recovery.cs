using System.Collections.Generic;
using UnityEngine;

public class ZFallBotRecovery : MonoBehaviour
{
    [SerializeField] private List<GameObject> teleporters;
    [SerializeField] private GameObject Player;
    [SerializeField] private float sampleInterval = 0.5f;
    [SerializeField] private float floorTolerance = 2f;
    private Vector3 pastPlayerPosition;
    private float sampleTimer;
    private bool isFalling;

    private void Start()
    {
        pastPlayerPosition = Player.transform.position;
        sampleTimer = 0f;
        isFalling = false;
    }
    private void Update()
    {
        Vector3 currentPlayerPosition = Player.transform.position;

        sampleTimer += Time.deltaTime;
        if (sampleTimer < sampleInterval)
            return;
        sampleTimer = 0f;

        float deltaY = currentPlayerPosition.y - pastPlayerPosition.y;

        if (Mathf.Abs(deltaY) > 5f)
        {
            // Still dropping fast between samples: keep waiting, don't pick a floor mid-fall.
            isFalling = true;
        }
        else if (isFalling)
        {
            isFalling = false;
            Debug.Log("Player has landed after falling, teleporting bot to closest teleporter on this floor");
            GameObject closestTeleporter = null;
            float minDist = Mathf.Infinity;
            foreach (GameObject teleporter in teleporters)
            {
                Vector3 teleporterPosition = teleporter.transform.position;

                if (Mathf.Abs(teleporterPosition.y - currentPlayerPosition.y) > floorTolerance)
                    continue;

                Vector3 currentFlat = new(currentPlayerPosition.x, 0f, currentPlayerPosition.z);
                Vector3 teleporterFlat = new(teleporterPosition.x, 0f, teleporterPosition.z);
                float dist = Vector3.Distance(currentFlat, teleporterFlat);
                if (dist < minDist)
                {
                    minDist = dist;
                    closestTeleporter = teleporter;
                    Debug.Log("Closest teleporter found at position: " + teleporterPosition);
                }

            }
            if (closestTeleporter != null)
            {
                GameObject botGatewayObject = closestTeleporter.transform.GetChild(1).gameObject;
                Teleport botGateway = botGatewayObject.GetComponent<Teleport>();
                botGateway.TeleportBot();
                Debug.Log("Bot teleported to closest teleporter at position: " + closestTeleporter.transform.position);
            }
        }

        pastPlayerPosition = currentPlayerPosition;
    }
}
