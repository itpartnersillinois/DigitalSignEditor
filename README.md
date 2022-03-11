# IT Partners README.MD file

## Summary: 

Project to manage the digital signs in Github

## Production location: 

Nothing yet

## Development location: 

Currently, none. We do development on local machines.

## How to deploy to production/development: 

Uses CI. 

## How to set up locally: 

Download the code. There is a database, but this uses EF to generate the database if it doesn't exist. Use NuGet to get the latest copy of the executables.

To build the database, run Update-Database in Package Manager Console (see https://docs.microsoft.com/en-us/ef/core/cli/powershell). Note that signs and sign permissions need to be added manually. This will add a sample sign automatically.

This will require an API key from Github. 

## Notes (error logging, external tools, links, etc.): 

N/A