#!/bin/bash

./UpdateVersion.sh

cd ../
dotnet build --configuration Release
dotnet pack CsharpExtras/CsharpExtras.csproj --configuration Release
