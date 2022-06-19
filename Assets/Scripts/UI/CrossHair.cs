using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CrossHair : MonoBehaviour
    {
        [SerializeField] private Image crossHair;
        [SerializeField] private Color enemyColor;
        [SerializeField] private Color normalColor;
        private Transform _aimTransform;
        [SerializeField] private float bestAccuracy = .5f;
        [SerializeField] private float worstAccuracy = 2f;
        [SerializeField] private float accuracyImproveSpeed = 10f;
        [SerializeField] private float rotationAccuracyMultiplier = .5f;
        private float _currAccuracy = 1f;
        public void SetAimTransform(Transform aim)
        {
            _aimTransform = aim;
        }

        private void Update()
        {
            if (Camera.main != null && _aimTransform != null)
            {
                var enemyDetected = Physics.OverlapSphere(_aimTransform.position, 0.01f, LayerMask.GetMask("Player Hitboxes")).Length > 0;
                crossHair.color = enemyDetected ? enemyColor : normalColor;
                crossHair.transform.localScale = Vector3.Lerp(crossHair.transform.localScale,
                    Vector3.one * _currAccuracy, accuracyImproveSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(_aimTransform.position), 10 * Time.deltaTime);
            }
        }

        public void SetAccuracy(float playerSpeed, float playerRotationSpeed)
        {
            if (playerSpeed > 2f)
            {
                _currAccuracy = worstAccuracy;
                return;
            }

            _currAccuracy = playerRotationSpeed * rotationAccuracyMultiplier + bestAccuracy;
            _currAccuracy = Mathf.Clamp(_currAccuracy, bestAccuracy, worstAccuracy);
        }
    }
}