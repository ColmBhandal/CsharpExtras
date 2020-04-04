#!/bin/bash

./UpdateVersion.sh

cd ../CsharpExtras
dotnet build --configuration Release
dotnet pack CsharpExtras.csproj --configuration Release
