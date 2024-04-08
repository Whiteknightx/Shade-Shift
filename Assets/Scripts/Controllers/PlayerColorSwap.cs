using System.Linq;
using UnityEngine;

public class PlayerColorSwap : MonoBehaviour
{
    public KeyCode swapColorKey = KeyCode.Space;
    public Color whiteColor = Color.white;
    public Color blackColor = Color.black;
    public Gradient trailColorWhite = new Gradient();
    public Gradient trailColorBlack = new Gradient();

    public bool isWhite = true;

    public float obstacleDetectionRadius;

    public LayerMask GroundLayerMask;
    public LayerMask PlayerLayerMask;

    int noofobs;

    public void Update()
    {
        CheckObstacleInteraction();
    }

    private void CheckObstacleInteraction()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, obstacleDetectionRadius, GroundLayerMask);

        hitColliders = hitColliders.Where(collider => collider.CompareTag("whiteObstacle") || collider.CompareTag("blackObstacle")).ToArray();

        noofobs = hitColliders.Length;
        print(noofobs);

        foreach (Collider2D collider in hitColliders)
        {
            string obstacleColourTag = collider.tag;

            if (isWhite)
            {
                if (obstacleColourTag == "whiteObstacle")
                {
                    collider.excludeLayers = LayerMask.NameToLayer("Nothing");
                }
                else if (obstacleColourTag == "blackObstacle")
                {
                    collider.excludeLayers = PlayerLayerMask;
                }
            }
            else // If not white, assume black
            {
                if (obstacleColourTag == "whiteObstacle")
                {
                    collider.excludeLayers = PlayerLayerMask;
                }
                else if (obstacleColourTag == "blackObstacle")
                {
                    collider.excludeLayers = LayerMask.NameToLayer("Nothing");
                }
            }
        }
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, obstacleDetectionRadius);
    }
}
