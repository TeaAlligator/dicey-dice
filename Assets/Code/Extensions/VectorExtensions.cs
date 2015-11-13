using UnityEngine;

namespace Assets.Code.Extensions
{
    static class VectorConvertor
    {
        public static Vector3 Cardinalized(this Vector3 input)
        {
            if(Mathf.Abs(input.x) > Mathf.Abs(input.z))
                return new Vector3(input.x, 0, 0);

            return new Vector3(0, 0, input.z);
        }

        public static Vector2 Cardinalized(this Vector2 input)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                return new Vector2(input.x, 0);

            return new Vector2(0, input.y);
        }

        public static Vector3 ToWholeVector(this Vector3 input)
        {
            return new Vector3((int)input.x, (int)input.y, (int)input.z);
        }

        /// <summary>
        /// makes vector a whole vector, multiplies labelled dimension for easy y-culling.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="x">0 for no x, 1 for x</param>
        /// <param name="y">0 for no y, 1 for y</param>
        /// <param name="z">0 for no z, 1 for z</param>
        /// <returns></returns>
        public static Vector3 ToBoardVector(this Vector3 input, int x, int y, int z)
        {
            return new Vector3((int)input.x * x, (int)input.y * y, (int)input.z * z);
        }

        public static Vector2 ToWholeVector(this Vector2 input)
        {
            return new Vector2((int)input.x, (int)input.y);
        }
    }
}
