using UnityEngine;

namespace RIMA.Balance
{
    public static class DamageColors
    {
        public static readonly Color Physical = FromHex(0xE89020);
        public static readonly Color Ability = FromHex(0x00FFCC);
        public static readonly Color True = FromHex(0xF4F0E6);
        public static readonly Color Crit = FromHex(0xFFD24A);
        public static readonly Color Fire = FromHex(0xFF6A1F);
        public static readonly Color Frost = FromHex(0x7FE0FF);
        public static readonly Color Lightning = FromHex(0xFFE600);
        public static readonly Color Void = FromHex(0x7B3FA8);
        public static readonly Color Light = FromHex(0xFFF0B0);
        public static readonly Color Poison = FromHex(0x7BC043);

        public static Color For(DamageType damageType)
        {
            return damageType switch
            {
                DamageType.Physical => Physical,
                DamageType.Ability => Ability,
                DamageType.True => True,
                _ => True
            };
        }

        public static Color For(ElementTag elementTag)
        {
            return elementTag switch
            {
                ElementTag.Fire => Fire,
                ElementTag.Frost => Frost,
                ElementTag.Lightning => Lightning,
                ElementTag.Void => Void,
                ElementTag.Light => Light,
                ElementTag.Poison => Poison,
                _ => True
            };
        }

        public static Color For(DamagePacket packet)
        {
            return packet.elementTag != ElementTag.None ? For(packet.elementTag) : For(packet.damageType);
        }

        private static Color FromHex(int hex)
        {
            return new Color(
                ((hex >> 16) & 0xFF) / 255f,
                ((hex >> 8) & 0xFF) / 255f,
                (hex & 0xFF) / 255f,
                1f);
        }
    }
}
