# MVP-01: POST fund transfer and GET status endpoints

Deliveries:

 * a POST endpoint that insert an fund transfer
 * a GET endpoint that returns an author list
 * a database implementation in memory

## Applied principles

 * DRY & KISS

   * create projet from zero
   * use a simple data access implementation
   * use make approach to test endpoint
 
 * MVP
 
   * deliver a POST endpoint that receive an fund transfer request
   * deliver a GET endpoint that returns fund transfer status
   * deliver a Makefile with tests saving and retrieving fund-transfer

## Implementation details

Database in memory was implemented using a dictionary of transfer status and a list of transactions.

``` bash
├── Controllers
│   └── FundTransferController.cs
├── Domain
│   ├── Account.cs
│   ├── Enums
│   │   └── TransferStatus.cs
│   ├── Requests
│   │   └── TransactionRequest.cs
│   ├── Responses
│   │   └── TransferStatusResponse.cs
│   └── Transaction.cs
├── Makefile
├── Program.cs
├── Services
│   └── TransferService.cs
```

## Run tests

Tests is on Makefile and use [curl](https://curl.se/) and [jq](https://jqlang.github.io/jq/) to do the job - `curl` send request and `jq` process result giving a pretty JSON output. Try install `jq` if following error occour on test execution:

``` bash
$ make get-fund-transfer
/bin/sh: 4: jq: not found
make: *** [Makefile:5: get-fund-transfer] Error 127
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

To test fund-transfer retrieving, try:

``` bash
$ make get-fund-transfer id=253b4724-459b-4b0e-80ad-3a97f99a699e
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

See [CHALLENGE.md#problema](CHALLENGE.md#problema) section to possible status list.
