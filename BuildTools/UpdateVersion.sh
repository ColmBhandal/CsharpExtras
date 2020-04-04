#!/bin/bash

csprojPath=CsharpExtras/CsharpExtras.csproj
gitDescribe=`git describe --tags`
versionTag=`git describe --tags --abbrev=0`
cleanVersion=`echo ${versionTag} | sed -e "s;v;;g"`
revList=`git rev-list ${versionTag}..HEAD --count`

finalVersion=${cleanVersion}.${revList}
if [[ ${revList} == 0 ]]; then
    finalVersion=${cleanVersion}
fi

echo "Updating version to: ${finalVersion}"

sed -e "s;<Version>.*</Version>;<Version>${finalVersion}</Version>;g" -e "s;<PackageReleaseNotes>.*</PackageReleaseNotes>;<PackageReleaseNotes>${gitDescribe}</PackageReleaseNotes>;g" -i ${csprojPath} -b
