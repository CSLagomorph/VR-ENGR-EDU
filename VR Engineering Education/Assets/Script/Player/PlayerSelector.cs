using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerSelector : MonoBehaviour
{
    public const string OCULUS_PLAYER_PREFAB_PATH = "Prefabs/OVR_Player";
    public const string OPEN_VR_PLAYER_PREFAB_PATH = "Prefabs/OpenVR_Player";
    public const string PC_PLAYER_PREFAB_PATH = "Prefabs/PC_Player";
    public Vector3 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        if(!GameObject.Find("player"))
        {
            GameObject player;
            spawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
            if(PlayerInfo.VRDeviceName == PlayerInfo.OCULUS_DEVICE)
            {
                player = GameObject.Instantiate((GameObject)Resources.Load(OCULUS_PLAYER_PREFAB_PATH), spawnPoint, Quaternion.Euler(0, 180, 0));
                
            }
            else if(PlayerInfo.VRDeviceName == PlayerInfo.OPEN_VR_DEVICE)
            {
                player = GameObject.Instantiate((GameObject)Resources.Load(OPEN_VR_PLAYER_PREFAB_PATH), spawnPoint, Quaternion.Euler(0, 180, 0));
            }
            else
            {
                player = GameObject.Instantiate((GameObject)Resources.Load(PC_PLAYER_PREFAB_PATH), new Vector3(spawnPoint.x, spawnPoint.y + PC_PlayerController.PLAYER_HEIGHT, spawnPoint.z), Quaternion.Euler(0, 180, 0));
            }
            player.name = "player";
            DontDestroyOnLoad(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
