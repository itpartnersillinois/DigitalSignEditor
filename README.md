# IT Partners README.MD file

## Summary: 

Project to manage the digital signs in Github

[![deploy_main](https://github.com/itpartnersillinois/DigitalSignEditor/actions/workflows/deploy_main.yml/badge.svg?branch=main)](https://github.com/itpartnersillinois/DigitalSignEditor/actions/workflows/deploy_main.yml)

[![test_develop](https://github.com/itpartnersillinois/DigitalSignEditor/actions/workflows/test_develop.yml/badge.svg)](https://github.com/itpartnersillinois/DigitalSignEditor/actions/workflows/test_develop.yml)

## Production location: 

https://digitalsigneditor.itpartners.illinois.edu

## Development location: 

Currently, none. We do development on local machines.

**Note:** Make sure you have either disconnected the Github tokens or do not run the GithubExport command. Otherwise, you will copy your local database to the production Github repository and mess up all the signs.

You can use the DigitalSignEditorUnitTest to test out the GithubExport process independently.

You can also update the Github configuration in the appsettings.json (or better yet, your user secrets file) to point to a temporary repository if you want to do end-to-end test. 

## How to deploy to production/development: 

Uses CI. 

## How to set up locally: 

Download the code. There is a database, but this uses EF to generate the database if it doesn't exist. Use NuGet to get the latest copy of the executables.

To build the database, run Update-Database in Package Manager Console (see https://docs.microsoft.com/en-us/ef/core/cli/powershell). Note that calendars, signs, and sign permissions need to be added manually. This will add sample signs automatically.

### Github Personal Access Token: 
This application requires a fine-grain token from Github (pulled from https://github.com/settings/personal-access-tokens).

If the automated publish to the digital sign is failing, chances are it's because this token has expired. To confirm this, set the DigitalSignEditor Azure App Service Environment value to ASPNETCORE_ENVIRONMENT to *Development*, then access the URL https://digitalsigneditor.itpartners.illinois.edu/api/manager/cleancommit. If the Github Personal Access Token has expired, then you should see a Connection Failed error. Chnage the ASPNETCORE_ENVIRONMENT back to *Production* and follow the steps below to regenerate the Github token. 

1. Go to https://github.com/settings/personal-access-tokens. Generate a new token.
2. Change the resource owner is itpartnersillinois
3. Make the expiration date no more than one year from the current date.
4. Give the token access to the DigitalSignV2 repository with the following permissions:
   * Read access to actions variables, metadata, and secrets
   * Read and Write access to actions, code, commit statuses, deployments, environments, and pull requests
5. Copy the string it gives you, and copy it to the Github__Token Environment value in the Digital Sign Editor Azure App Service and the Github__Token Environment value in the Deploy Slot.
6. Restart the Application. 

## Notes (error logging, external tools, links, etc.): 

To add a new area, add a super-admin using the Configuration in Azure, add an enum to /Data/Models/SignEnums.cs, and add the enum to the dropdown in /Pages/AddSign.cshtml
