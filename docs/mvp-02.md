# MVP-01: POST fund transfer and GET status endpoints

Deliveries:

 * a POST endpoint that insert an fund transfer
 * a GET endpoint that returns an author list
 * a database implementation using MongoDB
 * a RequestIdMiddleware for logging
 * log request endpoint operations

## Applied principles

 * DRY & KISS

   * create projet from zero
   * use a simple data access implementation
   * use make approach to test endpoint
 
 * MVP
 
   * deliver a POST endpoint that receive an fund transfer request
   * deliver a GET endpoint that returns fund transfer status
   * deliver a Makefile with tests saving and retrieving authors

## Implementation details

Database access is implemented based on [Microsoft suggestion](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-7.0&tabs=visual-studio-code).

``` bash
├── Controllers
│   └── FundTransferController.cs
├── Domain
│   ├── Account.cs
│   ├── DatabaseSettings.cs
│   ├── Enums
│   │   └── TransferStatus.cs
│   ├── Requests
│   │   └── TransactionRequest.cs
│   ├── Responses
│   │   └── TransferStatusResponse.cs
│   └── Transaction.cs
├── FundTransfer.csproj
├── FundTransfer.sln
├── Makefile
├── Middlewares
│   └── RequestIdMiddleware.cs
├── Program.cs
├── Properties
│   └── launchSettings.json
├── Services
│   ├── DatabaseService.cs
│   └── TransferService.cs
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
