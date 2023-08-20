# MVP-04: Fund transfer Worker to process queue messages

Deliveries:

 * a POST endpoint that insert an fund transfer
 * a GET endpoint that returns an author list
 * a database implementation using MongoDB
 * a RequestIdMiddleware for logging
 * log request endpoint operations
 * RabbitMQ publish implentation
 * add Dockerfile.yml and docker-compose.yml
 * add `Installing, testing and running` README.md section
 * fund transfer worker creation
 * update database with status and error message when applicable
 * use HTTP client factory with polly to improve resilience

## Applied principles

 * DRY & KISS

   * create projet from zero
   * use a simple data access implementation
   * use make approach to test endpoint
 
 * MVP
 
   * fund transfer worker creation
   * update database with status and error message when applicable

## Implementation details

External http request is implemented based on [Microsoft suggestion](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests).

A improvement to be considered is use of a SDK to avoid code duplication between projects. This worker contains a copy of some code available on API:

 * [TransactionsDatabaseService.cs](../FundTransferWorker/Services/TransactionsDatabaseService.cs)
 * [DatabaseSettings.cs](../FundTransferWorker/Model/DatabaseSettings.cs)
 * [QueueSettings.cs](../FundTransferWorker/Model/QueueSettings.cs)
 * [Transactions.cs](../FundTransferWorker/Model/Transaction.cs)


``` bash
├── FundTransferWorker.csproj
├── FundTransferWorker.sln
├── Makefile
├── Model
│   ├── AccountApiSettings.cs
│   ├── AccountValidationResponse.cs
│   ├── DatabaseSettings.cs
│   ├── QueueSettings.cs
│   └── Transaction.cs
├── Program.cs
├── Properties
│   └── launchSettings.json
├── Services
│   ├── FundTransferService.cs
│   └── TransactionsDatabaseService.cs
├── Worker.cs
├── appsettings.Development.json
└── appsettings.json
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
