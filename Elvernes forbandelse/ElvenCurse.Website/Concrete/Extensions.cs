using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElvenCurse.Website.Concrete
{
    public static class Extensions
    {
        //public static IEnumerable<SelectListItem> GetEnumSelectList(this Enum col)
        //{
        //    var type = col.GetType();
        //    return (Enum.GetValues(type).Cast<int>().Select(e => new SelectListItem() { Text = Enum.GetName(type, e), Value = e.ToString() })).ToList();
        //}

        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            var type = typeof(T);
            return (Enum.GetValues(type).Cast<int>().Select(e => new SelectListItem() { Text = Enum.GetName(type, e), Value = e.ToString() })).ToList();
        }
    }
}