#pragma checksum "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "84a125c4e93aedf05593508da41d252b4a8346e9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_StationsShadules_Index), @"mvc.1.0.view", @"/Views/StationsShadules/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\_ViewImports.cshtml"
using TrainzInfo;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\_ViewImports.cshtml"
using TrainzInfo.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"84a125c4e93aedf05593508da41d252b4a8346e9", @"/Views/StationsShadules/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1776ca7247dd799357a8245bd7caea0e11e9526c", @"/Views/_ViewImports.cshtml")]
    public class Views_StationsShadules_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<TrainzInfo.Models.StationsShadule>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Trains", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n<p>\r\n");
#nullable restore
#line 10 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
     if (ViewBag.user != null)
    {
        if (ViewBag.user.Role == "Superadmin" || ViewBag.user.Role == "Admin")
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "84a125c4e93aedf05593508da41d252b4a8346e94950", async() => {
                WriteLiteral("Добавить");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 15 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
        }
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</p>\r\n<table class=\"table\">\r\n    <thead>\r\n        <tr>\r\n            <th>\r\n                ");
#nullable restore
#line 23 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
           Write(Html.DisplayName("Номер"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 26 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
           Write(Html.DisplayName("Станция"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 29 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
           Write(Html.DisplayName("Филия"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 32 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
           Write(Html.DisplayName("Время прибытия"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 35 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
           Write(Html.DisplayName("Время отправления"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 41 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "84a125c4e93aedf05593508da41d252b4a8346e98331", async() => {
#nullable restore
#line 45 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
                                                                                                  Write(Html.DisplayFor(modelItem => item.TrainInfo));

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-number", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 45 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
                                                                          WriteLiteral(item.TrainInfo);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["number"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-number", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["number"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 48 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.Station));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    \r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 52 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.UzFilia));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 55 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.TimeOfArrive.TimeOfDay));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td width=\"70px\">\r\n                    ");
#nullable restore
#line 58 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
               Write(Html.DisplayFor(modelItem => item.TimeOfDepet.TimeOfDay));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <!--<td>\r\n                    <a class=\"btn btn-success\" asp-action=\"Edit\" asp-route-id=\"");
#nullable restore
#line 61 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
                                                                          Write(item.id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\">Edit</a> |-->\r\n");
            WriteLiteral(" <!--|\r\n                    <a class=\"btn btn-danger\"  asp-action=\"Delete\" asp-route-id=\"");
#nullable restore
#line 63 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
                                                                            Write(item.id);

#line default
#line hidden
#nullable disable
            WriteLiteral("\">Delete</a>\r\n                </td>-->\r\n            </tr>\r\n");
#nullable restore
#line 66 "D:\C#\TrainzInfo\TrainzInfo\TrainzInfo\Views\StationsShadules\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    </tbody>
</table>
<style>
    body {
        background: url(https://klike.net/uploads/posts/2020-04/1586763576_18.jpg);
    }
</style>
<style type=""text/css"">
    td {
        background-color: aliceblue; /* Цвет фона */
        position: center;
        padding: 5px; /* Поля в ячейках */
        opacity: 1.9; /* Полупрозрачность таблицы */
        filter: alpha(Opacity=50); /* Для IE */
        width: 700px;
        color: cornflowerblue;
        text-decoration: solid;
    }
</style>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<TrainzInfo.Models.StationsShadule>> Html { get; private set; }
    }
}
#pragma warning restore 1591
