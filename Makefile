.PHONY: clean

DOTNET=dotnet
SOLUTION=InterknotCalculator

SERVER=$(SOLUTION).Server

PUBLISH_ARGS=-c Release /p:PublishDir=./../publish/ --self-contained true

default: server

clean:
	rm -rf ./publish

server:
	$(DOTNET) publish $(SERVER)/$(SERVER).csproj $(PUBLISH_ARGS)