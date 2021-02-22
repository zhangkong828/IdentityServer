using IdentityServer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityAdminWeb.Controllers
{
    public class ConfigurationController : BaseController
    {
        private readonly IClientService _clientService;

        public ConfigurationController(IClientService clientService)
        {
            _clientService = clientService;
        }


        public IActionResult Clients()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryClients(int pageIndex, int pageSize, string key)
        {
            var pageData = await _clientService.GetClientsAsync(key, pageIndex, pageSize);
            return Json(new { code = 0, data = pageData });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (id == 0) return Json(new { code = -1, msg = "不存在" });

            var client = await _clientService.GetClientAsync(id);
            if (client == null) return Json(new { code = -1, msg = "不存在" });

            var result = await _clientService.RemoveClientAsync(client) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        public IActionResult Client()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryClient(int id)
        {
            if (id == 0) Json(new { code = -1, msg = "不存在" });

            var client = await _clientService.GetClientAsync(id);
            client = _clientService.BuildClientViewModel(client);

            return Json(new { code = 0, data = client });
        }
    }
}
