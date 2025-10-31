using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private Mover mover;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mover = GetComponent<Mover>();
    }

    private void Start()
    {
        var movers = FindObjectsByType<Mover>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        var index = playerInput.playerIndex;
        mover = movers.ToList().FirstOrDefault(m => m.GetPlayerIndex() == index);
    }

    public void OnJump(CallbackContext context)
    {
        if(mover != null)
        {
            mover.Jump();
        }
        Debug.Log("Jump!");
    }
}
