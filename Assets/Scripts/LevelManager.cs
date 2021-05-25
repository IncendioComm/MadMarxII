using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{public static LevelManager instance;
    
    public Transform respawnPoint;
    public GameObject playerPrefab;
    public GameObject hearts;
    public Transform CanvasAnchor;  

    public CinemachineVirtualCameraBase cam;
    
    private void Awake() {
        instance = this;
    }

    public void Respawn () {
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        //PlayerDeath.Start();
        GameObject life = Instantiate(hearts, CanvasAnchor.position, Quaternion.identity);
        cam.Follow = player.transform;
    }
}
