using IdentityServer.IdentityAdminWeb.Models;
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
        private readonly IIdentityResourceService _identityResourceService;
        public ConfigurationController(IClientService clientService, IIdentityResourceService identityResourceService)
        {
            _clientService = clientService;
            _identityResourceService = identityResourceService;
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
        public async Task<IActionResult> CloneClient(CloneClientRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            var clientId = await _clientService.CloneClientAsync(request.OriginalId, request.ClientId, request.ClientName);
            return Json(new { code = 0, data = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> QueryClient(int id)
        {
            if (id == 0) Json(new { code = -1, msg = "不存在" });

            var client = await _clientService.GetClientAsync(id);
            return Json(new { code = 0, data = client });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientSecret(AddClientSecretRequest request)
        {
            var result = await _clientService.AddClientSecretAsync(request.ClientId, request.ClientSecret) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientSecret(int id)
        {
            var result = await _clientService.DeleteClientSecretAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientProperty(AddClientPropertyRequest request)
        {
            var result = await _clientService.AddClientPropertyAsync(request.ClientId, request.ClientProperty) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientProperty(int id)
        {
            var result = await _clientService.DeleteClientPropertyAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientClaim(AddClientClaimRequest request)
        {
            var result = await _clientService.AddClientClaimAsync(request.ClientId, request.ClientClaim) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientClaim(int id)
        {
            var result = await _clientService.DeleteClientClaimAsync(id) > 0;
            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }



        public IActionResult IdentityResources()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryIdentityResources(int pageIndex, int pageSize, string key)
        {
            var pageData = await _identityResourceService.GetIdentityResourcesAsync(key, pageIndex, pageSize);
            return Json(new { code = 0, data = pageData });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdentityResource(int id)
        {
            if (id == 0) return Json(new { code = -1, msg = "不存在" });

            var client = await _identityResourceService.GetIdentityResourceAsync(id);
            if (client == null) return Json(new { code = -1, msg = "不存在" });

            var result = await _identityResourceService.DeleteIdentityResourceAsync(client) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }

        [HttpPost]
        public async Task<IActionResult> IdentityResource(IdentityResourceDto identityResource)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            //Add new
            if (identityResource.Id == 0)
            {
                var identityResourceId =await _identityResourceService.AddIdentityResourceAsync(identityResource);
                return Json(new { code = identityResourceId > 0 ? 0 : -1, msg = identityResourceId > 0 ? "成功" : "失败" });
            }

            //Update
            var result = await _identityResourceService.UpdateIdentityResourceAsync(identityResource) > 0;

            return Json(new { code = result ? 0 : -1, msg = result ? "成功" : "失败" });
        }
    }
}
