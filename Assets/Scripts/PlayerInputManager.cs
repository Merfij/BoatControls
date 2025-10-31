using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerInputManager : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnpoints;
    private HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();

    private bool playerOneJoined = false;
    private bool playerTwoJoined = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Update is called once per frame
    void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamepad))
            {
                //var player = PlayerInput.Instantiate(playerPrefab, controlScheme: "Player_1", pairWithDevice: gamepad);

                var player = PlayerInput.Instantiate(playerPrefab, spawnpoints[0]);
                var pi = player.GetComponent<PlayerInput>();
                pi.SwitchCurrentControlScheme("Player_1" + Gamepad.all.IndexOfReference(gamepad) + 1);

                //player.SwitchCurrentControlScheme("dddd", gamepad);

                joinedGamepads.Add(gamepad);

                if (spawnpoints.Length > 0)
                {
                    player.transform.position = spawnpoints[0].position;
                }
            }
        }
    }
}
