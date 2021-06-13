using UnityEngine;

public class Files : MonoBehaviour
{
    public CanvasGroup settingsCg;
    private FileManager _fileManager;
    private LevelDataContainer _ldc;
    private MenuBar _menuBar;

    private void Start()
    {
        _fileManager = FindObjectOfType<FileManager>();
        _menuBar = FindObjectOfType<MenuBar>();
        _ldc = FindObjectOfType<LevelDataContainer>();
    }

    public void NewFile()
    {
        _ldc.ResetLevelData();
        _menuBar.HideToolMenus();
    }

    public void LoadLevel()
    {
        _fileManager.LoadLevel();
        _menuBar.HideToolMenus();
    }

    public void SaveLevel()
    {
        _fileManager.SaveLevel();
        _menuBar.HideToolMenus();
    }

    public void SaveLevelAs()
    {
        _fileManager.SaveLevelAs();
        _menuBar.HideToolMenus();
    }

    public void Quit()
    {
        Application.Quit();
        _menuBar.HideToolMenus();
    }
}