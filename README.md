# GitHub Organization Manager
The GitHub Organization Manager service contains utilities for configuring organization resources, and the handling of events from GitHub.

## Branch Protections
The GitHub Organization Manager service handles GitHub Webhooks for when a new branch is created in a repository. If the branch is `master`, then the service will automatically update the branch protections to enforce consistent configuration across the organization's repositories.

This feature requires the [create](https://developer.github.com/webhooks/event-payloads/#create) webhook to be configured at the organization level.

## Local Development

### Testing
Execute unit tests - `dotnet test`

### Run Service
In order to run the service locally, [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools) is required.
Once installed, execute `func start` from the [GitHubOrganizationManager](./GitHubOrganizationManager) directory.

## Deployment and Hosting
The GitHub Organization Manager uses Azure Functions to host the service.

### CI/CD
GitHub Actions are used to test, build, and deploy the service after new commits to `master`. Reference the [Actions Workflow](.github/workflows/dotnet-core.yml) for further details.

## References
- [Octokit](https://github.com/octokit/octokit.net)
- [Azure Functions](https://azure.microsoft.com/en-us/services/functions/)
- [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)
- [Azure Functions Action](https://github.com/marketplace/actions/azure-functions-action)