using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model.Items;

namespace ElvenCurse.Website.Controllers
{
    public class ItemimageController : Controller
    {
        private readonly IItemsService _itemsService;

        public ItemimageController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        public ActionResult Index(int id = 0)
        {
            var item = _itemsService.GetItem(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            var image = GetImage(item);

            if (item.Category == Itemcategory.Wearable)
            {
                // Klip et enkelt billede ude af vores spritesheet
                var row = 2;
                image = Crop(image, 64, 64, row * 64, 0);
            }
            
            return File(image, "image/png");
        }

        private byte[] Crop(byte[] image, int width, int height, int top, int left)
        {
            using (var ms1 = new MemoryStream(image))
            {
                using (var source = Image.FromStream(ms1))
                {
                    using (var target = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                    {
                        using (var graphics = Graphics.FromImage(target))
                        {
                            graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

                            graphics.DrawImage(
                                source, 
                                0,0,
                                new Rectangle(top, left, target.Width, target.Height),
                                GraphicsUnit.Pixel
                                );

                            return ImageToByteArray(target);
                        }
                    }
                }
            }
        }

        private byte[] GetImage(Item item)
        {
            var path = "~/Content/Assets/Graphics/Objects/";
            if (item.Category == Itemcategory.Wearable)
            {
                path = "~/Content/Assets/Graphics/Universal-LPC-spritesheet-master/";
            }

            return GetImage(path, item.Imagepath);
        }

        private byte[] GetImage(string path, string imagepath)
        {
            var rootPath = Server.MapPath(path);
            using (var img = Image.FromFile(rootPath + imagepath + ".png"))
            using (var bmp = new Bitmap(img))
            {
                return ImageToByteArray(bmp);
            }
        }

        private static byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}