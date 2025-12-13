using System;
using HtmlAgilityPack;

namespace TrainzInfoShared.DTO.GetDTO
{
    public class NewsDTO
    {
        public int id { get; set; }
        public string NameNews { get; set; }
        public string BaseNewsInfo { get; set; }
        public string NewsInfoAll { get; set; }
        public string DateTime { get; set; }
        public string NewsImage { get; set; }
        public string ImageMimeTypeOfData { get; set; }
        
        public string NameNewsInfoHtmlPlain
        {
            get
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(NameNews ?? "");
                return doc.DocumentNode.InnerText;
            }
        }
        
        public string BaseNewsInfoHtmlPlain
        {
            get
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(BaseNewsInfo ?? "");
                return doc.DocumentNode.InnerText;
            }
        }
        
        public string AllNewsInfoHtmlPlain
        {
            get
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(NewsInfoAll ?? "");
                return doc.DocumentNode.InnerText;
            }
        }
        
    }
}
