using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public GameObject muzzleFlashEffect;
    [Header("Gun Properties")]
    public float rateOfFire;
    public float damage;
    public float recoil;
    public int ammoCount;
}