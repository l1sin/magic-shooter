using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContorller : MonoBehaviour
{
    [SerializeField] private SpellController _spellController;

    [SerializeField] private GameObject _spellMenu;
    [SerializeField] private float _pauseTimeScale = 0.2f;

    [SerializeField] private List<Button> LeftSpellButtons;
    [SerializeField] private List<Button> RightSpellButtons;

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
