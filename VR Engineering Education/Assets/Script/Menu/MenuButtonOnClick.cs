using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonOnClick : MonoBehaviour
{
    public void TutorialButtonOnClick()
    {
        SceneManager.LoadScene(sceneName:"ProofOfConcept");
    }

    public void RampButtonOnClick()
    {
        SceneManager.LoadScene(sceneName:"Ramp");
        PlayerInfo.PlayerTransform.position = new Vector3(3.0F, 0.0F, -3.0F);
        addPCPlayerHeightOffset();
    }

    public void TensileButtonOnClick()
    {
        SceneManager.LoadScene(sceneName:"MetalTesting");
        PlayerInfo.PlayerTransform.position = new Vector3(0.0F, 0.0F, -15.0F);
        addPCPlayerHeightOffset();
    }

    public void ProjectileButtonOnClick()
    {
        SceneManager.LoadScene(sceneName:"Projectile");
        PlayerInfo.PlayerTransform.position = new Vector3(0.0F, 0.0F, 0.0F);
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
