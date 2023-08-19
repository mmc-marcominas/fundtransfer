# Fund Transfer Microservice

This project intends to be a implementation to a simple REST API and worker offering solution to this [Challenge](./docs/CHALLENGE.md).

The main idea is use some principles:

 * DRY - Don't Reinvent the Wheel
 * KISS - Keep It Simple Sir :)
 * MVP - Minimum Viable Product

This project aims to deliver:

 * an API with endpoint to post a fund transfer
 * an API with endpoint to get fund transfer status
 * an worker to process fund transfer available on a queue
 
 ## MVP history

  * [mvp-01](./docs/mvp-01.md) - fund transfer POST and GET endpoints
  * [mvp-02](./docs/mvp-02.md) - add MongoDB persistence and RequestIdMiddleware
