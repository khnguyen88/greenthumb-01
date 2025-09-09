#include "WiFiS3.h"
#include "WiFiSSLClient.h"

#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"

#include <DHT11.h>

#include "arduino_secrets.h"
#include "adafruit_secrets.h"
#include <ArduinoJson.h>

#include "adafruit_helper.h"

/////START GLOBAL STATES//////////////////

///////please enter your sensitive data in the Secret tab/arduino_secrets.h
char ssid[] = SECRET_SSID;      // your network SSID (name)
char pass[] = SECRET_PASS;      // your network password (use for WPA, or use as key for WEP)
int keyIndex = 0;               // your network key index number (needed only for WEP)

int status = WL_IDLE_STATUS;
WiFiServer server(80);

///////please enter your sensitive data in the Secret tab/adafruit_secrets.h
WiFiSSLClient sslClient;

// Setup the MQTT client class by passing in the WiFi client and MQTT server and login details.
Adafruit_MQTT_Client mqtt(&sslClient, AIO_SERVER, AIO_SERVERPORT_SECURE, AIO_USERNAME, AIO_KEY);
AdafruitHelper mqttHelper(mqtt); 



const int ledPin = 4; //LED pin #
const int dhtPin = 7; //DHT sensor pin #
const int sonarTrigPin = 9; //Ultrasonic sensor trigger pin
const int sonarEchoPin = 10; //Ultrasonic sensor echo pin
const int maxDistance = 200; //cm

unsigned long startMillis;
unsigned long currentMillis;
unsigned long maxPeriodMillis; //5mins; 1 sec = 1000 mils


float duration, distance;
int humidity, temperature;
bool ledOnState;

DHT11 dht11(dhtPin);

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


  // Time Tracking Setup
  startMillis = millis();
  maxPeriodMillis = secondsToMillis(30); //300sec => 5min

  // LED/Sensor Setup
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);
  ledOnState = false;

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

  currentMillis = millis();

  if(currentMillis - startMillis >= maxPeriodMillis){
    //Sensor Reading
    sonarDistance(sonarTrigPin, sonarEchoPin);
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

  

    //FIX TIMER
    startMillis = currentMillis;
  }
}

void sonarDistance(int trigP, int echoP){
  digitalWrite(trigP, LOW); //clears the trigger pin
  delayMicroseconds(2);
  digitalWrite(trigP, HIGH); // activate the trigger pin
  delayMicroseconds(10);
  digitalWrite(trigP, LOW); // turn off the trigger pin

  duration = pulseIn(echoP, HIGH); // read the echo pins, return the soundwave travel time in microseconds
  distance = (duration*.0343)/2; //calculate for distance in cm
  if(distance > 0){
    Serial.print("Distance: ");
    Serial.print(distance/2.54); //converts cm to in, 1 in = 2.5 cm
    Serial.println(" in");
  }

  // delay(1000);
}

void sensorDHT(int temp, int humid, DHT11 dht){
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

// Function to connect and reconnect as necessary to the MQTT server.
// Should be called in the loop function and it will take care if connecting.


//NOTES ABOUT POINTERS:
//----------------------------------------
//int a = 42;
//int* p = &a;   // p is a pointer to int
//Serial.println(*p);  // This prints the value at p → prints 42

//&	In front of var	Get address of a variable (make pointer)
//*	Declaring pointer	This is a pointer variable
//*	Using a pointer	Dereference—get or set pointed value