ignore := 'test*|obj|bin'
basePath := ~/projects/personal/interviews/digisystem/fundtransfer/FundTransfer
bin := ./bin/Release/net6.0/appName

transaction := '{ "accountOrigin": "$(origin)",  "accountDestination": "$(destination)",  "value": $(value) }'
url := http://localhost:5010/api/fund-transfer

# Usage samples:
# 
#   make build
#   make publish
#   make run
#   make post-fund-transfer origin=1234 destination=4321 value=10
#   make get-fund-transfer id=f2394e5a-91fb-493a-ad56-0aa1db99d4b0

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

post-fund-transfer:
		@echo $(url)/$(id)
		@echo $(transaction)
		@curl \
				--silent \
				-X POST $(url) \
			  -H 'accept: */*' \
				-H 'Content-Type: application/json' \
				-d $(transaction) \
				| jq .

get-fund-transfer:
		@echo $(url)/$(id)
		@curl \
				--silent \
				-X GET $(url)/$(id) \
				-H 'Content-Type: application/json' \
				| jq .
