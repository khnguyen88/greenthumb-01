#include "WiFiS3.h"
#include "WiFiSSLClient.h"

#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"

#include <DHT11.h>
#include <NewPing.h>  //Custom & example arduino code did not measure consistent distance, used new ping library instead.
#include <RBD_LightSensor.h>
#include <ArduinoJson.h>

#include "arduino_secrets.h"
#include "adafruit_secrets.h"

#include "adafruit_helper.h"
#include "unit_converter.h"

#include "arduino_task_timer.h"
/////START GLOBAL STATES//////////////////

///////please enter your sensitive data in the Secret tab/arduino_secrets.h
char ssid[] = SECRET_SSID;      // your network SSID (name)
char pass[] = SECRET_PASS;      // your network password (use for WPA, or use as key for WEP)
int keyIndex = 0;               // your network key index number (needed only for WEP)

int status = WL_IDLE_STATUS;
WiFiServer server(80);

byte mac[6];  // the MAC address of your Wifi shield

///////please enter your sensitive data in the Secret tab/adafruit_secrets.h
WiFiSSLClient sslClient;

// Setup the MQTT client class by passing in the WiFi client and MQTT server and login details.
Adafruit_MQTT_Client mqtt(&sslClient, AIO_SERVER, AIO_SERVERPORT_SECURE, AIO_USERNAME, AIO_KEY);
AdafruitHelper mqttHelper(mqtt); 


//Analog Sensors
const int photoResistorPin = A0;
const float photoResistoMaxDarkRawValue = 20; //Range 0-1023, Initial Calibration required. In Pitch black environment.
const float photoResistorMaxBrightRawValue = 1023;//Range 0-1023, Initial Calibration required. In Full Light environment. Adjust for indoor/outdoor.
long currentPhotoResistorReadingRaw = 0; // 0 to 1023;
float currentPhotoResistorReadingPct = 0; // 0 to 100;
long photoResistorCheckFrequencyHr = 6;
long photoResistorCheckFrequencyMillis = hoursToMillis(photoResistorCheckFrequencyHr);

const int soilMoisturePin = A5;
const float soilMoistureAirRawValue = 588.00;//Range 0-1023, Initial Calibration required. Exposed in air, 0% saturation. 
const float soilMoistureWaterRawValue = 255.00; //Range 0-1023, Initial Calibration required. Submerged in water, 100% saturation.
long currentSoilMoistureReadingRaw = 0; // 0 to 1023;
float currentSoilMoistureReadingPct = 0; // 0 to 100;
long soilMoistureCheckFrequencyHr = 4;
long soilMoistureCheckFrequencyMillis = hoursToMillis(soilMoistureCheckFrequencyHr);


//Pump Device (Digital Signal)
const float soilVoidRatio = 0.4;
const int minPlantSoilMoistPct = 40;
const int waterFreqTilFull = 4;
const int containerVolCubicIn = 9 * 5 * 3; // Container dimension based on spec
const int soilVolCubicIn = containerVolCubicIn * (2 / 3); // Only 2/3 of the container will be filled with soil.
const float soilVoidVolCubicIn = (float)soilVolCubicIn * soilVoidRatio; // Actual space water can fill in soil is the void space and volume
const float allowableSoilWaterVolCubicIn = soilVoidVolCubicIn * (float)(minPlantSoilMoistPct/100); // The actual amount of water a plan is happy with is the never 100%. Every plant prefer a certain moisture content. This factors it.
const float soilWaterVolByIncremCubicIn = (float)(allowableSoilWaterVolCubicIn / waterFreqTilFull); // This calculates the amount of water we want to fill each time the pump is activated, based on user preferred increments

const float pumpFlowRateLPM = 1.6;  // 1.6 Liter Per Minutes based on Specs at 3.3V
const float pumpFlowRateCInPM = pumpFlowRateLPM * (float)cubicInPerLiter;
const float pumpFlowRateCInPS = pumpFlowRateCInPM * (1 / 60);
const int waterPumpPin = 3;

const float tubeTravelTimeSec = 1.0; //Estimated, but it can be calculated too. Area of tube * length / flow-rate;
const float pumpActiveDurationSec = soilWaterVolByIncremCubicIn / pumpFlowRateCInPS + tubeTravelTimeSec;
const long pumpActiveDurationMillis = secondsToMillis(pumpActiveDurationSec);

float soilMoisturePumpTriggerValue = (float)minPlantSoilMoistPct;
bool isPumpActive = false;
bool isUserForcePumpActive = false; // Update from Adafruit Subscription


//Digital Sensors
const int sonarWaterEchoPin = 6; //Ultrasonic sensor echo pin
const int sonarWaterTrigPin = 7; //Ultrasonic sensor trigger pin
const float minAllowWaterLevelInch = 2.0; // Minimum water level to keep the pump submerged, inches + a lil safety factor. Pump height is 1.57 inches.
const float maxAllowWaterLevelInch = 5.0; // Height of the water container, inches
float initNoWaterDepthInch = 0; //Currently the sonar is hitting the pump. We need to account and adjust for the depth differences
float currentWaterLevelInch = 0;
const float calibratedWaterSensorLevelInch = 0; //To Adjust for any obstruction or constraints placed on the sensor depth

const int sonarPlantEchoPin = 8; //Ultrasonic sensor echo pin
const int sonarPlantTrigPin = 9; //Ultrasonic sensor trigger pin
float initDepthToSoilInch = 0;
float currentPlantHeightInch = 0;
const float maxPlantHeight = 6.0; // Minimum water level to keep the pump submerged, inches
const float calibratedPlantSensorHeightInch = 0; //To Adjust for any obstruction or constraints placed on the sensor depth

const int maxDistanceCm = 200; //ultrasonic sensor, cm

const int dhtPin = 10; //DHT sensor pin #
int currentHumidityPct;
int currentTempCel;

//Light Devices (Digital Signal)
const int ledPin = 12; //LED pin #

const int growLightPin = 13; // Growlight pin # (really it's a relay switch)
int lightIntGrowLightTrigPct = 10; // If the photo resistor sensor reading reaches below this percentage 
int growlightActiveDurationHrs = 1;
long growlightActiveDurationMillis = hoursToMillis(growlightActiveDurationHrs) ;
bool isUserForceGrowLightActive = false; // Update from Adafruit Subscription based on User Input
bool isGrowlightActive = false;

unsigned long startMillis;
unsigned long currentMillis;
unsigned long maxPeriodMillis; //5mins; 1 sec = 1000 mils

bool isLedActive;

DHT11 dht11(dhtPin);
NewPing sonarWater(sonarWaterTrigPin, sonarWaterEchoPin, maxDistanceCm);
NewPing sonarPlant(sonarPlantTrigPin, sonarPlantEchoPin, maxDistanceCm);
RBD::LightSensor light_sensor(photoResistorPin);

bool isStartofLoop = false;

//ACTUAL
// ArduinoTaskTimer photoResistorCheckTimer(photoResistorCheckFrequencyMillis);
// ArduinoTaskTimer growlightActiveTimer(soilMoistureCheckFrequencyMillis);

// ArduinoTaskTimer soilMoistureCheckTimer(4 * 60 * 60 * 1000); 
// ArduinoTaskTimer pumpActiveTimer(pumpActiveDurationMillis);

//FOR TESTING ONLY
ArduinoTaskTimer photoResistorCheckTimer(photoResistorCheckFrequencyMillis);
ArduinoTaskTimer growlightActiveTimer(growlightActiveDurationMillis); 

ArduinoTaskTimer soilMoistureCheckTimer(soilMoistureCheckFrequencyMillis);
ArduinoTaskTimer pumpActiveTimer(pumpActiveDurationMillis);

long publishFrequencyHrs = 1;
long subscribeFrequencyHrs = 1;
long sensorReadingFrequencyMinutes = 1;

ArduinoTaskTimer publishFrequencyTimer(hoursToMillis(publishFrequencyHrs));
ArduinoTaskTimer subscribeFrequencyTimer(hoursToMillis(subscribeFrequencyHrs));
ArduinoTaskTimer sensorReadingFrequencyTimer(minutesToMillis(sensorReadingFrequencyMinutes));
/****************************** Feeds ***************************************/

// Setup a feed called 'test' for publishing and subscribing
// Notice MQTT paths for AIO follow the form: <username>/feeds/<feedname>
Adafruit_MQTT_Publish pub_test = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/test");
Adafruit_MQTT_Subscribe sub_test = Adafruit_MQTT_Subscribe(&mqtt, AIO_USERNAME "/feeds/test");

Adafruit_MQTT_Publish pub_photoResistor = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/photo-resistor-pct");
Adafruit_MQTT_Publish pub_soilMoisture = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/soil-moisture-pct");
Adafruit_MQTT_Publish pub_waterLevel = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/water-level-in");
Adafruit_MQTT_Publish pub_plantHeight = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/plant-height-in");
Adafruit_MQTT_Publish pub_temperature = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/temperature-cel");
Adafruit_MQTT_Publish pub_humidity = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/humidity-pct");

Adafruit_MQTT_Publish pub_growLight = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/grow-light-bool");
Adafruit_MQTT_Publish pub_pump = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/pump-bool");



/////END GLOBAL STATE////////////////

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);

  delay(1000);
  Serial.println("SETUP STARTING");

  // Wifi Setup; Code Taken from Tutorial
  // check for the WiFi module:
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");
    // don't continue
    while (true);
  }

  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Please upgrade the firmware");
  }

  // attempt to connect to WiFi network:
  while (status != WL_CONNECTED) {
    Serial.print("Attempting to connect to Network named: ");
    Serial.println(ssid);                   // print the network name (SSID);

    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:
    status = WiFi.begin(ssid, pass);
    // wait 10 seconds for connection:
    delay(10000);
  }
  server.begin();                           // start the web server on port 80
  printWifiStatus();                        // you're connected now, so print out the status
  printMacAddress();                        // unique device identifier :)


  // Time Tracking Setup
  startMillis = millis();
  maxPeriodMillis = secondsToMillis(30); //300sec => 5min

  //Photo Resistor
  pinMode(photoResistorPin, INPUT); //INPUT listens to voltage changes on the pin
  (float)analogRead(photoResistorPin)/1023.0 * 100;
  delay(200);
  Serial.print("Setup for PhotoResistor (Raw Analog): ");
  Serial.println((float)analogRead(photoResistorPin));
  Serial.print("Setup for PhotoResistor (Mapped Analog) & ");
  Serial.print("Initial PhotoResistor Sensor Reading (pct): ");
  currentPhotoResistorReadingRaw = analogRead(photoResistorPin);
  currentPhotoResistorReadingPct = (float)map(currentPhotoResistorReadingRaw, photoResistoMaxDarkRawValue, photoResistorMaxBrightRawValue, 0, 100); // Brighter light, higher value;
  constrainMinMaxRange(currentPhotoResistorReadingPct);
  Serial.println(currentPhotoResistorReadingPct); 

  //Soil Moisture
  pinMode(soilMoisturePin, INPUT);
  (float)analogRead(soilMoisturePin)/1023.0 * 100;
  delay(200);
  Serial.print("Setup for Soil Moisture (Raw Analog): ");
  Serial.println((float)analogRead(soilMoisturePin));
  Serial.print("Setup for Soil Moisture (Mapped Analog) & ");
  Serial.print("Initial Soil Moisture Sensor Reading (pct): ");
  currentSoilMoistureReadingRaw = analogRead(soilMoisturePin);
  currentSoilMoistureReadingPct = (float)map(currentSoilMoistureReadingRaw, soilMoistureWaterRawValue, soilMoistureAirRawValue, 100, 0); //More saturation lower value. So we flipped the mapping.
  constrainMinMaxRange(currentSoilMoistureReadingPct);
  Serial.println(currentSoilMoistureReadingPct);

  //Pump
  pinMode(waterPumpPin, OUTPUT);
  Serial.print("Pump Duration, SEC: ");
  Serial.println(pumpActiveDurationSec);

  // Growlight Setup
  pinMode(growLightPin, OUTPUT); //OUTPUT transmit signal to the component
  digitalWrite(growLightPin, LOW);

  // LED Setup
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);
  isLedActive = false;

  // Depth Sensor
  initNoWaterDepthInch = (float)(sonarWater.ping_cm()/2.54) + calibratedWaterSensorLevelInch;
  Serial.print("Setup for Initial (No) Water Sensor Depth Reading(in): ");
  Serial.println(initNoWaterDepthInch);

  initDepthToSoilInch = (float)(sonarPlant.ping_cm()/2.54) + calibratedPlantSensorHeightInch;
  Serial.print("Setup for Initial Plant (Just Soil) Sensor Depth Reading (in): ");
  Serial.println(initDepthToSoilInch);

  sonarDistance(sonarPlant, "Plant", initDepthToSoilInch, currentPlantHeightInch);
  sonarDistance(sonarWater, "Water", initNoWaterDepthInch, currentWaterLevelInch);

  sensorDHT(currentTempCel, currentHumidityPct, dht11);
  
  // Set Adafruit IO's root CA
  sslClient.setCACert(adafruitio_root_ca);
  mqtt.subscribe(&sub_test);

  Serial.println("SETUP COMPLETE");
}

void loop() {
  // put your main code here, to run repeatedly:

  if(!isStartofLoop){
  Serial.println("START OF LOOPING");
  isStartofLoop = true;
  Serial.println("==============================");
  }


  // Check and act on sensorReadingFrequencyTimer
  // One general timer can be used to update sensor reading
  if (sensorReadingFrequencyTimer.checkIsItTaskTime()) {
    Serial.println("-----------------------------------------------");
    Serial.println("Reading Sensor Data Now...");
    isLedActive = true;
    digitalWrite(ledPin, isLedActive ? HIGH : LOW);

    currentPhotoResistorReadingRaw = analogRead(photoResistorPin);
    currentPhotoResistorReadingPct = (float)map(currentPhotoResistorReadingRaw, photoResistoMaxDarkRawValue, photoResistorMaxBrightRawValue, 0, 100); // Brighter light, higher value;
    constrainMinMaxRange(currentPhotoResistorReadingPct);

    Serial.print("Light Intensity Pct: ");
    Serial.print(currentPhotoResistorReadingPct); 
    Serial.println("%");  

    currentSoilMoistureReadingRaw = analogRead(soilMoisturePin);
    currentSoilMoistureReadingPct = (float)map(currentSoilMoistureReadingRaw, soilMoistureWaterRawValue, soilMoistureAirRawValue, 100, 0); //More saturation lower value. So we flipped the mapping.
    constrainMinMaxRange(currentSoilMoistureReadingPct);
  
    Serial.print("Soil Moisture Pct: ");
    Serial.print(currentSoilMoistureReadingPct); 
    Serial.println("%");  

    sonarDistance(sonarPlant, "Plant", initDepthToSoilInch, currentPlantHeightInch);
    sonarDistance(sonarWater, "Water", initNoWaterDepthInch, currentWaterLevelInch);

    sensorDHT(currentTempCel, currentHumidityPct, dht11);
    Serial.println("Sensor Reading Complete...");

    delay(500);
    isLedActive = false;
    digitalWrite(ledPin, isLedActive ? HIGH : LOW);
  }

  // ----------------------------------------------
  // Photoresistor Sensor Check and Grow Light Activity
  // ----------------------------------------------

  // Determine if it's time to check photo resistor sensor
  if (photoResistorCheckTimer.checkIsItTaskTime()) {
    if (currentPhotoResistorReadingPct <= lightIntGrowLightTrigPct){
      Serial.println("-----------------------------------------------");
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
      Serial.println("-----------------------------------------------");
      Serial.println("User has force activated growlight.");
      isGrowlightActive = true;
      digitalWrite(growLightPin, isGrowlightActive ? HIGH : LOW);

      growlightActiveTimer.resetTaskConditions();
      isUserForceGrowLightActive = false;
  }

  if (isGrowlightActive){
    if (growlightActiveTimer.checkIsItTaskTime()){
      Serial.println("-----------------------------------------------");
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

    // Check and act on publishFrequencyTimer
  // One general timer can be used to publish data (send data)
  if (publishFrequencyTimer.checkIsItTaskTime()) {
    Serial.println("-----------------------------------------------");
    Serial.println("Publishing Data Now...");
    
    //MQTT Connect
    mqttHelper.AdaMqttClientConnect();
    mqttHelper.PublishToFeed<float>(pub_photoResistor, "photo-resistor-pct", currentPhotoResistorReadingPct);
    mqttHelper.PublishToFeed<float>(pub_soilMoisture, "soil-moisture-pct", currentSoilMoistureReadingPct);
    mqttHelper.PublishToFeed<float>(pub_waterLevel, "water-level-in", currentWaterLevelInch);
    mqttHelper.PublishToFeed<float>(pub_plantHeight, "plant-height-in", currentPlantHeightInch);
    mqttHelper.PublishToFeed<int32_t>(pub_temperature, "temperature-cel", currentTempCel);
    mqttHelper.PublishToFeed<int32_t>(pub_humidity, "humidity-pct", currentHumidityPct);

    mqttHelper.PublishToFeed<int32_t>(pub_growLight, "grow-light-bool", isGrowlightActive);
    mqttHelper.PublishToFeed<int32_t>(pub_pump, "pump-bool", isPumpActive);

    Serial.println("Publishing Data Complete...");
  }

  // Check and act on subscribeFrequencyTimer
  // One general timer can be used to subscribe data (recieve data)
  if (subscribeFrequencyTimer.checkIsItTaskTime()) {
    Serial.println("-----------------------------------------------");
    Serial.println("Subsribing To Data Now...");

    //MQTT Connect
    mqttHelper.AdaMqttClientConnect();
    // mqttHelper.UpdateWithSubscribeFeed(sub_test, "test", isLedActive, true);

    Serial.println("Subsribing Data Complete...");
  }
}

void sonarDistance(NewPing& sonarSensor, String sensorName, float& initDepth, float& actualHeightLevel){
  delay(50);
  float distanceCm = sonarSensor.ping_cm();
  float distanceIn = (float)(distanceCm/2.54); //converts cm to in, 1 in = 2.5 cm
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

void sensorDHT(int& temp, int& humid, DHT11& dht){
  int result = dht.readTemperatureHumidity(temp, humid);
  Serial.print("Temperature: ");
  Serial.print(temp);
  Serial.println("°C");
  Serial.print("Humidity: ");
  Serial.print(humid);
  Serial.println("%");
}

void printWifiStatus() {
  // print the SSID of the network you're attached to:
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());

  // print your board's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.print(rssi);
  Serial.println(" dBm");
}

void printMacAddress(){
  WiFi.macAddress(mac);
  Serial.print("MAC: ");
  Serial.print(mac[5], HEX);
  Serial.print(":");
  Serial.print(mac[4], HEX);
  Serial.print(":");
  Serial.print(mac[3], HEX);
  Serial.print(":");
  Serial.print(mac[2], HEX);
  Serial.print(":");
  Serial.print(mac[1], HEX);
  Serial.print(":");
  Serial.println(mac[0], HEX);
}

void constrainMinMaxRange(float& val){
  if (val < 0){
    val = 0;
  }
  if(val > 100){
    val = 100;
  }
}

//NOTES ABOUT POINTERS:
//----------------------------------------
//int a = 42;
//int* p = &a;   // p is a pointer to int
//Serial.println(*p);  // This prints the value at p → prints 42

//&	In front of var	Get address of a variable (make pointer)
//*	Declaring pointer	This is a pointer variable
//*	Using a pointer	Dereference—get or set pointed value