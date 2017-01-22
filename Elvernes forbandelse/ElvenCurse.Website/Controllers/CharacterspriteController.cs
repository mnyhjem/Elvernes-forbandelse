using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Creatures;

namespace ElvenCurse.Website.Controllers
{
    public class CharacterspriteController : Controller
    {
        private readonly IWorldService _worldService;
        private readonly ICharacterService _characterService;

        public CharacterspriteController(IWorldService worldService, ICharacterService characterService)
        {
            _worldService = worldService;
            _characterService = characterService;
        }

        public ActionResult Index(int id = 0, bool isNpc = false)
        {
            Creature creature;
            if (isNpc)
            {
                creature = _worldService.GetNpc(id);
            }
            else
            {
                creature = _characterService.GetCharacterNoUsercheck(id);
            }

            if (id == 0)
            {
                creature = GetTestAppearance();
            }

            if (creature == null)
            {
                return HttpNotFound();
            }

            var sprite = GetBody(creature.CharacterAppearance);
            if (creature.CharacterAppearance.Sex == Sex.Female)
            {
                sprite = Merge(sprite, GetImage("torso/dress_female/tightdress_black"));
            }
            else
            {
                sprite = Merge(sprite, GetImage("torso/leather/chest_male"));
            }
            

            sprite = Merge(sprite, GetImage("weapons/right hand/either/bow"));

            return File(sprite, "image/png");
        }

        // GET: Charactersprite
        private Creature GetTestAppearance()
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

            var creature = new Character(Creaturetype.Hunter);
            creature.CharacterAppearance = c;

            return creature;

            //var character = GetBody(c);

            //var image2 = GetImage("torso/dress_female/tightdress_black");
            //var result = Merge(character, image2);

            //return File(result, "image/png");
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
            using(var bmp = new Bitmap(img))
            {
                return ImageToByteArray(bmp);
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

    

    
}