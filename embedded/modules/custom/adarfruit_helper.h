// For Arduino projects using Arduino IDE only
#ifndef ADA_HELPER_H
#define ADA_HELPER_H

#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"
#include <string>
#include <cstring>

class AdafruitHelper {
    private:
        Adafruit_MQTT_Client& _mqtt;

        std::string CreateAdaFeedPath(const std::string& username, const std::string& feedName){
            return username + "/feed/" + feedName;
        }

    public:
        // Default Constructor
        AdafruitHelper() = default;
        AdafruitHelper(Adafruit_MQTT_Client &mqtt) : _mqtt(mqtt) {};

        // Methods
        void AdaMqttClientConnect() {
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

        Adafruit_MQTT_Publish CreateAdaMqttFeedPub(const std::string& username, const std::string& feedName){
            std::string adafruitFeedPath = CreateAdaFeedPath(username, feedName);
            return Adafruit_MQTT_Publish(&_mqtt, adafruitFeedPath.c_str());
        }

        Adafruit_MQTT_Subscribe CreateAdaMqttFeedSub(const std::string& username, const std::string& feedName){
            std::string adafruitFeedPath = CreateAdaFeedPath(username, feedName);
            return Adafruit_MQTT_Subscribe(&_mqtt, adafruitFeedPath.c_str());
        }

        
        void PublishToFeed(Adafruit_MQTT_Publish& adaMqttFeedPub, std::string& feedName, T value){
            Serial.print(F("\nSending val "));
            Serial.print(value);
            Serial.print(F(" to " + feedName + " feed..."));
            if (!adaMqttFeedPub.publish(value)) {
            Serial.println(F("Failed"));
            } else {
            Serial.println(F("OK!"));
            }
        }

        void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, std::string& feedName, std::string& refVariable){
            //MQTT Subscribe ==> Retrieve data from MQTT broker
            Adafruit_MQTT_Subscribe *subscription;

            while ((subscription = _mqtt.readSubscription(1000))) {
                if (subscription == &adaMqttFeedSub) {
                    if(adaMqttFeedSub.lastread != null){
                        Serial.print(F("MQTT subscription value received for " + feedName + ": "));
                        Serial.println((char *)adaMqttFeedSub.lastread);

                        Serial.print("The current value for the referenced variable is: ");
                        Serial.println(refVariable);

                        refVariable = (char *)test_sub.lastread;
  

                        Serial.print("The new value for the referenced variable is: ");
                        Serial.println(refVariable);
                    }
                }
            }
        }

        void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, std::string& feedName, bool& refVariable, bool flip = false){
            //MQTT Subscribe ==> Retrieve data from MQTT broker
            Adafruit_MQTT_Subscribe *subscription;

            while ((subscription = _mqtt.readSubscription(1000))) {
                if (subscription == &adaMqttFeedSub) {
                    if(adaMqttFeedSub.lastread != null){
                        Serial.print(F("MQTT subscription value received for " + feedName + ": "));
                        Serial.println((char *)adaMqttFeedSub.lastread);

                        Serial.print("The current value for the referenced variable is: ");
                        Serial.println(refVariable);

                        refVariable = (bool)atoi((char *)test_sub.lastread);

                        if(flip){
                            refVariable = !refVariable;
                        }

                        Serial.print("The new value for the referenced variable is: ");
                        Serial.println(refVariable);
                    }
                }
            }
        }

        void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, std::string& feedName, int& refVariable){
            //MQTT Subscribe ==> Retrieve data from MQTT broker
            Adafruit_MQTT_Subscribe *subscription;

            while ((subscription = _mqtt.readSubscription(1000))) {
                if (subscription == &adaMqttFeedSub) {
                    if(adaMqttFeedSub.lastread != null){
                        Serial.print(F("MQTT subscription value received for " + feedName + ": "));
                        Serial.println((char *)adaMqttFeedSub.lastread);

                        Serial.print("The current value for the referenced variable is: ");
                        Serial.println(refVariable);

                        refVariable = atoi((char *)test_sub.lastread);

                        Serial.print("The new value for the referenced variable is: ");
                        Serial.println(refVariable);
                    }
                }
            }
        }

        void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, std::string& feedName, double& refVariable){
            //MQTT Subscribe ==> Retrieve data from MQTT broker
            Adafruit_MQTT_Subscribe *subscription;

            while ((subscription = _mqtt.readSubscription(1000))) {
                if (subscription == &adaMqttFeedSub) {
                    if(adaMqttFeedSub.lastread != null){
                        Serial.print(F("MQTT subscription value received for " + feedName + ": "));
                        Serial.println((char *)adaMqttFeedSub.lastread);

                        Serial.print("The current value for the referenced variable is: ");
                        Serial.println(refVariable);

                        refVariable = std::stod((char *)test_sub.lastread);

                        Serial.print("The new value for the referenced variable is: ");
                        Serial.println(refVariable);
                    }
                }
            }
        }

        void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, std::string& feedName, float& refVariable){
            //MQTT Subscribe ==> Retrieve data from MQTT broker
            Adafruit_MQTT_Subscribe *subscription;

            while ((subscription = _mqtt.readSubscription(1000))) {
                if (subscription == &adaMqttFeedSub) {
                    if(adaMqttFeedSub.lastread != null){
                        Serial.print(F("MQTT subscription value received for " + feedName + ": "));
                        Serial.println((char *)adaMqttFeedSub.lastread);

                        Serial.print("The current value for the referenced variable is: ");
                        Serial.println(refVariable);

                        refVariable = std::stof((char *)test_sub.lastread);

                        Serial.print("The new value for the referenced variable is: ");
                        Serial.println(refVariable);
                    }
                }
            }
        }
}