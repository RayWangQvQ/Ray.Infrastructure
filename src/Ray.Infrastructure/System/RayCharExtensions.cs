using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System;

public static class RayCharExtensions
{
    private struct CharLengthSegment : IComparable<CharLengthSegment>
    {
        public int Start;
        public int End;
        public int Length;

        public CharLengthSegment(int start, int end, int length)
        {
            Start = start;
            End = end;
            Length = length;
        }

        public int InRange(int c)
        {
            if (c < Start)
            {
                return c - Start;
            }
            else if (c >= End)
            {
                return End - c + 1;
            }
            else
            {
                return 0;
            }
        }

        public int CompareTo(CharLengthSegment other)
        {
            return Start.CompareTo(other.Start);
        }
    }

    private class CharLengthSegments : IReadOnlyList<CharLengthSegment>
    {
        private readonly CharLengthSegment[] _charLengthSegments;

        public CharLengthSegments(CharLengthSegment[] charLengthSegments)
        {
            _charLengthSegments = charLengthSegments;
            Array.Sort(charLengthSegments);
        }

        public CharLengthSegment this[int index] => _charLengthSegments[index];

        public int Count => _charLengthSegments.Length;

        public CharLengthSegment BinarySearch(char c)
        {
            int index = c;
            int low = 0,
                high = Count,
                middle;

            while (low <= high)
            {
                middle = (low + high) / 2;

                var segment = this[middle];

                if (index < segment.Start)
                {
                    high = middle - 1;
                }
                else if (index < segment.End)
                {
                    return segment;
                }
                else
                {
                    low = middle + 1;
                }
            }

            return default;
        }

        public IEnumerator<CharLengthSegment> GetEnumerator()
        {
            foreach (var segment in _charLengthSegments)
            {
                yield return segment;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _charLengthSegments.GetEnumerator();
        }
    }

    private static readonly CharLengthSegments AllCharsLengthSegments = new(
        new CharLengthSegment[]
        {
            new(0, 7, 1),
            new(7, 9, 0),
            new(9, 10, 8),
            new(10, 11, 0),
            new(11, 13, 1),
            new(13, 14, 0),
            new(14, 162, 1),
            new(162, 166, 2),
            new(166, 167, 1),
            new(167, 169, 2),
            new(169, 175, 1),
            new(175, 178, 2),
            new(178, 180, 1),
            new(180, 182, 2),
            new(182, 183, 1),
            new(183, 184, 2),
            new(184, 215, 1),
            new(215, 216, 2),
            new(216, 247, 1),
            new(247, 248, 2),
            new(248, 449, 1),
            new(449, 450, 2),
            new(450, 711, 1),
            new(711, 712, 2),
            new(712, 713, 1),
            new(713, 716, 2),
            new(716, 729, 1),
            new(729, 730, 2),
            new(730, 913, 1),
            new(913, 930, 2),
            new(930, 931, 1),
            new(931, 938, 2),
            new(938, 945, 1),
            new(945, 962, 2),
            new(962, 963, 1),
            new(963, 970, 2),
            new(970, 1025, 1),
            new(1025, 1026, 2),
            new(1026, 1040, 1),
            new(1040, 1104, 2),
            new(1104, 1105, 1),
            new(1105, 1106, 2),
            new(1106, 8208, 1),
            new(8208, 8209, 2),
            new(8209, 8211, 1),
            new(8211, 8215, 2),
            new(8215, 8216, 1),
            new(8216, 8218, 2),
            new(8218, 8220, 1),
            new(8220, 8222, 2),
            new(8222, 8229, 1),
            new(8229, 8231, 2),
            new(8231, 8240, 1),
            new(8240, 8241, 2),
            new(8241, 8242, 1),
            new(8242, 8244, 2),
            new(8244, 8245, 1),
            new(8245, 8246, 2),
            new(8246, 8251, 1),
            new(8251, 8252, 2),
            new(8252, 8254, 1),
            new(8254, 8255, 2),
            new(8255, 8364, 1),
            new(8364, 8365, 2),
            new(8365, 8451, 1),
            new(8451, 8452, 2),
            new(8452, 8453, 1),
            new(8453, 8454, 2),
            new(8454, 8457, 1),
            new(8457, 8458, 2),
            new(8458, 8470, 1),
            new(8470, 8471, 2),
            new(8471, 8481, 1),
            new(8481, 8482, 2),
            new(8482, 8544, 1),
            new(8544, 8556, 2),
            new(8556, 8560, 1),
            new(8560, 8570, 2),
            new(8570, 8592, 1),
            new(8592, 8596, 2),
            new(8596, 8598, 1),
            new(8598, 8602, 2),
            new(8602, 8712, 1),
            new(8712, 8713, 2),
            new(8713, 8719, 1),
            new(8719, 8720, 2),
            new(8720, 8721, 1),
            new(8721, 8722, 2),
            new(8722, 8725, 1),
            new(8725, 8726, 2),
            new(8726, 8728, 1),
            new(8728, 8729, 2),
            new(8729, 8730, 1),
            new(8730, 8731, 2),
            new(8731, 8733, 1),
            new(8733, 8737, 2),
            new(8737, 8739, 1),
            new(8739, 8740, 2),
            new(8740, 8741, 1),
            new(8741, 8742, 2),
            new(8742, 8743, 1),
            new(8743, 8748, 2),
            new(8748, 8750, 1),
            new(8750, 8751, 2),
            new(8751, 8756, 1),
            new(8756, 8760, 2),
            new(8760, 8764, 1),
            new(8764, 8766, 2),
            new(8766, 8776, 1),
            new(8776, 8777, 2),
            new(8777, 8780, 1),
            new(8780, 8781, 2),
            new(8781, 8786, 1),
            new(8786, 8787, 2),
            new(8787, 8800, 1),
            new(8800, 8802, 2),
            new(8802, 8804, 1),
            new(8804, 8808, 2),
            new(8808, 8814, 1),
            new(8814, 8816, 2),
            new(8816, 8853, 1),
            new(8853, 8854, 2),
            new(8854, 8857, 1),
            new(8857, 8858, 2),
            new(8858, 8869, 1),
            new(8869, 8870, 2),
            new(8870, 8895, 1),
            new(8895, 8896, 2),
            new(8896, 8978, 1),
            new(8978, 8979, 2),
            new(8979, 9312, 1),
            new(9312, 9322, 2),
            new(9322, 9332, 1),
            new(9332, 9372, 2),
            new(9372, 9632, 1),
            new(9632, 9634, 2),
            new(9634, 9650, 1),
            new(9650, 9652, 2),
            new(9652, 9660, 1),
            new(9660, 9662, 2),
            new(9662, 9670, 1),
            new(9670, 9672, 2),
            new(9672, 9675, 1),
            new(9675, 9676, 2),
            new(9676, 9678, 1),
            new(9678, 9680, 2),
            new(9680, 9698, 1),
            new(9698, 9702, 2),
            new(9702, 9733, 1),
            new(9733, 9735, 2),
            new(9735, 9737, 1),
            new(9737, 9738, 2),
            new(9738, 9792, 1),
            new(9792, 9793, 2),
            new(9793, 9794, 1),
            new(9794, 9795, 2),
            new(9795, 12288, 1),
            new(12288, 12292, 2),
            new(12292, 12293, 1),
            new(12293, 12312, 2),
            new(12312, 12317, 1),
            new(12317, 12319, 2),
            new(12319, 12321, 1),
            new(12321, 12330, 2),
            new(12330, 12353, 1),
            new(12353, 12436, 2),
            new(12436, 12443, 1),
            new(12443, 12447, 2),
            new(12447, 12449, 1),
            new(12449, 12535, 2),
            new(12535, 12540, 1),
            new(12540, 12543, 2),
            new(12543, 12549, 1),
            new(12549, 12586, 2),
            new(12586, 12690, 1),
            new(12690, 12704, 2),
            new(12704, 12832, 1),
            new(12832, 12868, 2),
            new(12868, 12928, 1),
            new(12928, 12958, 2),
            new(12958, 12959, 1),
            new(12959, 12964, 2),
            new(12964, 12969, 1),
            new(12969, 12977, 2),
            new(12977, 13198, 1),
            new(13198, 13200, 2),
            new(13200, 13212, 1),
            new(13212, 13215, 2),
            new(13215, 13217, 1),
            new(13217, 13218, 2),
            new(13218, 13252, 1),
            new(13252, 13253, 2),
            new(13253, 13262, 1),
            new(13262, 13263, 2),
            new(13263, 13265, 1),
            new(13265, 13267, 2),
            new(13267, 13269, 1),
            new(13269, 13270, 2),
            new(13270, 19968, 1),
            new(19968, 40870, 2),
            new(40870, 55296, 1),
            new(55296, 55297, 0),
            new(55297, 56320, 1),
            new(56320, 56321, 2),
            new(56321, 57344, 1),
            new(57344, 59335, 2),
            new(59335, 59337, 1),
            new(59337, 59493, 2),
            new(59493, 63733, 1),
            new(63733, 63734, 2),
            new(63734, 63744, 1),
            new(63744, 64046, 2),
            new(64046, 65072, 1),
            new(65072, 65074, 2),
            new(65074, 65075, 1),
            new(65075, 65093, 2),
            new(65093, 65097, 1),
            new(65097, 65107, 2),
            new(65107, 65108, 1),
            new(65108, 65112, 2),
            new(65112, 65113, 1),
            new(65113, 65127, 2),
            new(65127, 65128, 1),
            new(65128, 65132, 2),
            new(65132, 65281, 1),
            new(65281, 65375, 2),
            new(65375, 65504, 1),
            new(65504, 65510, 2),
            new(65510, 65536, 1),
        }
    );

    public static bool IsWideChar(this char @this)
    {
        return GetCharLength(@this) > 1;
    }

    public static int GetCharLength(this char @this)
    {
        return AllCharsLengthSegments.BinarySearch(@this).Length;
    }

    public static int GetCharsLength(this string @this)
    {
        return @this.Select(GetCharLength).Sum();
    }
}
