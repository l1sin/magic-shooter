using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator _animatior;
    [SerializeField] private SpellController _spellController;
    [SerializeField] private Hand _hand;
    private enum Hand
    {
        Left,
        Right
    }

    private void Update()
    {
        switch (_hand)
        {
            case Hand.Left:
                if (CharacterInput.LeftMouseButton && !UIContorller.InMenu)
                {
                    _animatior.SetBool("Attack", true);
                }
                else
                {
                    _animatior.SetBool("Attack", false);
                }
                break;

            case Hand.Right:
                if (CharacterInput.RightMouseButton && !UIContorller.InMenu)
                {
                    _animatior.SetBool("Attack", true);
                }
                else
                {
                    _animatior.SetBool("Attack", false);
                }
                break;

            default: break;
        }
    }
}
