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

This will require an Token from Github (pulled from https://github.com/settings/tokens). Remember to give the the token access to contents and pull requests in the repository. 

## Notes (error logging, external tools, links, etc.): 

To add a new area, add a super-admin using the Configuration in Azure, add an enum to /Data/Models/SignEnums.cs, and add the enum to the dropdown in /Pages/AddSign.cshtml
