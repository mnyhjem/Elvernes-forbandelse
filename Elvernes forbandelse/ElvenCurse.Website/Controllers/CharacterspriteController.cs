﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Model.Creatures;
using ElvenCurse.Core.Model.Creatures.Npcs;
using ElvenCurse.Core.Model.Items;

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

        public ActionResult Index(int id = 0, bool isNpc = false, bool dressingroom = false)
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
            
            if (id == 0 && !dressingroom)
            {
                creature = GetTestAppearance();
            }

            if (dressingroom)
            {
                creature = (Character)Session["dressingroomCharacter"];
                creature.Equipment = GetDefaultEquipment(creature);
            }

            if (creature == null)
            {
                return HttpNotFound();
            }


            //if (creature.CharacterAppearance.Sex == Sex.Female)
            //{
            //    sprite = Merge(sprite, GetImage("torso/dress_female/tightdress_black"));
            //}
            //else
            //{
            //    sprite = Merge(sprite, GetImage("torso/leather/chest_male"));
            //}
            
            var sprite = GetBody(creature.CharacterAppearance);
            sprite = GetEquipment(creature.Equipment, sprite);
            

            sprite = Merge(sprite, GetImage("weapons/right hand/either/bow"));
            sprite = Merge(sprite, GetImage("weapons/left hand/either/arrow"));


            return File(sprite, "image/png");
        }

        private CharacterEquipment GetDefaultEquipment(Creature character)
        {
            var e = new CharacterEquipment();
            if (character.CharacterAppearance.Sex == Sex.Female)
            {
                e.Chest = new Item
                {
                    Category = Itemcategory.Wearable,
                    Type = 6,
                    Name = "Trist gammel kjole",
                    Description = "Denne kjole bør udskiftes hurtigst muligt",
                    Imagepath = "torso/dress_female/tightdress_black"
                };
            }
            else
            {
                e.Chest = new Item
                {
                    Category = Itemcategory.Wearable,
                    Type = 6,
                    Name = "Slidt lædervest",
                    Description = "Denne lædervest ville være bedre tjent med at fungere som taske",
                    Imagepath = "torso/leather/chest_male"
                };
            }
            return e;
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

        private byte[] GetEquipment(CharacterEquipment ce, byte[] sprite)
        {
            if (ce == null)
            {
                return sprite;
            }

            if (ce.Head != null)
            {
                sprite = Merge(sprite, GetImage(ce.Head.Imagepath));
            }

            if (ce.Chest != null)
            {
                sprite = Merge(sprite, GetImage(ce.Chest.Imagepath));
            }

            if (ce.Arms != null)
            {
                sprite = Merge(sprite, GetImage(ce.Arms.Imagepath));
            }

            if (ce.Shoulders != null)
            {
                sprite = Merge(sprite, GetImage(ce.Shoulders.Imagepath));
            }

            if (ce.Bracers != null)
            {
                sprite = Merge(sprite, GetImage(ce.Bracers.Imagepath));
            }
            
            if (ce.Hands != null)
            {
                sprite = Merge(sprite, GetImage(ce.Hands.Imagepath));
            }
            
            if (ce.Legs != null)
            {
                sprite = Merge(sprite, GetImage(ce.Legs.Imagepath));
            }

            if (ce.Feet != null)
            {
                sprite = Merge(sprite, GetImage(ce.Feet.Imagepath));
            }

            if (ce.Belt != null)
            {
                sprite = Merge(sprite, GetImage(ce.Belt.Imagepath));
            }

            if (ce.Weapon != null)
            {
                sprite = Merge(sprite, GetImage(ce.Weapon.Imagepath));
                // if bow, sæt også weapons\left hand\either\arrow på, så der er en pil på buen..
                //sprite = Merge(sprite, GetImage(ce.Torso.Imagepath));
            }

            return sprite;
            //var image = GetImage($"body/{ca.Sex}/{ca.Body}");
            //image = Merge(image, GetImage($"body/{ca.Sex}/eyes/{ca.Eyecolor}"));

            //if (ca.Nose != Nose.Default)
            //{
            //    image = Merge(image, GetImage($"body/{ca.Sex}/nose/{ca.Nose}_{ca.Body}"));
            //}

            //if (ca.Ears != Ears.Default)
            //{
            //    image = Merge(image, GetImage($"body/{ca.Sex}/Ears/{ca.Ears}_{ca.Body}"));
            //}

            //if (ca.Hair.Type != Hair.HairType.None)
            //{
            //    image = Merge(image, GetImage($"hair/{ca.Sex}/{ca.Hair.Type}/{ca.Hair.Color.ToString().Replace("_", "-")}"));
            //}

            //if (ca.Facial.Type != Facial.FacialType.None)
            //{
            //    image = Merge(image, GetImage($"facial/{ca.Sex}/{ca.Facial.Type}/{ca.Facial.Color.ToString().Replace("_", "-")}"));
            //}

            //return image;
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