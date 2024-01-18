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
                if (_animatior.GetCurrentAnimatorStateInfo(0).IsName("Armature|Attack"))
                {
                    _spellController.LeftHandAttack = true;
                }
                else _spellController.LeftHandAttack = false;
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
                if (_animatior.GetCurrentAnimatorStateInfo(0).IsName("Armature|Attack"))
                {
                    _spellController.RightHandAttack = true;
                }
                else _spellController.RightHandAttack = false;
                break;

            default: break;
        }
    }
}
