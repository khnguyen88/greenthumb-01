#include "WiFiS3.h"
#include "WiFiSSLClient.h"

#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"

#include <DHT11.h>
#include <NewPing.h>  //Custom & example arduino code did not measure consistent distance, used new ping library instead.
#include <RBD_LightSensor.h>

#include "arduino_secrets.h"
#include "adafruit_secrets.h"
#include <ArduinoJson.h>

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

//Analog Sensors
const int photoResistorPin = A0;
const int soilMoisturePin = A5;

//Pump Device (Digital Signal)
const int waterPumpPin = 3; // 1.6 Liters Per Minute (relay switch, technically)

//Digital Sensors
const int sonarWaterEchoPin = 6; //Ultrasonic sensor echo pin
const int sonarWaterTrigPin = 7; //Ultrasonic sensor trigger pin
float initNoWaterDepthInch = 0;
float waterLevelInch = 0;
const float minAllowWaterDepthInch = 1.5; // Minimum water level to keep the pump submerged, inches
const float maxAllowWaterDepthInch = 5.0; // Height of the water container, inches

const int sonarPlantEchoPin = 8; //Ultrasonic sensor echo pin
const int sonarPlantTrigPin = 9; //Ultrasonic sensor trigger pin
float initDepthToSoilInch = 0;
float plantHeightInch = 0;
const float maxPlantHeight = 6.0; // Minimum water level to keep the pump submerged, inches

const int maxDistanceCm = 200; //ultrasonic sensor, cm

const int dhtPin = 10; //DHT sensor pin #

//Light Devices (Digital Signal)
const int ledPin = 12; //LED pin #
const int growLightPin = 13; // Growlight pin # (really it's a relay switch)

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

//MOCK WEB API SERVER SETUP
char *api_server = "api.weather.com";

const char* weather_ca = "";

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

  //Soil Moisture
  pinMode(soilMoisturePin, OUTPUT);
  (float)analogRead(soilMoisturePin)/1023.0 * 100;

  //Pump
  pinMode(waterPumpPin, OUTPUT);
  digitalWrite(waterPumpPin, LOW);

  // Growlight Setup
  pinMode(growLightPin, OUTPUT);
  digitalWrite(growLightPin, LOW);

  // LED Setup
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);
  ledOnState = false;

  // Depth Sensor
  initNoWaterDepthInch = (float)(sonarWater.ping_cm()/2.54);
  Serial.print("Setup for Water Sensor (in): ");
  Serial.println(initNoWaterDepthInch);

  initDepthToSoilInch = (float)(sonarPlant.ping_cm()/2.54);
  Serial.print("Setup for Plant Sensor (in): ");
  Serial.println(initDepthToSoilInch);

  // Set Adafruit IO's root CA
  sslClient.setCACert(adafruitio_root_ca_exp);
  mqtt.subscribe(&test_sub);

  Serial.println("SETUP COMPLETE");
}

void loop() {
  // put your main code here, to run repeatedly:

  if(!isStartofLoop){
  Serial.println("START OF LOOPING");
  isStartofLoop = true;
  }

  currentMillis = millis();

  if(currentMillis - startMillis >= maxPeriodMillis){



    //Sensor Reading
    Serial.print("Soil Moisture Ratio: ");
    float soilMoistureRatio = (float)analogRead(soilMoisturePin)/1023.0 * 100;
    Serial.print(soilMoistureRatio); //Max threshold of analog value range is 1023.
    Serial.println("%");


    Serial.print("Light Intensity: ");
    Serial.print((float)analogRead(photoResistorPin)/1023.0 * 100); //Max threshold of analog value range is 1023.
    Serial.println("%");
    // Serial.println(light_sensor.getPercentValue());

    sonarDistance(sonarPlant, "Plant", initDepthToSoilInch, plantHeightInch);
    sonarDistance(sonarWater, "Water", initNoWaterDepthInch, waterLevelInch);
    sensorDHT(temperature, humidity, dht11);

    Serial.print("LED State: ");
    Serial.println(digitalRead(ledPin));

    //Web API Call
    // sslClient.setCACert(weather_ca);
    // getWeather();

    //MQTT Connect
    MQTT_connect();

    //MQTT Publish ==> Send data to MQTT broker
    // Now we can publish stuff!
    int32_t dummyVal = ledOnState; //int failed because its 16-bit in uno, so we need to specify int32_t, 32bit. bool can easily be converted to int, but not the other way around
    Serial.print(F("\nSending val "));
    Serial.print(dummyVal);
    Serial.print(F(" to test feed..."));
    if (! pub_test.publish(dummyVal)) {
      Serial.println(F("Failed"));
    } else {
      Serial.println(F("OK!"));
    }

    // wait a couple seconds to avoid rate limit
    delay(2000);

    //MQTT Subscribe ==> Retrieve data from MQTT broker
    Adafruit_MQTT_Subscribe *subscription;

    //Wait one second for subscription feed from MQTT Adafruit
    //If the subscription is the test_sub subscription, print value
    //We could do elseif if we have other subscription like &sub_plant_water or %sub_plant_temp
    while ((subscription = mqtt.readSubscription(1000))) {
      if (subscription == &test_sub) {
        Serial.print(F("MQTT Subscription Value Received: "));
        Serial.println((char *)test_sub.lastread);

        Serial.print("Current LED State: ");
        Serial.println(ledOnState);
        Serial.println(atoi((char *)test_sub.lastread)); //Convert char to int

        ledOnState = (bool)atoi((char *)test_sub.lastread); //This is critical. How do we convert bool to int. This works
        // ledOnState = !!atoi((char *)test_sub.lastread); //This is critical. How do we convert bool to int. This works too.
        ledOnState = !ledOnState;

        Serial.print("Flipped LED State: ");
        Serial.println(ledOnState);

        //Device Triggered
        digitalWrite(ledPin, ledOnState ? HIGH : LOW);
        digitalWrite(growLightPin, ledOnState ? HIGH : LOW);
        digitalWrite(waterPumpPin, ledOnState ? HIGH : LOW);

        Serial.print(F("NEW LED State: "));
        Serial.println(ledOnState);
      }
    }

    //FIX TIMER
    startMillis = currentMillis;
  }
}

void sonarDistance(NewPing& sonarSensor, String sensorName, float& initDepth, float& actualHeightLevel){
  delay(50);
  distanceCm = sonarSensor.ping_cm();
  distanceIn = (float)(distanceCm/2.54); //converts cm to in, 1 in = 2.5 cm
  actualHeightLevel = (float)(distanceIn - initDepth) >= 0.0 ? (float)(distanceIn - initDepth) : 0.0;

  if(distanceCm >= 0.0){
    Serial.print(sensorName + " Sonar ");
    Serial.print("Measured Height/Level: ");
    Serial.print(actualHeightLevel); 
    Serial.println(" in");
    Serial.print(sensorName + " Sensor Depth Reading: ");
    Serial.print(distanceIn); 
    Serial.println(" in");
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

long secondsToMillis(int seconds){
  return seconds * 1000;
}

long minutesToMillis(int minutes){
  return minutes * 60 * 1000;
}

long hoursToMillis(int hours){
  return hours * 60 * 60 * 1000;
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

//Code copied from Arduino Form
bool getWeather() {
  Serial.print("Starting connection to: ");
  Serial.println(api_server);
  if (sslClient.connect(api_server, 443)) {               
    Serial.print("Connected to: ");
    Serial.println(api_server);
    sslClient.stop();
    }
  else {
    Serial.println("Connection failed");
    }
   return true;
}

// Function to connect and reconnect as necessary to the MQTT server.
// Should be called in the loop function and it will take care if connecting.
void MQTT_connect() {
  int8_t ret;

  // Stop if already connected.
  if (mqtt.connected()) {
    return;
  }

  Serial.print("Connecting to MQTT... ");

  uint8_t retries = 3;
  while ((ret = mqtt.connect()) != 0) { // connect will return 0 for connected
       Serial.println(mqtt.connectErrorString(ret));
       Serial.println("Retrying MQTT connection in 5 seconds...");
       mqtt.disconnect();
       delay(5000);  // wait 5 seconds
       retries--;
       if (retries == 0) {
         // basically die and wait for WDT to reset me
         while (1);
       }
  }

  Serial.println("MQTT Connected!");
}


//NOTES ABOUT POINTERS:
//----------------------------------------
//int a = 42;
//int* p = &a;   // p is a pointer to int
//Serial.println(*p);  // This prints the value at p → prints 42

//&	In front of var	Get address of a variable (make pointer)
//*	Declaring pointer	This is a pointer variable
//*	Using a pointer	Dereference—get or set pointed value