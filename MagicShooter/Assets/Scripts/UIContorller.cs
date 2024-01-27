using UnityEngine;

public class UIContorller : MonoBehaviour
{
    [SerializeField] private GameObject _spellMenu;
    [SerializeField] private float _pauseTimeScale = 0.2f;

    public static bool InMenu;

    private void Update()
    {
        if (CharacterInput.SpellMenu)
        {
            InMenu = true;
            _spellMenu.SetActive(true);
            Time.timeScale = _pauseTimeScale;
            CursorHelper.ShowCursor();
            CharacterInput.MouseInputAllowed = false;
        }
        else
        {
            InMenu = false;
            _spellMenu.SetActive(false);
            Time.timeScale = 1;
            CursorHelper.LockAndHideCursor();
            CharacterInput.MouseInputAllowed = true;
        }
    }
}
