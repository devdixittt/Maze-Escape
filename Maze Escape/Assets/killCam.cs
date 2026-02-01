using Unity.Cinemachine;
using UnityEngine;

public class killCam : MonoBehaviour
{
    public CinemachineCamera killcam;
    public Transform enemy;
    public Transform enemyLookTarget;

    public void PlayKillCam()
    {
        //killcam.Follow = enemy;
        killcam.LookAt = enemyLookTarget;
        killcam.Priority = 0; // higher than player cam
    }

    public void StopKillCam()
    {
        killcam.Priority = 0;
    }


    public void OnAttackFinished()
    {
        Time.timeScale = 0f;
        // Show Game Over UI
    }
}
