using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoPartsStore.Services.Features
{
    public class Pagination
    {
        public Pagination(IUrlHelper urlHelper, int pageCount, int selectedIndex, int row, string action, string controller, string nextText = null, string previousText = null, string ulClasses = "")
        {
            UrlHelper = urlHelper;
            PageCount = pageCount;
            SelectedIndex = selectedIndex;
            Row = row;
            Action = action;
            Controller = controller;
            IsRazorPage = false;
            UlClasses = ulClasses;
            NextText = nextText;
            PreviousText = previousText;
        }
        public Pagination(int pageCount, int selectedIndex, int row, string urlPath, string nextText = null, string previousText = null, string ulClasses = "")
        {
            PageCount = pageCount;
            SelectedIndex = selectedIndex;
            Row = row;
            UrlPath = urlPath;
            IsRazorPage = false;
            UlClasses = ulClasses;
            NextText = nextText;
            PreviousText = previousText;
        }

        public Pagination(IUrlHelper urlHelper, int pageCount, int selectedIndex, int row, string page, string nextText = null, string previousText = null, string ulClasses = "")
        {
            UrlHelper = urlHelper;
            PageCount = pageCount;
            SelectedIndex = selectedIndex;
            Row = row;
            Page = page; ;
            IsRazorPage = true;
            UlClasses = ulClasses;
            NextText = nextText;
            PreviousText = previousText;
        }
        public IUrlHelper UrlHelper { get; set; }
        public string UrlPath { get; set; }
        public int Row { get; set; }
        public int PageCount { get; set; }
        public int SelectedIndex { get; set; }
        public string Action { get; set; }
        public bool IsRazorPage { get; set; }
        public string UlClasses { get; set; }
        public string Page { get; set; }
        public string Controller { get; set; }
        public string NextText { get; set; }
        public string PreviousText { get; set; }
        public static IEnumerable<TEntity> GetData<TEntity>(IEnumerable<TEntity> entities, ref int count, int row = 5, int index = 1) where TEntity : class
        {
            int skip = (index - 1) * row;
            count = entities.Count();
            return entities.Skip(skip).Take(row);
        }
        public string GetUrl(int index)
        {
            if (!string.IsNullOrEmpty(UrlPath))
            {
                if (UrlPath.IndexOf('?') > 0)
                    return $"{UrlPath}&index={index}&row={Row}";
                return $"{UrlPath}?index={index}&row={Row}";
            }
            object routeValues = new
            {
                row = Row,
                index = index
            };
            if (IsRazorPage)
                return UrlHelper.Page(Page, routeValues);
            return UrlHelper.Action(Action, Controller, routeValues);
        }
    }
}
