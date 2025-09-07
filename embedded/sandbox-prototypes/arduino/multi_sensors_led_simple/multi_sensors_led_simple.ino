#include <NewPing.h>
#include <DHT11.h>

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

// NewPing sonar(sonarTrigPin, sonarEchoPin, maxDistance); // not used
DHT11 dht11(dhtPin);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  startMillis = millis();
  maxPeriodMillis = secondsToMillis(1);
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);
  ledOnState = false;
}

void loop() {
  // put your main code here, to run repeatedly:
  currentMillis = millis();

  if(currentMillis - startMillis >= maxPeriodMillis){
    // traditional method, very inaccurate or inconsistent
    sonarDistance(sonarTrigPin, sonarEchoPin);
    sensorDHT(temperature, humidity, dht11);

    // Use NewPing library
    // delay(1000);
    // Serial.print("Ping: ");
    // Serial.print(sonar.ping_cm()/2.54);
    // Serial.println("in");

    Serial.print("LED State: ");
    Serial.println(digitalRead(ledPin));

    ledOnState = !ledOnState;
    digitalWrite(ledPin, ledOnState ? HIGH : LOW);

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
