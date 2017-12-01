using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Frank
{

    public class MainMenuCallbacks : MonoBehaviour
    {
        public GameObject fadeoutp;
        public void OnStartButton()
        {
            print("Start Pressed");
            if (fadeoutp != null) fadeoutp.SetActive(true);
            else print("fadeout panel is null");
            Invoke("NextScene", .5f);
        }

        void NextScene()
        {
            int c = SceneManager.GetActiveScene().buildIndex;
            if (c < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene("TestScene");
        }

    }
}