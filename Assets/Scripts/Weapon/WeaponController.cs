using UnityEngine;

namespace Weapon {
    public enum WeaponType {
        Bubble
    }

    [RequireComponent(typeof(WeaponManager), typeof(WeaponPool), typeof(BubbleJump))]
    public class WeaponController : MonoBehaviour {
        private WeaponManager.IWeapon _iWeapon;
        private WeaponType _weaponType = WeaponType.Bubble;

        private void Start(){
            _weaponType = WeaponType.Bubble;
            HandleWeaponType(_weaponType);
        }

        private void HandleWeaponType(WeaponType weaponChoice){
            Component c = GetComponent<WeaponManager.IWeapon>() as Component;
            if (c != null) Destroy(c);
            switch (weaponChoice) {
                case WeaponType.Bubble:
                    _iWeapon = gameObject.AddComponent<BubbleWeapon>();
                    break;
                default:
                    _iWeapon = gameObject.AddComponent<BubbleWeapon>();
                    break;
            }
        }

        public void Fire(){
            _iWeapon.Shoot();
        }
    }
}