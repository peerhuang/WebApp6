pipeline {
    agent any
	parameters {
		string(name: 'DOCKERTAG', defaultValue: '', description: '')
        booleanParam(name: 'PUSH', defaultValue: true, description: '')
    }
	environment {
		DOCKERNAME = "peerhuang/webapp6"
		DOCKERFILENAME = "Dockerfile"
		PROJECT = "WebApp6/WebApp6.csproj"
		DLL = "WebApp6.dll"
		BASE = "mcr.microsoft.com/dotnet/aspnet:6.0"
		PORT = ""
	}
    stages {
        stage('build') {
            steps {
                script {
				    if(isUnix()) {
                        sh '''
                        [ -z "$CLEAN" ] && CLEAN='true'
                        [ -z "$PUSH" ] && PUSH='true'
						[ "$CLEAN" = 'true' ] && rm -rf publish
						if [ -n "$DLL" ];then
                        	dotnet publish $PROJECT -c:Release --output publish --self-contained false /p:DebugType=None /p:DebugSymbols=false
						else
						    (yarn install && yarn build) || (npm install && npm build)
						fi
                        '''
					}else {
                        bat '''
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
						[ -z "$BASE" ] && BASE="mcr.microsoft.com/dotnet/aspnet:6.0"
						[ -z "$DOCKERFILENAME" ] && DOCKERFILENAME='Dockerfile'
						[ -e "$DOCKERFILENAME" ] && DFName="$DOCKERFILENAME" || DFName='Dockerfile.auto'
						if [ -n "$DOCKERTAG" ];then
							dockerWithTag=$dockerWithoutTag:$DOCKERTAG
						else
							dockerWithTag=$dockerWithoutTag:$BRANCH_NAME
							#dockerWithTag=$dockerWithoutTag:`date +%Y`
						fi
                        if [ -n "$dockerWithoutTag" ];then
							if [ "$DFName" != "$DOCKERFILENAME" ];then
								[ -e $DFName ] && rm -f $DFName
echo "
FROM $BASE
RUN sed -i 's/deb.debian.org/mirrors.aliyun.com/' /etc/apt/sources.list \
&& apt update \
&& apt install -y curl
WORKDIR /app
COPY publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo Asia/Shanghai > /etc/timezone
" > $DFName
								if [ -n "$PORT" ];then
									echo EXPOSE $PORT >> $DFName
									echo HEALTHCHECK --interval=5s --timeout=2s --retries=12 CMD curl --silent --fail localhost:$PORT/health || exit 1
									echo ENTRYPOINT ['"dotnet"', '"'$DLL'"', '"--urls"', '"'http://*:$PORT'"'] >> $DFName
								else
									echo ENTRYPOINT ['"dotnet"', '"'$DLL'"'] >> $DFName
								fi
							fi
							#clean image
							if [ "$CLEAN" = 'true' ];then
								existImages=`docker images | grep $dockerWithoutTag | awk '{print $3}'`
								[ -n "$existImages" ] && docker rmi -f $existImages
							fi
							#build image
							docker build -f ./$DFName -t $dockerWithTag --pull=true .
							#push image
							[ "$PUSH" != 'false' ] && docker push $dockerWithTag
							if [ -z "$DOCKERTAG" ];then
								#update latest
								docker tag $dockerWithTag $dockerWithoutTag
								[ "$PUSH" != 'false' ] && docker push $dockerWithoutTag
							fi
                        fi
                        '''
					}
				}
            }
        }
    }
}