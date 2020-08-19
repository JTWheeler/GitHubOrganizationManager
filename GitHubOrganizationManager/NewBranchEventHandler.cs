using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Octokit;

namespace JTWheeler.GitHubOrganizationManager
{
    public class NewBranchEventHandler
    {
        private readonly IGitHubManager GitHubManager;

        public NewBranchEventHandler(IGitHubManager gitHubManager)
        {
            GitHubManager = gitHubManager;
        }

        [FunctionName("RepositoryEventHandler")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"Request body {requestBody}");

            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string refType = data?.ref_type;
            string reference = data.@ref;

            if (refType == "branch" && reference == "master")
            {
                string owner = data.repository.owner.login;
                string repositoryName = data.repository.name;

                await GitHubManager.ConfigureBranchProtections(owner, repositoryName, "master");
            }

            string message = $"message received {requestBody}";

            return new OkObjectResult(message);
        }
    }
}
