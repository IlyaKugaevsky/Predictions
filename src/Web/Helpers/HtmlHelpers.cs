﻿using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Web.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString PartialFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string partialViewName)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            object model = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model;
            var viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = new System.Web.Mvc.TemplateInfo { HtmlFieldPrefix = name }
            };
            return helper.Partial(partialViewName, model, viewData);
        }
    }
}