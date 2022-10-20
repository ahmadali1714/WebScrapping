﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using WebReaper.Proxy.Abstract;

namespace WebReaper.Proxy.Concrete
{

    public class WebShareProxyProvider : IProxyProvider
    {
        private readonly string proxyApiKey = "a45n2ninq91ccjrjrrmgr1okqxiyte7iy4uw0kds";
        private readonly string getProxiesUrl = "https://proxy.webshare.io/api/proxy/list/";
        static readonly HttpClient client = new HttpClient();

        public WebProxy GetProxy()
        {
            throw new NotImplementedException();
        }

        private async Task<List<Proxy>> GetProxies()
        {
            var proxies = new List<Proxy>();

            client.DefaultRequestHeaders.Add("Authorization", proxyApiKey);

            for (int page = 0; page < 10; page++)
            {
                var proxiesResp = await client.GetAsync(getProxiesUrl + $"?page={page}");

                JObject json = JObject.Parse(await proxiesResp.Content.ReadAsStringAsync());

                List<Proxy> m = JsonConvert.DeserializeObject<List<Proxy>>(json["results"].ToString());

                proxies.AddRange(m);
            }

            return proxies;
        }

        private async Task<IEnumerable<WebProxy>> GetWebProxies()
        {
            var proxiesRaw = await GetProxies();

            var proxies = proxiesRaw.Select(p => new WebProxy
            {
                Address = new Uri($"http://{p.Proxy_Address}:{p.Ports["http"]}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,

                // *** These creds are given to the proxy server, not the web server ***
                Credentials = new NetworkCredential(
                userName: p.Username,
                password: p.Password)
            });

            return proxies;
        }
    }
}
