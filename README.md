# country/state api

## example api to compare to
`https://xc-countries-api.herokuapp.com/api/countries/`

## add new country
`post -h Content-Type=application/json -c "{"name":"America","code":"US"}"`

## add new state

`post -h Content-Type=application/json -c "{"name":"Virginia","code":"VA","countryid":"1"}"`