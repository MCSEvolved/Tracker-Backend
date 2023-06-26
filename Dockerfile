FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MCST_Controllers/MCST_Controller.csproj", "MCST_Controllers/"]
COPY ["MCST_Command/MCST_Command.csproj", "MCST_Command/"]
COPY ["MCST_Computer/MCST_Computer.csproj", "MCST_Computer/"]
COPY ["MCST_ControllerInterfaceLayer/MCST_ControllerInterfaceLayer.csproj", "MCST_ControllerInterfaceLayer/"]
COPY ["MCST_Enums/MCST_Enums.csproj", "MCST_Enums/"]
COPY ["MCST_Models/MCST_Models.csproj", "MCST_Models/"]
COPY ["MCST_EventBus/MCST_EventBus.csproj", "MCST_EventBus/"]
COPY ["MCST_Inventory/MCST_Inventory.csproj", "MCST_Inventory/"]
COPY ["MCST_Location/MCST_Location.csproj", "MCST_Location/"]
COPY ["MCST_Message/MCST_Message.csproj", "MCST_Message/"]
COPY ["MCST_Notification/MCST_Notification.csproj", "MCST_Notification/"]
COPY ["MCST_Auth/MCST_Auth.csproj", "MCST_Auth/"]
RUN dotnet restore "MCST_Controllers/MCST_Controller.csproj"
COPY . .
WORKDIR "/src/MCST_Controllers"
RUN dotnet build "MCST_Controller.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "MCST_Controller.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MCST_Controller.dll"]
