#include "arduino_task_timer.h"
#include <NewPing.h> 

// Create three timers with different intervals
ArduinoTaskTimer timer1(60000);  // 1 minute
ArduinoTaskTimer timer2(30000);  // 30 seconds
ArduinoTaskTimer timer3(10000);  // 10 seconds

ArduinoTaskTimer photoResistorCheckTimer(6 * 60 * 60 * 1000); //6 * 60 * 60 * 1000 - Check Photoresistor every 6 hours
ArduinoTaskTimer growlightActiveTimer(2 * 60 * 60 * 1000); // 2 * 60 * 60 * 1000 - Activate LED Lights for duration of 2 hours if conditions are met

ArduinoTaskTimer soilMoistureCheckTimer(4 * 60 * 60 * 1000); //4 * 60 * 60 * 1000 - Check Soil Moisture every 4 hours
ArduinoTaskTimer pumpActiveTimer(2 * 1000); // 2 * 1000 - Activate LED Lights for duration of 2 seconds if conditions are met

//Photoresistor Sensor and Grow Light Device
int photoResistorPin = A0;
int growLightPin = 13;
int rawPhotoResistorValue = 0;
float currentPhotoResistorReadingPct; //pct via map
float photoResistorGrowLightTriggerValue = 10; //pct, user set
bool isGrowlightActive = false;
bool isUserForceGrowLightActive = false; // Update from Adafruit Subscription


//Soil Moisture Sensor and Pump Device
int soilMoisturePin = A5;
int waterPumpPin = 3;
int rawSoilMoistureValue = 0;
float currentSoilMoistureReadingPct; //pct via map
float soilMoisturePumpTriggerValue = 10; //pct, user set
bool isPumpActive = false;
bool isUserForcePumpActive = false; // Update from Adafruit Subscription


//Ultrasonic Sonar Sensors
const int sonarWaterEchoPin = 6; //Ultrasonic sensor echo pin
const int sonarWaterTrigPin = 7; //Ultrasonic sensor trigger pin
const float minAllowWaterLevelInch = 2.0; // Minimum water level to keep the pump submerged, inches
const float maxAllowWaterLevelInch = 5.0; // Height of the water container, inches
float initNoWaterDepthInch = 0; //Currently the sonar is hitting the pump. We need to account and adjust for the depth differences
float currentWaterLevelInch = 0;
const float calibratedWaterSensorLevel = minAllowWaterLevelInch; //To Adjust for any obstruction or constraints placed on the sensor depth

const int sonarPlantEchoPin = 8; //Ultrasonic sensor echo pin
const int sonarPlantTrigPin = 9; //Ultrasonic sensor trigger pin
float initDepthToSoilInch = 0;
float currentPlantHeightInch = 0;
const float maxPlantHeight = 6.0; // Minimum water level to keep the pump submerged, inches
const float calibratedPlantSensorHeight = 0; //To Adjust for any obstruction or constraints placed on the sensor depth

const int maxDistanceCm = 200; //ultrasonic sensor, cm

NewPing sonarWater(sonarWaterTrigPin, sonarWaterEchoPin, maxDistanceCm);
NewPing sonarPlant(sonarPlantTrigPin, sonarPlantEchoPin, maxDistanceCm);

void setup() {
  Serial.begin(9600);
  pinMode(photoResistorPin, INPUT); //INPUT listens to voltage changes on the pin
  pinMode(growLightPin, OUTPUT); //OUTPUT transmit signal to the component

  rawPhotoResistorValue = analogRead(photoResistorPin);
  currentPhotoResistorReadingPct = map(rawPhotoResistorValue, 0, 1023, 0, 100);
  Serial.print("Initial Photo Resistor Reading (pct): ");
  Serial.println(currentPhotoResistorReadingPct);

  pinMode(soilMoisturePin, INPUT);
  pinMode(waterPumpPin, OUTPUT);

  rawSoilMoistureValue = analogRead(soilMoisturePin);
  currentSoilMoistureReadingPct = map(rawSoilMoistureValue, 588, 255, 0, 100);
  Serial.print("Initial Soil Moisture Sensor Reading (pct): ");
  Serial.println(currentSoilMoistureReadingPct);

  // Depth Sensor
  initNoWaterDepthInch = (float)(sonarWater.ping_cm()/2.54) + calibratedWaterSensorLevel;
  Serial.print("Setup for Initial (No) Water Sensor Depth Reading(in): ");
  Serial.println(initNoWaterDepthInch);

  initDepthToSoilInch = (float)(sonarPlant.ping_cm()/2.54) + calibratedPlantSensorHeight;
  Serial.print("Setup for Initial Plant (Just Soil) Sensor Depth Reading (in): ");
  Serial.println(initDepthToSoilInch);
}

void loop() {
  // Check and act on timer1
  // One general timer can be used to publish data (send data)
  if (timer1.checkIsItTaskTime()) {
    Serial.println("Timer 1: 1 minute passed");
  }

  // Check and act on timer2
  // One general timer can be used to subscribe data (recieve data)
  if (timer2.checkIsItTaskTime()) {
    Serial.println("Timer 2: 30 seconds passed");
  }

  // Check and act on timer3
  // One general timer can be used to update sensor reading
  if (timer3.checkIsItTaskTime()) {
    Serial.println("Timer 3: 10 seconds passed");
    rawPhotoResistorValue = analogRead(photoResistorPin);
    currentPhotoResistorReadingPct = map(rawPhotoResistorValue, 0, 1023, 0, 100);

    Serial.print("Light Intensity Pct: ");
    Serial.print(currentPhotoResistorReadingPct); 
    Serial.println("%");  

    rawSoilMoistureValue = analogRead(soilMoisturePin);
    currentSoilMoistureReadingPct = map(rawSoilMoistureValue, 588, 255, 0, 100);
  
    Serial.print("Soil Moisture Pct: ");
    Serial.print(currentSoilMoistureReadingPct); 
    Serial.println("%");  

    sonarDistance(sonarPlant, "Plant", initDepthToSoilInch, currentPlantHeightInch);
    sonarDistance(sonarWater, "Water", initNoWaterDepthInch, currentWaterLevelInch);
  }

  // ----------------------------------------------
  // Photoresistor Sensor Check and Grow Light Activity
  // ----------------------------------------------

  // Determine if it's time to check photo resistor sensor
  if (photoResistorCheckTimer.checkIsItTaskTime()) {
    if (currentPhotoResistorReadingPct <= photoResistorGrowLightTriggerValue){
      Serial.println("Low Light Level Detected - Activiating grow lights for 2 hours");
      isGrowlightActive = true;
      digitalWrite(growLightPin, isGrowlightActive ? HIGH : LOW);

      growlightActiveTimer.resetTaskConditions();
    }
    else{
      Serial.println("Light level sufficient — No need to turn on LED grow lights");
    }
  }

  // If user manually force Grow Light activation outside sensor reading
  if (isUserForceGrowLightActive){
      // If Force activation, turn on growlight and reset grow light active timer, and turn off user trigger
      Serial.println("User has force activated growlight.");
      isGrowlightActive = true;
      digitalWrite(growLightPin, isGrowlightActive ? HIGH : LOW);

      growlightActiveTimer.resetTaskConditions();
      isUserForceGrowLightActive = false;
  }

  if (isGrowlightActive){
    if (growlightActiveTimer.checkIsItTaskTime()){
      Serial.println("Growlight has been active for X hours, turning off growlight now.");
      isGrowlightActive = false;
      digitalWrite(growLightPin, isGrowlightActive ? HIGH : LOW);

    }
  }

  // -------------------------------------------------------------
  // Soil Moisture Sensor and Water Depth, and Pump Activity
  // --------------------------------------------------------------

  // Determine if it's time to check soil moisture sensor
  if (soilMoistureCheckTimer.checkIsItTaskTime()) {
    if (currentSoilMoistureReadingPct <= soilMoisturePumpTriggerValue){
      Serial.println("Low Soil Moisture Level Detected");

      if(currentWaterLevelInch >= minAllowWaterLevelInch){
        Serial.println("Water is at an acceptable level");
        Serial.println("Activating water pump");
        isPumpActive = true;
        digitalWrite(waterPumpPin, isPumpActive ? HIGH : LOW);
        pumpActiveTimer.resetTaskConditions();
      }
      else{
        Serial.println("Water is below the acceptable level");
        Serial.println("water pump will remain inactive");
      }
    }
    else{
      Serial.println("Soil Moisture Level sufficient — No need to activate pump");
    }
  }

  // If user manually force pump activation outside sensor reading
  if (isUserForcePumpActive){
      // If Force activation, turn on pump and reset pump active timer, and turn off user trigger
      Serial.println("User has force activated pump.");
      if(currentWaterLevelInch >= minAllowWaterLevelInch){
        Serial.println("Water is at an acceptable level");
        Serial.println("Activating water pump");
        isPumpActive = true;
        digitalWrite(waterPumpPin, isPumpActive ? HIGH : LOW);
        pumpActiveTimer.resetTaskConditions();
      }
      else{
        Serial.println("Water is below the acceptable level");
        Serial.println("water pump will remain inactive");
      }
      isUserForcePumpActive = false;
  }

  if (isPumpActive){
    if (pumpActiveTimer.checkIsItTaskTime()){
      Serial.println("Pump has been active for X seconds, turning off pump now.");
      isPumpActive = false;
      digitalWrite(waterPumpPin, isPumpActive ? HIGH : LOW);

    }
  }
}

void sonarDistance(NewPing& sonarSensor, String sensorName, float& initDepth, float& actualHeightLevel){
  delay(50);
  distanceCm = sonarSensor.ping_cm();
  distanceIn = (float)(distanceCm/2.54); //converts cm to in, 1 in = 2.5 cm
  actualHeightLevel = (float)(distanceIn - initDepth);
  if(sensorName == "Water"){
    actualHeightLevel = actualHeightLevel <= 0 ? -actualHeightLevel : 0;
  }
  if(sensorName == "Plant"){
    actualHeightLevel = actualHeightLevel >= 0 ? actualHeightLevel : 0;
  }

  if(distanceCm >= 0.0){
    Serial.print(sensorName + " Sonar ");
    Serial.print("Measured Height/Level: ");
    Serial.print(actualHeightLevel); 
    Serial.println(" in");
    Serial.print(sensorName + " Uncalibrated Sensor Depth Reading: ");
    Serial.print(distanceIn); //Delete if there is no obstruction.
    Serial.println(" in");
    Serial.print("Initial Measured Height/Level: ");
    Serial.println(initDepth);
  }

  delay(500);
}