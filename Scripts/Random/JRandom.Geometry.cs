using UnityEngine;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        const double Tau = 6.2831853071795864769252867665590057683943387987502116419498891846156328125724179972560696506842341359642962;
        const float TauF = (float)Tau;


        /// <summary> Gets a random single-precision value in the range [0, 360]. </summary>
        public float AngleDegrees()
            => Range(0f, 360f);

        /// <summary> Gets a random double-precision value in the range [0, 360]. </summary>
        public double AngleDegreesD()
            => Range(0d, 360d);

        /// <summary> Gets a random single-precision value in the range [0, tau]. </summary>
        public float AngleRadians()
            => Range(0f, TauF);

        /// <summary> Gets a random double-precision value in the range [0, tau]. </summary>
        public double AngleRadiansD()
            => Range(0d, Tau);


        /// <summary> Gets a random point on the edge of a circle ceneterd on (0, 0) with a radius of 1. </summary>
        public Vector2 PointOnUnitCircle()
            => Vector2.up.Rotate(AngleDegrees()); // could be optimized

        /// <summary> Gets a random point within a circle ceneterd on (0, 0) with a radius of 1. </summary>
        public Vector2 PointWithinUnitCircle()
            => PointOnUnitCircle() * Fraction();

        /// <summary> Gets a random point on the edge of a circle ceneterd on (0, 0) with a radius of <paramref name="radius"/>. </summary>
        public Vector2 PointOnCircle(float radius)
            => PointOnUnitCircle() * radius;

        /// <summary> Gets a random point within a circle ceneterd on (0, 0) with a radius of <paramref name="radius"/>. </summary>
        public Vector2 PointWithinCircle(float radius)
            => PointWithinUnitCircle() * radius;
    }
}
