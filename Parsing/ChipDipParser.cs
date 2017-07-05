using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chipper.Models;
using HtmlAgilityPack;
using System.Text;
using System.Threading;

namespace Chipper.Controllers.Parsing
{
    public class ChipDipParser : IParser
    {
        private string queryString = "https://www.ru-chipdip.by/search?searchtext=";
        private string siteString = "https://www.ru-chipdip.by";
        private string pageDescription = "&page=";
        private int pageNumber = 1;
        private Thread thread;

        public ChipDipParser()
        {
            thread = new Thread(new ParameterizedThreadStart(CollectChips));
        }

        public Thread getThread()  { return thread; }

        public void CollectChips(object request)
        {
            string htmlPage;
            HtmlDocument HD = new HtmlDocument();
            var web = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8,
            };
            List<Chip> chips = new List<Chip>();
            while (true)
            {
                htmlPage = queryString + request + pageDescription + pageNumber++;
                HD = web.Load(htmlPage);
                if (!HtmlDocValidation(HD)) break;
                ParsingFactory.GetAllChips().AddRange(GetChipsInfo(HD));
            }         
        }

        public List<Chip> GetChipsInfo(HtmlDocument htmlDoc)
        {
            var chips = new List<Chip>();
            var nodes = htmlDoc.DocumentNode.SelectNodes("//tr[@class='with-hover']");
            if (nodes != null)
                foreach (HtmlNode node in nodes)
                {
                    Chip chip = new Chip();
                    if (node.SelectSingleNode(".//div[@class='name']//a[@class='link']") != null)
                        chip.Reference = siteString + node.SelectSingleNode(".//div[@class='name']//a[@class='link']").Attributes["href"].Value;
                    if (node.SelectSingleNode(".//div[@class='name']//a[@class='link']") != null)
                        chip.Name =  node.SelectSingleNode(".//div[@class='name']//a[@class='link']").InnerText;
                    if (node.SelectSingleNode(".//div[@class='img-wrapper']/span/img") != null)
                        chip.ImageReference = node.SelectSingleNode(".//div[@class='img-wrapper']/span/img").Attributes["src"].Value;
                    chip.Price = node.SelectSingleNode(".//span[@class='price_mr']").InnerText;
                    chip.Availability = node.SelectSingleNode(".//div[@class='av_w2']").InnerText;
                    chips.Add(chip);
                }

            return chips;
        }
        /*
        is document valid or not
         */
        private bool HtmlDocValidation(HtmlDocument htmlDoc)
        {
            if (htmlDoc.DocumentNode.SelectNodes("//div[@class='not-found-wrapper']") == null) return true;
            return false;
        }

        

       
    }
}