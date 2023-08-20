ignore := 'test*|obj|bin'
basePath := ~/projects/personal/interviews/digisystem/fundtransfer/FundTransferWorker
bin := ./bin/Release/net6.0/FundTransferWorker

transaction := '{ "accountOrigin": "$(origin)",  "accountDestination": "$(destination)",  "value": $(value) }'
url := http://localhost:5010/api/fund-transfer

# Usage samples:
# 
#   make build
#   make publish
#   make run
#   make all

build:
		@dotnet build --configuration Release
		@tree . -I $(ignore)

publish:
		@dotnet publish --configuration Release
		@tree . -I $(ignore)

run:
		@dotnet run

clean:
		@cd $(basePath) && rm -rf ./bin ./obj
		@tree . -I $(ignore)

all: clean build publish run

start:
		@docker run --rm -d -p 5000:80 --name fundtransfer-account-api baldini/testacesso
		@docker run --rm -d -p 27017:27017 --name fundtransfer-mongo mongo:4
		@docker run --rm -d -p 5672:5672 -p 15672:15672 --name fundtransfer-queue rabbitmq:3-management
