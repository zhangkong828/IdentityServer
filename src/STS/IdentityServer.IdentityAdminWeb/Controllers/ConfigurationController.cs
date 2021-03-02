using IdentityServer.Service.Dtos.Configuration;
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

        [HttpPost]
        public async Task<IActionResult> Client(ClientDto client)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            //Add new client
            if (client.Id == 0)
            {
                client = _clientService.BuildClientViewModel(client);
                var clientId = await _clientService.AddClientAsync(client);

                return Json(new { code = clientId > 0 ? 0 : -1, msg = clientId > 0 ? "成功" : "失败" });
            }

            //Update client
            var result = await _clientService.UpdateClientAsync(client) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpGet]
        public IActionResult Client(int id)
        {
            ViewBag.ClientId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryClient(int id)
        {
            if (id == 0) Json(new { code = -1, msg = "不存在" });

            var client = await _clientService.GetClientAsync(id);
            return Json(new { code = 0, data = client });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientSecret(ClientSecretsDto clientSecret)
        {
            await _clientService.AddClientSecretAsync(clientSecret);

            return RedirectToAction(nameof(ClientSecrets), new { Id = clientSecret.ClientId });
        }
    }
}
