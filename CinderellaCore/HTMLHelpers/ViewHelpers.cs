﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Web.HTMLHelpers
{
    public class ViewHelpers
    {
        public static List<SelectListItem> GetYearRange(int start, int end)
        {
            return Enumerable.Range(start, end - start + 1).Reverse().
                Select(i => new SelectListItem { Value = i.ToString(), Text = i.ToString() }).ToList();
        }
    }
}