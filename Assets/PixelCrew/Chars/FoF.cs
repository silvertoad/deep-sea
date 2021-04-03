using PixelCrew.Town;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class FoF : MonoBehaviour
    {
        [SerializeField] private TeamType _team;

        public bool CanAttack(TeamType team)
        {
            return team != _team;
        }

        public void SetTeam(TeamType team)
        {
            _team = team;
        }
    }
}