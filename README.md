Video Tutorial: https://youtu.be/BbUmLRaxZG8?t=1070

Table: WeatherForecast
PK: City(String)
SK: Date(String)

```shell
export AWS_PROFILE=localstack
aws dynamodb --endpoint-url=http://localhost:4566 list-tables
```

```json
{
  "TemperatureC": 20,
  "Summary": "Warm",
  "City": "Melbourne",
  "Date": "2023-05-13T23:02:26.270Z"
}
```
