using System.Collections;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class introCam : MonoBehaviour
{
    public CinemachinePositionComposer IntroCam;
    public float startingDist;
    public float endDist;
    public float duration;
    public GameObject menuUI;
    public AudioSource introMusic;
    public GameObject objective;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(moveCamera());
        StartCoroutine(BeginGameplayAfterSound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator moveCamera()
    {
        float t = 0f;
        var bodyCam = IntroCam;

        while (t < duration)
        {
            bodyCam.CameraDistance = Mathf.Lerp(startingDist, endDist, t / duration);
            t += Time.deltaTime;

            yield return null;

        }
        bodyCam.CameraDistance = endDist;
        menuUI.SetActive(true);
    }

    IEnumerator BeginGameplayAfterSound()
    {
        yield return new WaitForSeconds(introMusic.clip.length);

        // Enable player input / enemy AI here
        Time.timeScale = 1f;
    }

    public void play()
    {
        menuUI.SetActive(false);
        StartCoroutine(showObjective());
    }

    IEnumerator showObjective()
    {
        objective.SetActive(true);

        yield return new WaitForSeconds(5f);

        objective.SetActive(false);

        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
