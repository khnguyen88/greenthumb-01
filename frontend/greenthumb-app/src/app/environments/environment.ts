export const environment = {
  production: false,
  baseUrl: 'https://localhost:7169/api/',
  subPath: {
    adafruit: 'Adafruit/',
  },
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
