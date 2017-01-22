using System.Web.Mvc;
using ElvenCurse.Core.Interfaces;
using ElvenCurse.Core.Model.Items;

namespace ElvenCurse.Website.Areas.Admin.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemsService _itemsService;

        public ItemsController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        // GET: Admin/Items
        public ActionResult Index()
        {
            var items = _itemsService.GetItems();

            return View(items);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var model = new EditItemViewmodel();
            if (id > 0)
            {
                model.Item = _itemsService.GetItem(id);
            }
            else
            {
                model.Item = new Item();
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditItemViewmodel model)
        {
            if (ModelState.IsValid)
            {
                var id = _itemsService.SaveItem(model.Item);
                if (id > 0)
                {
                    return RedirectToAction("Edit", new {id = id});
                }
            }

            return RedirectToAction("Edit");
        }
    }

    public class EditItemViewmodel
    {
        public Item Item { get; set; }
    }
}