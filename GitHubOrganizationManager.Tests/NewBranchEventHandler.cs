using Xunit;
using Moq;
using JTWheeler.GitHubOrganizationManager;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GitHubOrganizationManager.Tests
{
    public class GitHubManagerTests
    {
        [Fact]
        public async void WhenMasterBranchIsCreated_ThenBranchProtectionsAreUpdated()
        {
            var gitHubManager = new Mock<IGitHubManager>();
            var mockRequest = new Mock<HttpRequest>();
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            string webhookBody = "{\r\n    \"ref\": \"master\",\r\n    \"ref_type\": \"branch\",\r\n    \"repository\": {\r\n        \"name\": \"someCoolRepository\",\r\n        \"owner\": {\r\n            \"login\": \"theRepositoryOwner\"\r\n        }\r\n    }\r\n}";

            sw.Write(webhookBody);
            sw.Flush();

            ms.Position = 0;
            mockRequest.Setup(x => x.Body).Returns(ms);

            var newBranchEventHandler = new NewBranchEventHandler(gitHubManager.Object);

            await newBranchEventHandler.Run(mockRequest.Object, new Mock<ILogger>().Object);

            gitHubManager.Verify(a => a.ConfigureBranchProtections("theRepositoryOwner", "someCoolRepository", "master"));
        }

        [Fact]
        public async void WhenABranchThatIsNotMasterIsCreated_ThenNoBranchProtectionUpdatesAreMade()
        {
            var gitHubManager = new Mock<IGitHubManager>();
            var mockRequest = new Mock<HttpRequest>();
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            string webhookBody = "{\r\n    \"ref\": \"someNewCoolBranch\",\r\n    \"ref_type\": \"branch\",\r\n    \"repository\": {\r\n        \"name\": \"someCoolRepository\",\r\n        \"owner\": {\r\n            \"login\": \"theRepositoryOwner\"\r\n        }\r\n    }\r\n}";

            sw.Write(webhookBody);
            sw.Flush();

            ms.Position = 0;
            mockRequest.Setup(x => x.Body).Returns(ms);

            var newBranchEventHandler = new NewBranchEventHandler(gitHubManager.Object);

            await newBranchEventHandler.Run(mockRequest.Object, new Mock<ILogger>().Object);

            gitHubManager.Verify(a => a.ConfigureBranchProtections("theRepositoryOwner", "someCoolRepository", "master"), Times.Never);
        }
    }
}
