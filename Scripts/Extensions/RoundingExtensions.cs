using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class RoundingExtensions
    {
        public static float RoundTo(this float value, float nearest)
            => Mathf.Round(value / nearest) * nearest;

        public static float RoundUpTo(this float value, float nearest)
            => Mathf.CeilToInt(value / nearest) * nearest;

        public static Vector3 RoundTo(this Vector3 value, float nearest)
            => new Vector3(value.x.RoundTo(nearest), value.y.RoundTo(nearest), value.z.RoundTo(nearest));

        public static Quaternion RoundTo(this Quaternion value, float nearestDegree)
        {
            Vector3 eulerAngles = quaternionToEuler(value).RoundTo(nearestDegree);
            return eulerToQuaternion(eulerAngles);


            // rounding the components of quaternions doesn't work. I have no idea why. However, rounding the components of Euler angles DOES work.
            // you can't call Unity's methods for converting Euler angles <===> quaternions outside of Unity itself, because Unity is bullshit.
            // these methods are very, very close to Unity's functions. Much much closer than the rounding they'll be used for.

            Vector3 quaternionToEuler(Quaternion q)
            {
                Vector3 euler;

                // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
                float unit = (q.x * q.x) + (q.y * q.y) + (q.z * q.z) + (q.w * q.w);

                // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
                float test = q.x * q.w - q.y * q.z;

                if (test > 0.4995f * unit) // singularity at north pole
                {
                    euler.x = Mathf.PI / 2;
                    euler.y = 2f * Mathf.Atan2(q.y, q.x);
                    euler.z = 0;
                }
                else if (test < -0.4995f * unit) // singularity at south pole
                {
                    euler.x = -Mathf.PI / 2;
                    euler.y = -2f * Mathf.Atan2(q.y, q.x);
                    euler.z = 0;
                }
                else // no singularity - this is the majority of cases
                {
                    euler.x = Mathf.Asin(2f * (q.w * q.x - q.y * q.z));
                    euler.y = Mathf.Atan2(2f * q.w * q.y + 2f * q.z * q.x, 1 - 2f * (q.x * q.x + q.y * q.y)); // I don't even fucking know, man. Fuck you quaternions.
                    euler.z = Mathf.Atan2(2f * q.w * q.z + 2f * q.x * q.y, 1 - 2f * (q.z * q.z + q.x * q.x));
                }

                // all the math so far has been done in radians. Before returning, we convert to degrees...
                euler *= Mathf.Rad2Deg;

                //...and then ensure the degree values are between 0 and 360
                euler.x %= 360;
                euler.y %= 360;
                euler.z %= 360;

                return euler;
            }

            Quaternion eulerToQuaternion(Vector3 euler)
            {
                float xOver2 = euler.x * Mathf.Deg2Rad * 0.5f;
                float yOver2 = euler.y * Mathf.Deg2Rad * 0.5f;
                float zOver2 = euler.z * Mathf.Deg2Rad * 0.5f;

                float sinXOver2 = Mathf.Sin(xOver2);
                float cosXOver2 = Mathf.Cos(xOver2);
                float sinYOver2 = Mathf.Sin(yOver2);
                float cosYOver2 = Mathf.Cos(yOver2);
                float sinZOver2 = Mathf.Sin(zOver2);
                float cosZOver2 = Mathf.Cos(zOver2);

                Quaternion result;
                result.x = cosYOver2 * sinXOver2 * cosZOver2 + sinYOver2 * cosXOver2 * sinZOver2;
                result.y = sinYOver2 * cosXOver2 * cosZOver2 - cosYOver2 * sinXOver2 * sinZOver2;
                result.z = cosYOver2 * cosXOver2 * sinZOver2 - sinYOver2 * sinXOver2 * cosZOver2;
                result.w = cosYOver2 * cosXOver2 * cosZOver2 + sinYOver2 * sinXOver2 * sinZOver2;

                return result;
            }
        }
    }
}