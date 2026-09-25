$originalPath = $pwd.path
$drop = "./bin/Debug/drop"
$buildNumber = Get-Date -Format "yyMMddHHmmss"
$image = "tsholofelo768/clientmanagement-presentation-web:dev-$($buildNumber)"

dotnet publish --output $drop --configuration Release 
copy-item -path "./Dockerfile" -destination "$($drop)/Dockerfile"
docker build -t $image $drop
docker push $image
$image2 = "tsholofelo768/clientmanagement-mcp:dev-$($buildNumber)"
$mcpProjectPath = "../ClientManagement.Mcp";
cd $mcpProjectPath
$drop2 = "./bin/Debug/drop"
dotnet publish --output $drop2 --configuration Release 
copy-item -path "./Dockerfile" -destination "$($drop2)/Dockerfile"
docker build -t $image2 $drop2
docker push $image2
cd "$($originalPath)/deployment"

"helm upgrade izzy-bill . -f .\values.yaml -f local-secrets.yaml  --install --namespace development --create-namespace" | write-output
write-output $buildNumber