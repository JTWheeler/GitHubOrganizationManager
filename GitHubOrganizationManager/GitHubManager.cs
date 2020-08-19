using System;
using System.Threading.Tasks;
using Octokit;

namespace JTWheeler.GitHubOrganizationManager
{
    public class GitHubManager : IGitHubManager
    {
        private readonly GitHubClient Client;
        public GitHubManager(GitHubClient client)
        {
            Client = client;
        }

        public async Task ConfigureBranchProtections(string owner, string repositoryName, string branchName)
        {
            await UpdateBranchProtection(owner, repositoryName, branchName);
            await CreateIssue(owner, repositoryName);
        }

        private async Task CreateIssue(string owner, string repositoryName)
        {
            var newIssue = new NewIssue($"Master Branch Protection Updated")
            {
                Body = "@jtlwheeler Branch protection updated.\n- Require pull request reviews before merging.\n- Require status checks to pass before merging"
            };

            await Client.Issue.Create(owner, repositoryName, newIssue);
        }

        private async Task UpdateBranchProtection(string owner, string repositoryName, string branchName)
        {
            var update = new BranchProtectionSettingsUpdate(
                                new BranchProtectionRequiredStatusChecksUpdate(false, new[] { "new" }),
                                new BranchProtectionRequiredReviewsUpdate(false, true, 2),
                                false
                            );

            await Client.Repository.Branch.UpdateBranchProtection(owner, repositoryName, branchName, update);
        }
    }
}