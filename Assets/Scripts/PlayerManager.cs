
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject m_player = null;

    public void OnPlayerJoined(PlayerInput player_input)
    {
        foreach (var device in player_input.devices)
        {
            Debug.Log(device);
        }
    }
}
