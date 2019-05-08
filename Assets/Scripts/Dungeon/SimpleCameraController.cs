using UnityEngine;

namespace UnityTemplateProjects
{
    public class SimpleCameraController : MonoBehaviour
    {
        class CameraState
        {
            //public float yaw;
            //public float pitch;
            //public float roll;

            public Quaternion rotation;
            public Vector3 position;

            public void SetFromTransform(Transform t)
            {
                //pitch = t.eulerAngles.x;
                //yaw = t.eulerAngles.y;
                //roll = t.eulerAngles.z;
                rotation = t.rotation;
                position = t.position;
            }

            //public void Translate(Vector3 translation)
            //{
            //    Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

            //    x += rotatedTranslation.x;
            //    y += rotatedTranslation.y;
            //    z += rotatedTranslation.z;
            //}

            //public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            //{
            //    yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            //    pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            //    roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            //    x = Mathf.Lerp(x, target.x, positionLerpPct);
            //    y = Mathf.Lerp(y, target.y, positionLerpPct);
            //    z = Mathf.Lerp(z, target.z, positionLerpPct);
            //}

            public void LerpTowardsWithMidpoint(CameraState target, CameraState midPoint, float positionLerpPct, float rotationLerpPct, float traveled)
            {
                //yaw = Mathf.Lerp(yaw, Mathf.Lerp(midPoint.yaw, target.yaw, traveled), rotationLerpPct);
                //pitch = Mathf.Lerp(pitch, Mathf.Lerp(midPoint.pitch, target.pitch, traveled), rotationLerpPct);
                //roll = Mathf.Lerp(roll, Mathf.Lerp(midPoint.roll, target.roll, traveled), rotationLerpPct);

                //yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                //pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                //roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

                rotation = Quaternion.Slerp(rotation, target.rotation, rotationLerpPct);

                position = Vector3.Lerp(position, Vector3.Lerp(midPoint.position, target.position, traveled), positionLerpPct);               
                // Debug.Log(" " + traveled);
            }

            public void UpdateTransform(Transform t)
            {
                // t.eulerAngles = new Vector3(pitch, yaw, roll);
                t.rotation = rotation;
                t.position = position;
            }
        }

        CameraState m_TargetCameraState = new CameraState();
        CameraState m_MidPointState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 2f)]
        public float positionLerpTime = 0.2f;

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 2f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Pos1")]
        public Transform pos1;
        [Tooltip("Mid1")]
        public Transform mid1;
        [Tooltip("Pos2")]
        public Transform pos2;
        [Tooltip("Mid2")]
        public Transform mid2;

        public float traveled = 1.0f;
        public float positionLerpPct;
        bool moveComplete = true;
        public DungeonMovement player;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
            m_MidPointState.SetFromTransform(transform);
            moveComplete = true;

        }

        public void MoveToPoint(Transform target, Transform passThrough)
        {
            m_InterpolatingCameraState.SetFromTransform(transform);
            m_TargetCameraState.SetFromTransform(target);
            m_MidPointState.SetFromTransform(passThrough);
            traveled = 0.0f;
            moveComplete = false;
        }

        void Update()
        {
            if (!moveComplete && Vector3.Distance(transform.position, m_TargetCameraState.position) > 0.1f)
            {
                // Framerate-independent interpolation
                // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
                positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
                var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
                traveled += positionLerpPct;
                // traveled = traveled < 1.0f ? traveled + positionLerpPct : 1.0f;
                // m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);
                m_InterpolatingCameraState.LerpTowardsWithMidpoint(m_TargetCameraState, m_MidPointState, positionLerpPct, rotationLerpPct, traveled);

                m_InterpolatingCameraState.UpdateTransform(transform);
            }
            else if (!moveComplete)
            {
                Debug.Log("Move complete");
                player.MoveComplete();
                moveComplete = true;
            }
        }
    }
}