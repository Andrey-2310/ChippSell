using Chipper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Threading;

namespace Chipper.Controllers.Parsing
{
    interface IParser
    {
        void CollectChips(object request);
        List<Chip> GetChipsInfo(HtmlDocument htmlDoc);
        Thread getThread();
        
    }
}
