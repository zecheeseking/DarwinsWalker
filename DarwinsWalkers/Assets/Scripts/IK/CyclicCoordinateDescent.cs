using System.Runtime.Remoting.Channels;
using UnityEngine;

namespace Assets.Scripts.IK
{
    class CyclicCoordinateDescent
    {
        private Vector3 rotAxis = new Vector3(0,0,-1);

        private int MAX_LOOPS = 100;
        private bool Damping = true;
        private float DAMPING_MAX = 0.5f;
        private float ANGLE_THRESHOLD = 0.999f;
        private float IK_POS_THRESHOLD = 0.125f;
        private float STEPSIZE = 3.0f;

        public void Solve(Transform[] bones, Vector3 target)
        {
            if (bones.Length == 0)
                return;

            Transform endEffector = bones[bones.Length - 1];
            Vector3 currEnd = Vector3.zero;
            Vector3 rootPos = Vector3.zero;
            Vector3 crossResult = Vector3.zero;
            Vector3 targetDirection = Vector3.zero;
            Vector3 currentDirection = Vector3.zero;

            float theDot = 0;
            float turnRadians = 0;

            int currentBone = bones.Length - 1;
            int attempts = 1;

            while (attempts < MAX_LOOPS && (currEnd - target).sqrMagnitude > IK_POS_THRESHOLD)
            {
                if (currentBone < 1)
                    currentBone = bones.Length - 1;

                rootPos = bones[currentBone].position;
                currEnd = endEffector.position;

                rootPos.z = 0;
                currEnd.z = 0;

                currentDirection = currEnd - rootPos;
                targetDirection = target - rootPos;

                currentDirection.Normalize();
                targetDirection.Normalize();

                theDot = Vector3.Dot(currentDirection, targetDirection);

                if (Mathf.Abs(theDot) < ANGLE_THRESHOLD)
                {
                    turnRadians = Mathf.Acos(theDot);

                    crossResult = Vector3.Cross(currentDirection, targetDirection);

                    if (crossResult.z > 0.0f)
                    {
                        turnRadians = (Damping && turnRadians > DAMPING_MAX) ? DAMPING_MAX : turnRadians;

                        bones[currentBone].Rotate(rotAxis, -turnRadians * STEPSIZE);
                    }
                    else if (crossResult.z < 0.0f)
                    {
                        turnRadians = (Damping && turnRadians > DAMPING_MAX) ? DAMPING_MAX : turnRadians;

                        bones[currentBone].Rotate(rotAxis, turnRadians * STEPSIZE);
                    }
                }

                attempts++;
                currentBone--;
            }
        }
    }
}
