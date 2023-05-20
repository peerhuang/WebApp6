pipeline {
    agent any
	parameters {
		string(name: 'DOCKERTAG', defaultValue: '', description: '')
        booleanParam(name: 'PUSH', defaultValue: true, description: '')
		booleanParam(name: 'CLEAN', defaultValue: false, description: '')
    }
	environment {
		PROJECT = "WebApp6/WebApp6.csproj"
		DLL = "WebApp6.dll"
		DOCKERNAME = "peerhuang/webapp6"
		BASE = "mcr.microsoft.com/dotnet/aspnet:6.0"
		PORT = "80"
	}
    stages {
        stage('dotnet build') {
            steps {
                script {
					if(isUnix()) {
                        sh '''
							if [ "$CLEAN" = 'true' ];then
								rm -rf publish
							fi
                            dotnet publish $PROJECT -c:Release --output publish --self-contained false /p:DebugType=None /p:DebugSymbols=false
                        '''
					}else {
                        bat '''
                            dotnet publish $PROJECT -c:Release --output publish --self-contained false /p:DebugType=None /p:DebugSymbols=false
                        '''
					}
				}
            }
        }
		stage('build docker') {
            steps {
                script {
					if(isUnix()) {
                        sh '''
							#!/bin/sh
							dockerWithoutTag=$DOCKERNAME
							Dockerfile='Dockerfile'
							if [ -n "$DOCKERTAG" ];then
								dockerWithTag=$dockerWithoutTag:$DOCKERTAG
							else
								dockerWithTag=$dockerWithoutTag:`date +%Y`
							fi
                            if [ -n "$dockerWithoutTag" ];then
								[ -e $Dockerfile ] && rm -f $Dockerfile
echo "
FROM $BASE
RUN sed -i 's/deb.debian.org/mirrors.aliyun.com/' /etc/apt/sources.list \
&& apt update \
&& apt install -y curl
WORKDIR /app
COPY . .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo Asia/Shanghai > /etc/timezone
" > $Dockerfile
								if [ -n "$PORT" ];then
									echo EXPOSE $PORT >> $Dockerfile
									echo HEALTHCHECK --interval=5s --timeout=2s --retries=12 CMD curl --silent --fail localhost:$PORT/health || exit 1
									echo ENTRYPOINT ['"dotnet"', '"'$DLL'"', '"--urls"', '"'http://*:$PORT'"'] >> $Dockerfile
								else
									echo ENTRYPOINT ['"dotnet"', '"'$DLL'"'] >> $Dockerfile
								fi
								#clean image
								if [ "$CLEAN" = 'true' ];then
									existImages=`docker images | grep $dockerWithoutTag | awk '{print $3}'`
									if [ -n "$existImages" ];then
										docker rmi -f $existImages
									fi
								fi
								#build image
								docker build -f ./$Dockerfile -t $dockerWithTag --pull=true publish
								#push image
								if [ "$PUSH" != 'false' ];then
									docker push $dockerWithTag
								fi
								if [ -z "$DOCKERTAG" ];then
									#update latest
									docker tag $dockerWithTag $dockerWithoutTag
									if [ "$PUSH" != 'false' ];then
										docker push $dockerWithoutTag
									fi
								fi
                            fi
                        '''
					}
				}
            }
        }
    }
}