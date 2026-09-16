# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
# For more information, please see https://aka.ms/containercompat

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-nanoserver-ltsc2022 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0-nanoserver-ltsc2022 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["Directory.Build.targets", "."]
COPY ["ClientManagement.Presentation.Web/ClientManagement.Presentation.Web.csproj", "ClientManagement.Presentation.Web/"]
COPY ["ClientManagement.Ai/ClientManagement.AI.csproj", "ClientManagement.Ai/"]
COPY ["ClientManagement.DataAccessLayer/ClientManagement.DataAccessLayer.csproj", "ClientManagement.DataAccessLayer/"]
COPY ["ClientManagement.Models/ClientManagement.Models.csproj", "ClientManagement.Models/"]
COPY ["ClientManagement.BusinessLogicLayer/ClientManagement.BusinessLogicLayer.csproj", "ClientManagement.BusinessLogicLayer/"]
COPY ["ClientManagement.Utils/ClientManagement.Utils.csproj", "ClientManagement.Utils/"]
COPY ["ClientManagement.Presentation.WebPages/ClientManagement.Presentation.WebPages.csproj", "ClientManagement.Presentation.WebPages/"]
COPY ["Core.Presentation.ViewComponents/Core.Presentation.ViewComponents.csproj", "Core.Presentation.ViewComponents/"]
RUN dotnet restore "./ClientManagement.Presentation.Web/ClientManagement.Presentation.Web.csproj"
COPY . .
WORKDIR "/src/ClientManagement.Presentation.Web"
RUN dotnet build "./ClientManagement.Presentation.Web.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClientManagement.Presentation.Web.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientManagement.Presentation.Web.dll"]