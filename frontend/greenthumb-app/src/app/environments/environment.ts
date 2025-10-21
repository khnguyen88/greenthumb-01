export const environment = {
  production: false,
  baseUrl: 'http://localhost:7169/api/',
  adafruitData: {
    growlightTrigger: 'GetGrowLightFeedData',
    lightIntensity: 'GetPhotoResistorFeedData',
    temperature: 'GetTemperatureFeedData',
    humidity: 'GetHumidityFeedData',
    soilMoisture: 'GetSoilMoistureFeedData',
    pumpTrigger: 'GetPumpTriggeredFeedData',
    waterLevel: 'GetWaterLevelFeedData',
    plantHeight: 'GetPlantHeightData',
  },
  envName: 'Development',
};
