using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chipper.Models;
using HtmlAgilityPack;
using System.Text;
using Chipper.Controllers.Parsing;
using System.Threading;

namespace Chipper.Controllers
{
    public class BelChipParser : IParser
    {
        private Thread thread;
        private string queryString = "http://belchip.by/search/?query=";
        private string siteString = "http://belchip.by/";

        public Thread getThread() { return thread; }

        public BelChipParser()
        {
            thread = new Thread(new ParameterizedThreadStart(CollectChips));      
        }

        public void CollectChips(object request)
        {
            string html = queryString + request;
            HtmlDocument HD = new HtmlDocument();
            var web = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            HD = web.Load(html);
            ParsingFactory.GetAllChips().AddRange(GetChipsInfo(HD));       
        }

        public List<Chip> GetChipsInfo(HtmlDocument htmlDoc)
        {
            var chips = new List<Chip>();
            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='cat-item']");
            if (nodes != null)
                foreach (HtmlNode node in nodes)
                {
                    chips.Add(new Chip()
                    {
                        Name = node.SelectSingleNode(".//h3//a").InnerText,
                        Reference = siteString + node.SelectSingleNode(".//h3//a").Attributes["href"].Value,
                        ImageReference = siteString + node.SelectSingleNode(".//div[@class='cat-pic']//a[2]//img").Attributes["src"].Value,
                        Price = node.SelectSingleNode(".//div[@class='butt-add']/span[1]").InnerText
                    });
                    if (node.SelectSingleNode(".//div[@class='butt-add']/span[1]").InnerText.Equals("цена по запросу"))
                        chips.Last().Availability = "По запросу";
                    else chips.Last().Availability = "Со склада";
                }
            return chips;
        }
    }
}