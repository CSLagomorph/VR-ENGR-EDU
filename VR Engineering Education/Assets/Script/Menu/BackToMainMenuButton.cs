using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMainMenuButton : MonoBehaviour
{
    public void BackToMainMenuButtonOnClick()
    {
        SceneManager.LoadScene(sceneName:"Menu");
        PlayerInfo.PlayerTransform.position = new Vector3(0.0F, 0.0F, -1.0F);
        addPCPlayerHeightOffset();
    }

    private void addPCPlayerHeightOffset()
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.NO_DETECTED_DEVICE)
        {
            PlayerInfo.PlayerTransform.position = new Vector3(PlayerInfo.PlayerTransform.position.x, PlayerInfo.PlayerTransform.position.y + PC_PlayerController.PLAYER_HEIGHT, PlayerInfo.PlayerTransform.position.z);
        }
    }
}
