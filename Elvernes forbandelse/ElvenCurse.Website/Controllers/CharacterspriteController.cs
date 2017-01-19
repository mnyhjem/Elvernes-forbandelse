using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElvenCurse.Website.Controllers
{
    public class CharacterspriteController : Controller
    {
        // GET: Charactersprite
        public ActionResult Index()
        {
            var c = new CharacterAppearance
            {
                Sex = Sex.Female,
                Body = Body.Light,
                Ears = Ears.Elvenears,
                Eyecolor = Eyecolor.Blue,
                Nose = Nose.Default,
            };
            //c.Facial.Type = Facial.FacialType.Beard;
            //c.Facial.Color = Facial.FacialColor.Brown;
            c.Hair.Type = Hair.HairType.Ponytail;
            c.Hair.Color = HairColor.Brunette;

            var character = GetBody(c);

            var image2 = GetImage("torso/dress_female/tightdress_black");

            var result = Merge(character, image2);

            return File(result, "image/png");
            //return View();
        }

        private byte[] GetBody(CharacterAppearance ca)
        {
            var image = GetImage($"body/{ca.Sex}/{ca.Body}");
            image = Merge(image, GetImage($"body/{ca.Sex}/eyes/{ca.Eyecolor}"));

            if (ca.Nose != Nose.Default)
            {
                image = Merge(image, GetImage($"body/{ca.Sex}/nose/{ca.Nose}_{ca.Body}"));
            }

            if (ca.Ears != Ears.Default)
            {
                image = Merge(image, GetImage($"body/{ca.Sex}/Ears/{ca.Ears}_{ca.Body}"));
            }

            if (ca.Hair.Type != Hair.HairType.None)
            {
                image = Merge(image, GetImage($"hair/{ca.Sex}/{ca.Hair.Type}/{ca.Hair.Color.ToString().Replace("_", "-")}"));
            }

            if (ca.Facial.Type != Facial.FacialType.None)
            {
                image = Merge(image, GetImage($"facial/{ca.Sex}/{ca.Facial.Type}/{ca.Facial.Color.ToString().Replace("_", "-")}"));
            }

            return image;
        }

        private byte[] Merge(byte[] bytearray1, byte[] bytearray2)
        {
            using (var ms1 = new MemoryStream(bytearray1))
            using (var ms2 = new MemoryStream(bytearray2))
            {
                using (var image1 = Image.FromStream(ms1))
                using (var image2 = Image.FromStream(ms2))
                {
                    using (var target = new Bitmap(image1.Width, image1.Height, PixelFormat.Format32bppArgb))
                    {
                        using (var graphics = Graphics.FromImage(target))
                        {
                            graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

                            graphics.DrawImage(image1, 0, 0);
                            graphics.DrawImage(image2, 0, 0);

                            return ImageToByteArray(target);
                        }
                    }
                }
            }
        }

        private byte[] GetImage(string path)
        {
            var rootPath = Server.MapPath("~/Content/Assets/Graphics/Universal-LPC-spritesheet-master/");
            using (var img = Image.FromFile(rootPath + path + ".png"))
            {
                return ImageToByteArray(img);
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private static byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }

    public class CharacterAppearance
    {
        public Sex Sex { get; set; }
        public Body Body { get; set; }
        public Eyecolor Eyecolor { get; set; }
        public Nose Nose { get; set; }
        public Ears Ears { get; set; }

        public Facial Facial { get; set; }
        public Hair Hair { get; set; }

        public CharacterAppearance()
        {
            Facial = new Facial();
            Hair = new Hair();
        }
    }

    public class Hair
    {
        public HairType Type { get; set; }
        public HairColor Color { get; set; }

        public enum HairType
        {
            None = 0,
            Bangs = 1,
            Bangslong = 2,
            Bangslong2 = 3,
            Bangsshort = 4,
            Bedhead = 5,
            Bunches = 6,
            Jewfro = 7,
            Long = 8,
            Longhawk = 9,
            Longknot = 10,
            Loose = 11,
            Messy1 = 12,
            Messy2 = 13,
            Mohawk = 14,
            Page = 15,
            Page2 = 16,
            Parted = 17,
            Pixie = 18,
            Plain = 19,
            Ponytail = 20,
            Ponytail2 = 21,
            Princess = 22,
            ShortHawk = 23,
            ShortKnot = 24,
            Shoulderl = 25,
            Shoulderr = 26,
            Swoop = 27,
            Unkempt = 28,
            Xlong = 29,
            Xlongknot = 30
        }
    }

    public class Facial
    {
        public FacialType Type { get; set; }
        public HairColor Color { get; set; }

        public enum FacialType
        {
            None = 0,
            Beard = 1,
            Bigstache = 2,
            Fiveoclock = 3,
            Frenchstache = 4,
            Mustache = 5
        }
    }

    public enum HairColor
    {
        Black,
        Blonde,
        Blonde2,
        Blue,
        Blue2,
        Brown,
        Brown2,
        Brunette,
        Brunette2,
        Dark_blonde,
        Gold,
        Gray,
        Gray2,
        Green,
        Green2,
        Light_blonde,
        Light_blonde2,
        Pink,
        Pink2,
        Purple,
        Raven,
        Raven2,
        Redhead,
        Redhead2,
        Ruby_red,
        White,
        White_blonde,
        White_blonde2,
        White_Cyan
    }

    public enum Ears
    {
        Default = 0,
        Bigears = 1,
        Elvenears = 2
    }

    public enum Nose
    {
        Default = 0,
        Bignose = 1,
        Buttonnose = 2,
        Straightnose = 3
    }

    public enum Eyecolor
    {
        Blue = 0,
        Brown = 1,
        Gray = 2,
        Green = 3,
        Orange = 4,
        Purple = 5,
        Red = 6,
        Yellow = 7
    }

    public enum Sex
    {
        Male = 0,
        Female = 1
    }

    public enum Body
    {
        Dark = 0,
        Dark2 = 1,
        Darkelf = 2,
        Darkelf2 = 3,
        Light = 4,
        Orc = 5,
        Red_orc = 6,
        Tanned = 7,
        tanned2 = 8
    }
}