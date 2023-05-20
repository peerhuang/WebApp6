
FROM mcr.microsoft.com/dotnet/aspnet:6.0
RUN sed -i 's/deb.debian.org/mirrors.aliyun.com/' /etc/apt/sources.list && apt update && apt install -y curl
WORKDIR /app
COPY . .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo Asia/Shanghai > /etc/timezone

EXPOSE 80
ENTRYPOINT ["dotnet", "WebApp6.dll", "--urls", "http://*:80"]
