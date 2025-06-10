using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    

    public void OnPressPlay()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
