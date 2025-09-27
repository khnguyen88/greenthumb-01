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
float currentPhotoResistorReadingPct = 0; // 0 to 100;

const int soilMoisturePin = A5;
const float soilMoistureAirRawValue = 588.00;//Range 0-1023, Initial Calibration required. Exposed in air, 0% saturation. 
const float soilMoistureWaterRawValue = 255.00; //Range 0-1023, Initial Calibration required. Submerged in water, 100% saturation. 
float currentSoilMoistureReadingPct = 0; // 0 to 100;

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

const bool forcePumpOn = false;
const float pumpDurationSec = soilWaterVolByIncremCubicIn / pumpFlowRateCInPS;
const long pumpDurationMillis = pumpDurationSec * 1000;
const int pumpFrequencyHr = hrsInDay / waterFreqTilFull; //Arbitary


//Digital Sensors
const int sonarWaterEchoPin = 6; //Ultrasonic sensor echo pin
const int sonarWaterTrigPin = 7; //Ultrasonic sensor trigger pin
const float minAllowWaterLevelInch = 1.5; // Minimum water level to keep the pump submerged, inches
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

const int dhtPin = 10; //DHT sensor pin #

//Light Devices (Digital Signal)
const int ledPin = 12; //LED pin #

const int growLightPin = 13; // Growlight pin # (really it's a relay switch)
int lightIntGrowLightTrigPct = 10; // If the photo resistor sensor reading reaches below this percentage 
int lightIntensityDurationHr = 2;
long lightIntensityDurationMillis = lightIntensityDurationHr * 60 * 60 * 1000;
bool forceLightOn = false; // Turns on when

unsigned long startMillis;
unsigned long currentMillis;
unsigned long maxPeriodMillis; //5mins; 1 sec = 1000 mils


float duration, distanceCm, distanceIn;
int humidity, temperature;
bool ledOnState;

DHT11 dht11(dhtPin);
NewPing sonarWater(sonarWaterTrigPin, sonarWaterEchoPin, maxDistanceCm);
NewPing sonarPlant(sonarPlantTrigPin, sonarPlantEchoPin, maxDistanceCm);
RBD::LightSensor light_sensor(photoResistorPin);

bool isStartofLoop = false;

/****************************** Feeds ***************************************/

// Setup a feed called 'test' for publishing and subscribing
// Notice MQTT paths for AIO follow the form: <username>/feeds/<feedname>
Adafruit_MQTT_Publish pub_test = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/test");
Adafruit_MQTT_Subscribe test_sub = Adafruit_MQTT_Subscribe(&mqtt, AIO_USERNAME "/feeds/test");

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
  pinMode(photoResistorPin, OUTPUT);
  (float)analogRead(photoResistorPin)/1023.0 * 100;
  delay(200);
  Serial.print("Setup for PhotoResistor (Raw Analog): ");
  Serial.println((float)analogRead(photoResistorPin));
  Serial.print("Setup for PhotoResistor (Mapped Analog): ");
  currentPhotoResistorReadingPct = (float)map(analogRead(photoResistorPin), photoResistoMaxDarkRawValue, photoResistorMaxBrightRawValue, 0, 100); // Brighter light, higher value;
  constrainMinMaxRange(currentPhotoResistorReadingPct);
  Serial.println(currentPhotoResistorReadingPct); 

  //Soil Moisture
  pinMode(soilMoisturePin, OUTPUT);
  (float)analogRead(soilMoisturePin)/1023.0 * 100;
  delay(200);
  Serial.print("Setup for Soil Moisture (Raw Analog): ");
  Serial.println((float)analogRead(soilMoisturePin));
  Serial.print("Setup for Soil Moisture (Mapped Analog): ");
  currentSoilMoistureReadingPct = (float)map(analogRead(soilMoisturePin), soilMoistureWaterRawValue, soilMoistureAirRawValue, 100, 0); //More saturation lower value. So we flipped the mapping.
  constrainMinMaxRange(currentSoilMoistureReadingPct);
  Serial.println(currentSoilMoistureReadingPct);

  //Pump
  pinMode(waterPumpPin, OUTPUT);
  digitalWrite(waterPumpPin, LOW);
  Serial.print("Pump Frequency, HR: ");
  Serial.println(pumpFrequencyHr);

  Serial.print("Pump Duration, SEC: ");
  Serial.println(pumpDurationSec);

  // Growlight Setup
  pinMode(growLightPin, OUTPUT);
  digitalWrite(growLightPin, LOW);

  // LED Setup
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);
  ledOnState = false;

  // Depth Sensor
  initNoWaterDepthInch = (float)(sonarWater.ping_cm()/2.54) + calibratedWaterSensorLevel;
  Serial.print("Setup for Water Sensor (in): ");
  Serial.println(initNoWaterDepthInch);

  initDepthToSoilInch = (float)(sonarPlant.ping_cm()/2.54) + calibratedPlantSensorHeight;
  Serial.print("Setup for Plant Sensor (in): ");
  Serial.println(initDepthToSoilInch);

  // Set Adafruit IO's root CA
  sslClient.setCACert(adafruitio_root_ca);
  mqtt.subscribe(&test_sub);

  Serial.println("SETUP COMPLETE");
}

void loop() {
  // put your main code here, to run repeatedly:

  if(!isStartofLoop){
  Serial.println("START OF LOOPING");
  isStartofLoop = true;
  }
  Serial.println("==============================");
  currentMillis = millis();

  if(currentMillis - startMillis >= maxPeriodMillis){
    //Sensor Reading
    Serial.print("Soil Moisture Ratio: ");
    currentSoilMoistureReadingPct = (float)map(analogRead(soilMoisturePin), soilMoistureWaterRawValue, soilMoistureAirRawValue, 100, 0);
    constrainMinMaxRange(currentSoilMoistureReadingPct);
    Serial.print((float)map(analogRead(soilMoisturePin), soilMoistureWaterRawValue, soilMoistureAirRawValue, 100, 0)); 
    Serial.println("%");


    Serial.print("Light Intensity: ");
    currentPhotoResistorReadingPct = (float)map(analogRead(photoResistorPin), photoResistoMaxDarkRawValue, photoResistorMaxBrightRawValue, 0, 100);
    constrainMinMaxRange(currentPhotoResistorReadingPct);
    Serial.print(currentPhotoResistorReadingPct); 
    Serial.println("%");

    sonarDistance(sonarPlant, "Plant", initDepthToSoilInch, currentPlantHeightInch);
    sonarDistance(sonarWater, "Water", initNoWaterDepthInch, currentWaterLevelInch);
    sensorDHT(temperature, humidity, dht11);

    Serial.print("LED State: ");
    Serial.println(digitalRead(ledPin));

    //MQTT Connect
    mqttHelper.AdaMqttClientConnect();

    //MQTT Publish ==> Send data to MQTT broker
    // Now we can publish stuff!
    int32_t dummyVal = ledOnState; //int failed because its 16-bit in uno, so we need to specify int32_t, 32bit. bool can easily be converted to int, but not the other way around
    mqttHelper.PublishToFeed<int32_t>(pub_test, "test", dummyVal);

    // wait a couple seconds to avoid rate limit
    delay(2000);

    //MQTT Subscribe ==> Retrieve data from MQTT broker
    mqttHelper.UpdateWithSubscribeFeed(test_sub, "test", ledOnState, true);

    Serial.print("Flipped LED State: ");
    Serial.println(ledOnState);

    digitalWrite(ledPin, ledOnState ? HIGH : LOW);
    digitalWrite(growLightPin, ledOnState ? HIGH : LOW);
    digitalWrite(waterPumpPin, ledOnState ? HIGH : LOW);

    //FIXED TIMER
    startMillis = currentMillis;
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