using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace orgel
{
    class Keyboard
    {
        readonly List<Key> keys = new List<Key>();

        public Keyboard()
        {
            int num = 1;
            for (int i = 0; i < 6; i++)
            {
                if (i < 5)
                {
                    keys.AddRange(Create12KeySection(ref num));
                }
                else
                {
                    keys.AddRange(CreateLastSection(ref num));
                }
            }
        }

        class Key
        {
            public int Num { get; set; }
            public int Graphic { get; set; }
            public bool Flipped { get; set; }

            public Key(int num, int graphic, bool flipped)
            {
                Num = num;
                Graphic = graphic;
                Flipped = flipped;
            }
        }

        private Key CreateLeftKey(int num)
        {
            return new Key(num, Resource.Drawable.keyside, false);
        }

        private Key CreateRightKey(int num)
        {
            return new Key(num, Resource.Drawable.keyside, true);
        }

        private Key CreateMiddleKey(int num)
        {
            return new Key(num, Resource.Drawable.keymid, false);
        }

        private Key CreateUpperKey(int num)
        {
            return new Key(num, Resource.Drawable.keyup, false);
        }

        private List<Key> Create12KeySection(ref int startNum)
        {
            List<Key> section = new List<Key>();

            for (int i = startNum; i < (startNum+12); i++)
            {
                if (i == startNum || i == startNum + 7)
                {
                    section.Add(CreateLeftKey(i));
                }
                else if (i == startNum + 6 || i == startNum + 11)
                {
                    section.Add(CreateRightKey(i));
                }
                else if (i == startNum + 2 || i == startNum + 4 || i == startNum + 9)
                {
                    section.Add(CreateMiddleKey(i));
                }
                else
                {
                    section.Add(CreateUpperKey(i));
                }
            }
            startNum += 12;

            return section;
        }

        private List<Key> CreateLastSection(ref int num)
        {
            List<Key> section = new List<Key>
            {
                CreateLeftKey(num++)
            };
            /*for (int i = 0; i < 2; i++)
            {
               section.Add(CreateUpperKey(num++));
               section.Add(CreateMiddleKey(num++));
            }
            section.Add(CreateUpperKey(num++));
            section.Add(CreateRightKey(num++));*/

            return section;
        }

        public int GetKeyImageByNum(int num)
        {
            return keys.Where(x => x.Num == num).First().Graphic;
        }

        public bool GetKeyFlippedByNum(int num)
        {
            return keys.Where(x => x.Num == num).First().Flipped;
        }

        private string GetNoteChannel(int num)
        {
            return "0000";
        }

        private string GetNoteNumber(int num)
        {
            byte msg = Convert.ToByte(11);
            msg += Convert.ToByte(num);
            string msgString = Convert.ToString(msg, 2);
            if (msgString.Length < 8)
            {
                string addition = "";
                for (int i = 0; i < 8-msgString.Length; i++)
                {
                    addition += "0";
                }
                return (addition + msgString);
            }
            return msgString;
        }

        private string GetVelocity(int num, bool noteon)
        {
            if (noteon)
            {
                return "11111111";
            }
            return "00000000"; //todo actual velocity, this is a placeholder
        }

        public byte[] GetNoteOnMessageByNum(int num)
        {
            string msg = "1001";
            msg += GetNoteChannel(num);
            msg += GetNoteNumber(num);
            msg += GetVelocity(num, true);
            return GetBytesFromBinaryString(msg);
        }

        public byte[] GetNoteOffMessageByNum(int num)
        {
            string msg = "1000";
            msg += GetNoteChannel(num);
            msg += GetNoteNumber(num);
            msg += GetVelocity(num, false);
            return GetBytesFromBinaryString(msg);
        }

        private byte[] GetBytesFromBinaryString(string binary)
        {
            List<byte> bytes = new List<byte>();
            string s;
            for (int i = 0; i < binary.Length; i += 8)
            {
                s = binary.Substring(i, 8);
                bytes.Add(Convert.ToByte(s, 2));
            }
            return bytes.ToArray();
        }
    }
}