using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System.Linq;

namespace HR_Project.Common.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent EnumTh<TModel, TEnum>(
    this IHtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TEnum>> expression)
        {
            var modelExplorer = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);

            var enumType = modelExplorer.ModelType;

            var thTag = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder("th");
            thTag.AddCssClass("sorting");
            thTag.Attributes["tabindex"] = "0";
            thTag.Attributes["aria-controls"] = "dataTable";
            thTag.Attributes["rowspan"] = "1";
            thTag.Attributes["colspan"] = "1";
            thTag.Attributes["style"] = "width: 140px;";
            thTag.Attributes["aria-label"] = $"{enumType.Name}.{GetDisplayName(modelExplorer.Model)}";

            thTag.InnerHtml.SetHtmlContent(modelExplorer.Metadata.DisplayName ?? modelExplorer.Metadata.PropertyName);

            return thTag;
        }


        public static IHtmlContent LeaveTypesTh<TModel>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, LeaveTypes>> expression)
        {
            return htmlHelper.EnumTh(expression);
        }

        private static string GetDisplayName(object value)
        {
            if (value == null)
                return null;

            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var displayAttribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>().FirstOrDefault();

            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
