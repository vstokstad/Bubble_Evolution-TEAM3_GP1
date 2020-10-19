using UnityEngine;

namespace Weapon {
    public class WeaponManager : MonoBehaviour {
        public interface IWeapon {
            void Shoot();
        }
    }
}