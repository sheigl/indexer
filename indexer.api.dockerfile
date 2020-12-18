FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /src

# caches restore result by copying csproj file separately
COPY ./indexer.api/*.csproj ./indexer.api/
COPY ./indexer.common/*.csproj ./indexer.common/

WORKDIR /src/indexer.api
RUN dotnet restore

# copies the rest of your code

WORKDIR /src
COPY . .
WORKDIR /src/indexer.api
RUN dotnet publish --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

ARG DB_PATH=.
ARG DB_NAME=database.db

ENV ASPNETCORE_ConnectionStrings__default="Filename=${DB_PATH}/${DB_NAME}"
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "indexer.api.dll"]