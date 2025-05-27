using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public void SceneLoader()
    {
        SceneManager.LoadScene("Game");
    }
}