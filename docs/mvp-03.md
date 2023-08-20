# MVP-03: RabbitMQ publish implentation and code improvements

Deliveries:

 * a POST endpoint that insert an fund transfer
 * a GET endpoint that returns an author list
 * a database implementation using MongoDB
 * a RequestIdMiddleware for logging
 * log request endpoint operations
 * RabbitMQ publish implentation
 * add Dockerfile.yml and docker-compose.yml
 * add `Installing, testing and running` README.md section

## Applied principles

 * DRY & KISS

   * create projet from zero
   * use a simple data access implementation
   * use make approach to test endpoint
 
 * MVP
 
   * RabbitMQ publish implentation
   * add Dockerfile.yml and docker-compose.yml
   * add `Installing, testing and running` README.md section
   * bonus: code refactor to improve maintenability

## Implementation details

Queue access is implemented based on [RabbitMQ suggestion](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html).

``` bash
├── Controllers
│   └── FundTransferController.cs
├── Dockerfile.yml
├── Domain
│   ├── Account.cs
│   ├── DatabaseSettings.cs
│   ├── Enums
│   │   └── TransactionStatus.cs
│   ├── QueueSettings.cs
│   ├── Requests
│   │   └── TransactionRequest.cs
│   ├── Responses
│   │   └── TransactionsStatusResponse.cs
│   └── Transaction.cs
├── FundTransfer.csproj
├── FundTransfer.sln
├── Makefile
├── Middlewares
│   └── RequestIdMiddleware.cs
├── Program.cs
├── Properties
│   └── launchSettings.json
├── Services
│   ├── TransactionsDatabaseService.cs
│   ├── TransactionsQueueService.cs
│   └── TransactionsService.cs
├── appsettings.Development.json
├── appsettings.json
└── docker-compose.yml
```

## Run tests

Tests is on Makefile and use [curl](https://curl.se/) and [jq](https://jqlang.github.io/jq/) to do the job - `curl` send request and `jq` process result giving a pretty JSON output. Try install `jq` if following error occour on test execution:

``` bash
$ make get-authors
/bin/sh: 4: jq: not found
make: *** [Makefile:5: get-authors] Error 127
```

### Test POST fund-transfer

To test post author feature, try:

``` bash
$ make post-fund-transfer origin=1234 destination=4321 value=10
```

As result is expected something like this:

``` json
{
  "transactionId": "bdfedcdd-d9c5-4781-9671-c23cd01873c2"
}
```

### Test GET fund-transfer

To test authors retrieving, try:

``` bash
$ make get-fund-transfer
```

As result is expected something like this:

``` json
{
  "status": "Processing"
}
```
or
``` json
{
  "status": "Error",
  "message": "Transaction not found"
}
```