using UnityEngine;

namespace UnityTemplateProjects
{
    public class SimpleCameraController : MonoBehaviour
    {
        class CameraState
        {
            public float yaw;
            public float pitch;
            public float roll;
            public float x;
            public float y;
            public float z;

            public void SetFromTransform(Transform t)
            {
                pitch = t.eulerAngles.x;
                yaw = t.eulerAngles.y;
                roll = t.eulerAngles.z;
                x = t.position.x;
                y = t.position.y;
                z = t.position.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

                x += rotatedTranslation.x;
                y += rotatedTranslation.y;
                z += rotatedTranslation.z;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

                x = Mathf.Lerp(x, target.x, positionLerpPct);
                y = Mathf.Lerp(y, target.y, positionLerpPct);
                z = Mathf.Lerp(z, target.z, positionLerpPct);
            }

            public void LerpTowardsWithMidpoint(CameraState target, CameraState midPoint, float positionLerpPct, float rotationLerpPct, float traveled)
            {
                yaw = Mathf.Lerp(yaw, Mathf.Lerp(midPoint.yaw, target.yaw, traveled), rotationLerpPct);
                pitch = Mathf.Lerp(pitch, Mathf.Lerp(midPoint.pitch, target.pitch, traveled), rotationLerpPct);
                roll = Mathf.Lerp(roll, Mathf.Lerp(midPoint.roll, target.roll, traveled), rotationLerpPct);

                x = Mathf.Lerp(x, Mathf.Lerp(midPoint.x, target.x, traveled), positionLerpPct);
                y = Mathf.Lerp(y, Mathf.Lerp(midPoint.y, target.y, traveled), positionLerpPct);
                z = Mathf.Lerp(z, Mathf.Lerp(midPoint.z, target.z, traveled), positionLerpPct);
               // Debug.Log(" " + traveled);
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(pitch, yaw, roll);
                t.position = new Vector3(x, y, z);
            }
        }

        CameraState m_TargetCameraState = new CameraState();
        CameraState m_MidPointState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float positionLerpTime = 0.2f;

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Pos1")]
        public Transform pos1;
        [Tooltip("Mid1")]
        public Transform mid1;
        [Tooltip("Pos2")]
        public Transform pos2;
        [Tooltip("Mid2")]
        public Transform mid2;

        float traveled = 0f;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
            m_MidPointState.SetFromTransform(transform);
        }

        public void MoveToPoint(Transform target, Transform passThrough)
        {
            m_TargetCameraState.SetFromTransform(target);
            m_MidPointState.SetFromTransform(passThrough);
            traveled = 0.0f;
        }

        void Update()
        {
            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            traveled = traveled < 1.0f ? traveled + positionLerpPct : 1.0f;
            // m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);
            m_InterpolatingCameraState.LerpTowardsWithMidpoint(m_TargetCameraState, m_MidPointState, positionLerpPct, rotationLerpPct, traveled);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }
    }

}