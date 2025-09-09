// For Arduino projects using Arduino IDE only
#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"
#include <string>
#include <cstring>
#include "adafruit_helper.h"
#include <Arduino.h>

//Private
// Methods
std::string AdafruitHelper::CreateAdaFeedPath(const std::string& username, const std::string& feedName){
    return username + "/feed/" + feedName;
}

//Public

// Default Constructor
AdafruitHelper::AdafruitHelper(Adafruit_MQTT_Client &mqtt) : _mqtt(mqtt) {};

// Methods
void AdafruitHelper::AdaMqttClientConnect() {
    // Function to connect and reconnect as necessary to the MQTT server.
    // Should be called in the loop function and it will take care if connecting.

    int8_t ret;

    // Stop if already connected.
    if (_mqtt.connected()) {
        return;
    }

    Serial.print("Connecting to MQTT... ");

    uint8_t retries = 3;
    while ((ret = _mqtt.connect()) != 0) { // connect will return 0 for connected
        Serial.println(_mqtt.connectErrorString(ret));
        Serial.println("Retrying MQTT connection in 5 seconds...");
        _mqtt.disconnect();
        delay(5000);  // wait 5 seconds
        retries--;
        if (retries == 0) {
            // basically die and wait for WDT to reset me
            while (1);
        }
    }

    Serial.println("MQTT Connected!");
}

Adafruit_MQTT_Publish AdafruitHelper::CreateAdaMqttFeedPub(const std::string& username, const std::string& feedName){
    std::string adafruitFeedPath = CreateAdaFeedPath(username, feedName);
    return Adafruit_MQTT_Publish(&_mqtt, adafruitFeedPath.c_str());
}

Adafruit_MQTT_Subscribe AdafruitHelper::CreateAdaMqttFeedSub(const std::string& username, const std::string& feedName){
    std::string adafruitFeedPath = CreateAdaFeedPath(username, feedName);
    return Adafruit_MQTT_Subscribe(&_mqtt, adafruitFeedPath.c_str());
}

void AdafruitHelper::UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, std::string& refVariable){
    //MQTT Subscribe ==> Retrieve data from MQTT broker
    Adafruit_MQTT_Subscribe *subscription;

    while ((subscription = _mqtt.readSubscription(1000))) {
        if (subscription == &adaMqttFeedSub) {
            if(adaMqttFeedSub.lastread != nullptr){
                std::string tmp = "MQTT subscription value received for " + feedName + ": ";
                Serial.print(tmp.c_str());
                
                Serial.println((char *)adaMqttFeedSub.lastread);

                Serial.print("The current value for the referenced variable is: ");
                Serial.println(refVariable.c_str());

                refVariable = (char *)adaMqttFeedSub.lastread;


                Serial.print("The new value for the referenced variable is: ");
                Serial.println(refVariable.c_str());
            }
        }
    }
}

void AdafruitHelper::UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, bool& refVariable, bool flip){
    //MQTT Subscribe ==> Retrieve data from MQTT broker
    Adafruit_MQTT_Subscribe *subscription;

    while ((subscription = _mqtt.readSubscription(1000))) {
        if (subscription == &adaMqttFeedSub) {
            if(adaMqttFeedSub.lastread != nullptr){
                std::string tmp = "MQTT subscription value received for " + feedName + ": ";
                Serial.print(tmp.c_str());
                Serial.println((char *)adaMqttFeedSub.lastread);

                Serial.print("The current value for the referenced variable is: ");
                Serial.println(refVariable);

                refVariable = (bool)atoi((char *)adaMqttFeedSub.lastread);

                if(flip){
                    refVariable = !refVariable;
                }

                Serial.print("The new value for the referenced variable is: ");
                Serial.println(refVariable);
            }
        }
    }
}

void AdafruitHelper::UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, int& refVariable){
    //MQTT Subscribe ==> Retrieve data from MQTT broker
    Adafruit_MQTT_Subscribe *subscription;

    while ((subscription = _mqtt.readSubscription(1000))) {
        if (subscription == &adaMqttFeedSub) {
            if(adaMqttFeedSub.lastread != nullptr){
                std::string tmp = "MQTT subscription value received for " + feedName + ": ";
                Serial.print(tmp.c_str());
                Serial.println((char *)adaMqttFeedSub.lastread);

                Serial.print("The current value for the referenced variable is: ");
                Serial.println(refVariable);

                refVariable = atoi((char *)adaMqttFeedSub.lastread);

                Serial.print("The new value for the referenced variable is: ");
                Serial.println(refVariable);
            }
        }
    }
}

void AdafruitHelper::UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, double& refVariable){
    //MQTT Subscribe ==> Retrieve data from MQTT broker
    Adafruit_MQTT_Subscribe *subscription;

    while ((subscription = _mqtt.readSubscription(1000))) {
        if (subscription == &adaMqttFeedSub) {
            if(adaMqttFeedSub.lastread != nullptr){
                std::string tmp = "MQTT subscription value received for " + feedName + ": ";
                Serial.print(tmp.c_str());
                Serial.println((char *)adaMqttFeedSub.lastread);

                Serial.print("The current value for the referenced variable is: ");
                Serial.println(refVariable);

                refVariable = std::stod((char *)adaMqttFeedSub.lastread);

                Serial.print("The new value for the referenced variable is: ");
                Serial.println(refVariable);
            }
        }
    }
}

void AdafruitHelper::UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, float& refVariable){
    //MQTT Subscribe ==> Retrieve data from MQTT broker
    Adafruit_MQTT_Subscribe *subscription;

    while ((subscription = _mqtt.readSubscription(1000))) {
        if (subscription == &adaMqttFeedSub) {
            if(adaMqttFeedSub.lastread != nullptr){
                std::string tmp = "MQTT subscription value received for " + feedName + ": ";
                Serial.print(tmp.c_str());
                Serial.println((char *)adaMqttFeedSub.lastread);

                Serial.print("The current value for the referenced variable is: ");
                Serial.println(refVariable);

                refVariable = std::stof((char *)adaMqttFeedSub.lastread);

                Serial.print("The new value for the referenced variable is: ");
                Serial.println(refVariable);
            }
        }
    }
}