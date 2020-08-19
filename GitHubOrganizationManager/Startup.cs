using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection;
using Octokit;

[assembly: FunctionsStartup(typeof(JTWheeler.GitHubOrganizationManager.Startup))]

namespace JTWheeler.GitHubOrganizationManager
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IGitHubManager>((s) =>
            {
                return InitGitHubClient();
            });
        }

        private IGitHubManager InitGitHubClient()
        {
            string GITHUB_USERNAME = Environment.GetEnvironmentVariable("GITHUB_USERNAME", EnvironmentVariableTarget.Process);
            string GITHUB_TOKEN = Environment.GetEnvironmentVariable("GITHUB_TOKEN", EnvironmentVariableTarget.Process);
            var client = new GitHubClient(new ProductHeaderValue("organization-event-handler"));
            var applicationCredentials = new Credentials(GITHUB_USERNAME, GITHUB_TOKEN);

            client.Credentials = applicationCredentials;

            return new GitHubManager(client);
        }
    }
}