FROM microsoft/aspnetcore:1.1
WORKDIR /app
COPY published ./
RUN apt-get update
RUN apt-get -y install cowsay fortune
ENTRYPOINT ["dotnet", "cowsay.dll"]

