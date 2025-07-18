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
        bool attack = _animatior.GetCurrentAnimatorStateInfo(0).IsName("Armature|Attack");
        bool isInTransition = _animatior.IsInTransition(0);
        bool readyToAttack = attack && !isInTransition;

        switch (_hand)
        {
            case Hand.Left:
                if (CharacterInput.LeftMouseButton && !UIContorller.SpellMenu) _animatior.SetBool("Attack", true);
                else _animatior.SetBool("Attack", false);

                if (readyToAttack && CharacterInput.LeftMouseButton) _spellController.LeftHandAttack = true;
                else _spellController.LeftHandAttack = false;
                break;

            case Hand.Right:
                if (CharacterInput.RightMouseButton && !UIContorller.SpellMenu) _animatior.SetBool("Attack", true);
                else _animatior.SetBool("Attack", false);

                if (readyToAttack && CharacterInput.RightMouseButton) _spellController.RightHandAttack = true;
                else _spellController.RightHandAttack = false;
                break;

            default: break;
        }
    }
}
