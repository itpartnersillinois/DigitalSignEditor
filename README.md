# IT Partners README.MD file

## Summary: 

Project to manage the digital signs in Github

[![deploy_main](https://github.com/itpartnersillinois/DigitalSignEditor/actions/workflows/deploy_main.yml/badge.svg?branch=main)](https://github.com/itpartnersillinois/DigitalSignEditor/actions/workflows/deploy_main.yml)

## Production location: 

Nothing yet

## Development location: 

Currently, none. We do development on local machines.

## How to deploy to production/development: 

Uses CI. 

## How to set up locally: 

Download the code. There is a database, but this uses EF to generate the database if it doesn't exist. Use NuGet to get the latest copy of the executables.

To build the database, run Update-Database in Package Manager Console (see https://docs.microsoft.com/en-us/ef/core/cli/powershell). Note that calendars, signs, and sign permissions need to be added manually. This will add sample signs automatically.

This will require an Token from Github (pulled from https://github.com/settings/tokens). Remember to give the the token access to contents and pull requests in the repository. 

This will require an API key from Twitter (pulled from Project and Apps at https://developer.twitter.com/en/portal)

## Notes (error logging, external tools, links, etc.): 

N/A