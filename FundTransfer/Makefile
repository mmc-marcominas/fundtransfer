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
#   make test
#   make post-fund-transfer origin=1234 destination=4321 value=10

build:
		@dotnet build --configuration Release
		@tree . -I $(ignore)

publish:
		@dotnet publish --configuration Release
		@tree . -I $(ignore)

run:
		@dotnet run

test:
		@echo 'Original array:' $(items)
		@$(bin) $(items)

clean:
		@cd $(basePath) && rm -rf ./bin ./obj
		@tree . -I $(ignore)

start:
		@docker run --rm -d -p 5000:80 --name fundtransfer-account-api baldini/testacesso
		@docker run --rm -d -p 27017:27017 --name fundtransfer-mongo mongo:4

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
