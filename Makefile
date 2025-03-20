DOTNET=dotnet

default: publish

all: publish

publish:
	$(DOTNET) publish -c Release /p:PublishDir=./../publish/ --self-contained true