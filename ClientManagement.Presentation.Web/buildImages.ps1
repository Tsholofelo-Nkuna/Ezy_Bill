$originalPath = $pwd.path
$drop = "./bin/Debug/drop"
$buildNumber = Get-Date -Format "yyMMddHHmmss"
$containerRegistry = "tsholofelo768/";
$image = "$($containerRegistry)clientmanagement-presentation-web:$($buildNumber)"
$config = "Debug"
dotnet publish --output $drop --configuration $config -p:DebugType=portable -p:PublishTrimmed=false
copy-item -path "./Dockerfile" -destination "$($drop)/Dockerfile"
docker build -t $image $drop
docker push $image
$image2 = "$($containerRegistry)clientmanagement-mcp:$($buildNumber)"
$mcpProjectPath = "../ClientManagement.Mcp";
cd $mcpProjectPath
$drop2 = "./bin/Debug/drop"
dotnet publish --output $drop2 --configuration $config -p:DebugType=portable -p:PublishTrimmed=false
copy-item -path "./Dockerfile" -destination "$($drop2)/Dockerfile"
docker build -t $image2 $drop2
docker push $image2
cd "$($originalPath)"

write-output $buildNumber
