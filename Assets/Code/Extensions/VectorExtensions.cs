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

        public static Vector2 ToWholeVector(this Vector2 input)
        {
            return new Vector2((int)input.x, (int)input.y);
        }
    }
}
