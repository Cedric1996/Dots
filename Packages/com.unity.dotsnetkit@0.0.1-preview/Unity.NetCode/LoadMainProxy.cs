using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainProxy : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("Main");
    }
}