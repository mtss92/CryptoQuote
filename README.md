# CryptoQuote
CryptoQuote is a web api that shows the latest cryptocurrency prices in different currencies like USD, EUR, BRL, GBP, and AUD.


## Prerequisites

Before running the application, ensure the following are prepared:

### 1. Environment Variables

The application requires API tokens for [ExchangeRatesApi.com](https://exchangeratesapi.io/) and [CoinMarketCap.io](https://coinmarketcap.com). Create a `.env` file in the root directory and add the following variables:

```env
ExchangeRatesApi__Token=your_a_com_token
CoinMarketCapApi__Token=your_b_com_token
```

### 2. Docker Installed
Ensure that Docker is installed on your system.


## How to Run
Follow these steps to run the application:

1. Clone the repository:

    ```bash
    git clone https://github.com/mtss92/CryptoQuote.git
    cd CryptoQuote
    ```

2. Ensure the `.env` file is correctly set up with the required tokens as described in the Prerequisites section.

3. Run the application using Docker Compose:

    ```bash
    docker-compose up
    ```

4. Once the application starts, it will be available at:
[Localhost:7080](http://localhost:7080)

    If port `7080` is inuse in your system you can replace it in your `docker-compose.yml`.

5. To stop the application, press `CTRL+C` in the terminal, then remove the containers with:

    ```bash
    docker-compose down
    ```

## How to Use the API

This software provides three API endpoints for currency and cryptocurrency exchange rate information. Below are the details:

### **Get Exchange Rates**

Retrieve the current exchange rates for `USD` , `EUR` , `BRL` , `GBP` , `AUD`

- **HTTP Method**: `GET`
- **Endpoint**:  
  ```plaintext
  http://localhost:7080/ExchangeRate
  ```

### **Get Cryptocurrency Quote**
Retrieve the exchange rate of a cryptocurrency against `USD` , `EUR` , `BRL` , `GBP` , `AUD`

- **Query Parameters**: `symbol`
  - `symbol` *required*: The `symbol` of the cryptocurrency (e.g., `BTC`, `ETH`).
- **Example Request**:
    ```bash
    http://localhost:7080/CryptoQuote?symbol=BTC
    ```

### **Get Cryptocurrency Quote with Currency Rates**
Retrieve the exchange rate of a cryptocurrency against `USD` , `EUR` , `BRL` , `GBP` , `AUD` and retrieve the current exchange rates for currencies


- **Query Parameters**: `symbol`
  - `symbol` *required*: The `symbol` of the cryptocurrency (e.g., `BTC`, `ETH`).
- **Example Request**:
    ```bash
    http://localhost:7080/CryptoQuote/details?symbol=BTC
    ```


## Future Features

### Test

- [ ] Implement unit tests for different possible scenarios

### Security

- [ ] Implement rate limiting to prevent abuse of API endpoints.
- [ ] Authentication mechanisms, such as OAuth, to restrict access.

### Logging

- [ ] Increased log at more sensitive points for easier troubleshooting

### Output Caching

- [ ] Implement caching (e.g., Redis) for API responses to reduce latency and improve performance.
