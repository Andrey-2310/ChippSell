using Chipper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chipper.Controllers.Parsing
{
    public class ParsingFactory
    {
        private static List<Chip>  allChips;

        public static List<Chip> GetAllChips(string request)
        {
            allChips = new List<Chip>();
            List<IParser> allParsers = new List<IParser>() { new BelChipParser(), new ChipDipParser()};
            foreach (IParser parser in allParsers)
            {
                parser.getThread().Start(request);
            }
            foreach(IParser parser in allParsers)
            {
                parser.getThread().Join();
            }
            return allChips;
        }

        public static List<Chip> GetAllChips() { return allChips; }
    }
}