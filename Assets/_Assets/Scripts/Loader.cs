using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    public enum Scene
    {
        MainMenu,
        GameScene,
        LoadingScene
    }
    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;
        
        //Load method gets a reference what scene to load. That we give as parameter and keeps it in mind. Then Loads the loadingScene.
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        
        
    }

    public static void LoaderCallback()
    {
        //LoaderCallback script that is in the loading scene. Calls this function after first update. And this one loads the target scene.
        SceneManager.LoadScene(targetScene.ToString());
    }
}
