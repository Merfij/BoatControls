using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private PlayerScript playerScript;

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Boat").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.position = playerScript.currentCheckPoint;
            Debug.Log(playerScript.currentCheckPoint.ToString());
        }
    }
}
