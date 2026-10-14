from mcr.microsoft.com/dotnet/sdk:10.0

workdir /app

#apenas para otimização, vai copiar primeiramente apenas o "requeriments" para o container
copy overload-csharp.csproj . 

run dotnet restore

copy . .

expose 5071

cmd ["dotnet", "watch"]
