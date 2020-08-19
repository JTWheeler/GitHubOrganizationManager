using System.Threading.Tasks;

namespace JTWheeler.GitHubOrganizationManager
{
    public interface IGitHubManager
    {
        Task ConfigureBranchProtections(string owner, string repositoryName, string branchName);
    }
}