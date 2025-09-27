#include "arduino_task_timer.h"

// Create three timers with different intervals
ArduinoTaskTimer timer1(60000);  // 1 minute
ArduinoTaskTimer timer2(30000);  // 30 seconds
ArduinoTaskTimer timer3(10000);  // 10 seconds

ArduinoTaskTimer photoResistorCheckTimer(6 * 60 * 60 * 1000); //6 * 60 * 60 * 1000 - Check Photoresistor every 6 hours
ArduinoTaskTimer growlightActiveTimer(2 * 60 * 60 * 1000); // 2 * 60 * 60 * 1000 - Activate LED Lights for duration of 2 hours if conditions are met

ArduinoTaskTimer soilMoistureCheckTimer(4 * 60 * 60 * 1000); //4 * 60 * 60 * 1000 - Check Soil Moisture every 4 hours
ArduinoTaskTimer pumpActiveTimer(2 * 1000); // 2 * 1000 - Activate LED Lights for duration of 2 seconds if conditions are met

int photoResistorPin = A0;
int growLightPin = 7;
float currentPhotoResistorValue; //pct via map
float photoResistorGrowLightTriggerValue = 10; //pct, user set
bool isGrowlightActive = false;
bool isUserForceGrowLightActive = false; // Update from Adafruit Subscription

int soilMoisturePin = A5;
int pumpPin = 8;

float currentSoilMoistureValue; //pct via map
float soilMoisturePumpTriggerValue = 10; //pct, user set
bool isPumpActive = false;
bool isUserForcePumpActive = false; // Update from Adafruit Subscription

void setup() {
  Serial.begin(9600);
  pinMode(photoResistorPin, INPUT); //INPUT listens to voltage changes on the pin
  pinMode(growLightPin, OUTPUT); //OUTPUT transmit signal to the component

  pinMode(soilMoisturePin, INPUT);
  pinMode(pumpPin, OUTPUT);

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
    int rawPhotoResistorValue = analogRead(photoResistorPin);
    currentPhotoResistorValue = map(rawPhotoResistorValue, 0, 1023, 0, 100);

    int rawSoilMoistureValue = analogRead(soilMoisturePin);
    currentSoilMoistureValue = map(rawSoilMoistureValue, 0, 1023, 0, 100);
  }

  // ----------------------------------------------
  // Photoresistor Sensor Check and Grow Light Activity
  // ----------------------------------------------

  // Determine if it's time to check photo resistor sensor
  if (photoResistorCheckTimer.checkIsItTaskTime()) {
    if (currentPhotoResistorValue <= photoResistorGrowLightTriggerValue){
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

  // ----------------------------------------------
  // Soil Moisture Sensor Check and Pump Activity
  // ----------------------------------------------

  // Determine if it's time to check soil moisture sensor
  if (soilMoistureCheckTimer.checkIsItTaskTime()) {
    if (currentSoilMoistureValue <= soilMoisturePumpTriggerValue){
      Serial.println("Low Soil Moisture Level Detected - Activiating pump for 2 seconds");
      isPumpActive = true;
      digitalWrite(pumpPin, isPumpActive ? HIGH : LOW);

      pumpActiveTimer.resetTaskConditions();
    }
    else{
      Serial.println("Soil Moisture Level sufficient — No need to activate pump");
    }
  }

  // If user manually force pump activation outside sensor reading
  if (isUserForcePumpActive){
      // If Force activation, turn on pump and reset pump active timer, and turn off user trigger
      Serial.println("User has force activated pump.");
      isPumpActive = true;
      digitalWrite(pumpPin, isPumpActive ? HIGH : LOW);

      pumpActiveTimer.resetTaskConditions();
      isUserForcePumpActive = false;
  }

  if (isPumpActive){
    if (pumpActiveTimer.checkIsItTaskTime()){
      Serial.println("Pump has been active for X seconds, turning off pump now.");
      isPumpActive = false;
      digitalWrite(pumpPin, isPumpActive ? HIGH : LOW);

    }
  }
}