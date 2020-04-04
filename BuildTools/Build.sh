#!/bin/bash

./BuildTools/UpdateVersion.sh

dotnet build --configuration Release
dotnet pack CsharpExtras/CsharpExtras.csproj --configuration Release
