#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM daisukekobayashi/darknet:cpu-cv AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5000
# DARKNET MODEL SETUP
WORKDIR /
RUN apt-get update -y && apt-get upgrade -y
RUN apt-get install unrar -y && apt-get install wget -y
RUN wget https://raw.githubusercontent.com/Redoublet-org/Redoublet-card-detection/main/trained-models/paper/yolov4-obj_9000.part1.rar \
    && wget https://raw.githubusercontent.com/Redoublet-org/Redoublet-card-detection/main/trained-models/paper/yolov4-obj_9000.part2.rar \ 
    && wget https://raw.githubusercontent.com/Redoublet-org/Redoublet-card-detection/main/trained-models/paper/yolov4-obj_9000.part3.rar \ 
    && unrar x yolov4-obj_9000.part1.rar \
    && mv yolov4-obj_9000.weights yolov4.weights \ 
    && rm yolov4-obj_9000.* \
    # && wget -O yolov4.data https://raw.githubusercontent.com/Redoublet-org/Redoublet-card-detection/main/trained-models/paper/obj_9000.data \
    && wget -O yolov4.cfg https://raw.githubusercontent.com/Redoublet-org/Redoublet-card-detection/main/trained-models/paper/yolov4-obj_9000.cfg \
    && touch yolov4.data && echo 'classes = 52' > yolov4.data && echo 'names = /yolov4.names' >> yolov4.data \
    && wget -O yolov4.names https://raw.githubusercontent.com/Redoublet-org/Redoublet-card-detection/main/trained-models/paper/obj_9000.names

ENV ASPNETCORE_URLS http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER true
# Add dotnet to path
RUN wget https://packages.microsoft.com/config/ubuntu/22.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \ 
    && dpkg -i packages-microsoft-prod.deb && rm packages-microsoft-prod.deb
RUN wget https://packages.microsoft.com/ubuntu/20.04/prod/pool/main/n/netstandard-targeting-pack-2.1/netstandard-targeting-pack-2.1_2.1.0-1_amd64.deb -O pkg.deb \
    && dpkg -i pkg.deb && rm pkg.deb
RUN apt-get update && \
    apt-get install -y dotnet-sdk-6.0
RUN apt-get update -y && apt-get install -y aspnetcore-runtime-6.0


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["card-detection-api/card-detection-api.csproj", "card-detection-api/"]
RUN dotnet restore "card-detection-api/card-detection-api.csproj"
COPY . .
WORKDIR "/src/card-detection-api"
RUN dotnet build "card-detection-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "card-detection-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN rm darknetconfig.txt && touch darknetconfig.txt \
    && echo 'darknet' > darknetconfig.txt && echo '/yolov4.data' >> darknetconfig.txt \
    && echo '/yolov4.cfg' >> darknetconfig.txt && echo '/yolov4.weights' >> darknetconfig.txt
ENTRYPOINT ["dotnet", "card-detection-api.dll"]