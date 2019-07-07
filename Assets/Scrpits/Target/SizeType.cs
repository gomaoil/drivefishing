using UnityEngine;

namespace Target
{
    public enum SizeType
    {
        Small,
        Medium,
        Large,
    }

    public static partial class EnumExtend
    {
        public static float GetScale(this SizeType type)
        {
            switch (type)
            {
                case SizeType.Small: return 0.6f;
                case SizeType.Medium: return 1f;
                case SizeType.Large: return 1.6f;
            }
            Debug.Assert(false, "Should not reach here");
            return 1f;
        }
    }

}