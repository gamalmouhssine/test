using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    public class articl
    {
        public string link { get; set; }
        public string image { get; set; }
        public string titel { get; set; }
        public string text { get; set; }
        public string Date { get; set; }
    }
    [ApiController]
    public class scraping : ControllerBase
    {
        [Route("GetResults")]
        [AcceptVerbs("GET")]
        public async Task<List<articl>> GetResults()
        {            //HtmlWeb web = new HtmlWeb();

            //var doc = web.Load("https://ar.le360.ma/politique");

            //var text = doc.DocumentNode.SelectNodes("//div[@class='post label-']//div[@class='holder']//p").ToArray();
            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync($"https://ar.le360.ma/politique");
            Stream stream = await result.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);
            var text2 = doc.DocumentNode.SelectNodes("//div[@class='post label-']");
            var articls = new List<articl>();
            foreach (var item in text2)
            {
                var articl = new articl
                {
                    link = "https://ar.le360.ma/" + item.Descendants("h3").FirstOrDefault().FirstChild.ChildAttributes("href").FirstOrDefault().Value,
                    image = item.Descendants("img").FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value,
                    titel = item.Descendants("h3").FirstOrDefault().InnerText,
                    text = item.Descendants("p").FirstOrDefault().InnerText,
                    Date = item.Descendants("span").FirstOrDefault().FirstChild.InnerText
                };
                articls.Add(articl);


            }
            return articls;
        }
    }
}