# Fund Transfer Microservice

This project intends to be a implementation to a simple REST API and worker offering solution to this [Challenge](./docs/CHALLENGE.md).

To original challenge on Github see: [https://github.com/acessocard/test-backend](https://github.com/acessocard/test-backend)

The main idea is use some principles:

 * DRY - Don't Reinvent the Wheel
 * KISS - Keep It Simple Sir :)
 * MVP - Minimum Viable Product

This project aims to deliver:

 * an API with endpoint to post a fund transfer
 * an API with endpoint to get fund transfer status
 * an worker to process fund transfer available on a queue
 
## Installing, testing and running

To run this solution you'll need have Docker in your system.

This solution use this docker solutions:

 * fundtransfer-account-api: baldini/testacesso
 * fundtransfer-queue RabbitMQ
 * fundtransfer-mongo MongoDB
 * fundtransfer-elk ELK stack

### Install

Change to your project directory and clone this repo.

``` bash
$ cd ~/your/projects/direcotory
$ git clone https://github.com/mmc-marcominas/fundtransfer
```

Go to created directory and then choose FundtransferAPI or FundtransferWorker project to start dependencies.

``` bash
$ cd ~/your/projects/direcotory/fundtransfer/FundtransferAPI
$ make start
```

or

``` bash
$ cd ~/your/projects/direcotory/fundtransfer/FundtransferWorker
$ make start
```

Both will start it's project dependence. An alternative is use of [FundTransferAPI/docker-compose.yml](./FundTransferAPI/docker-compose.yml) or [./FundTransferWorker/docker-compose.yml](./FundTransferWorker/docker-compose.yml) compose solutions.

For now I recomend use of FundTransferWorker compose that contains all dependencies. Solution API independ of `fund-transfer-account-api` used on worker only.

### Run project

``` bash
$ cd ~/your/projects/direcotory/fundtransfer/
$ make run
```

or

``` bash
$ cd ~/your/projects/direcotory/fundtransfer/
$ dotnet run
```

### Run tests

To perform test `run project` must be executede and running.

### Test a POST fund-transfer

``` bash
$ cd ~/your/projects/direcotory/fundtransfer/
$ make post-fund-transfer origin=1234 destination=4321 value=10
```

Expected result:

``` json
{
  "transactionId": "bdfedcdd-d9c5-4781-9671-c23cd01873c2"
}
```

### Test a GET fund-transfer

With `transactionId` result of a fund-transfer POST, try:

``` bash
$ cd ~/your/projects/direcotory/fundtransfer/
$ make get-fund-transfer id=bdfedcdd-d9c5-4781-9671-c23cd01873c2
```

Expected result:

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

 ## MVP history

  * [mvp-01](./docs/mvp-01.md) - fund transfer POST and GET endpoints
  * [mvp-02](./docs/mvp-02.md) - add MongoDB persistence and RequestIdMiddleware
  * [mvp-03](./docs/mvp-03.md) - RabbitMQ publish implentation on API and code improvements
  * [mvp-04](./docs/mvp-04.md) - add fund transfer Worker to process queue messages
  * [mvp-05](./docs/mvp-05.md) - use ELK stack to logging fund transfer Worker operations
