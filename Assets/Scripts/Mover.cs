using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private int playerIndex = 0;
    [SerializeField] Transform boat;
    [SerializeField] bool hasJumped = false;

    public int GetPlayerIndex()
    { 
        return playerIndex; 
    }

    private void Update()
    {

    }

    public void Jump()
    {
        if (hasJumped == false)
        {
            boat.position += boat.transform.up * 10;
            hasJumped = true;
            Debug.Log("JUMP JUMP JUMP");
        }
        
    }

}
