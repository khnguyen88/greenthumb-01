// For Arduino projects using Arduino IDE only
#ifndef ADA_HELPER_H
#define ADA_HELPER_H

#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"
#include <string>
#include <cstring>

template<typename T>
class AdafruitHelper {
    public:
        // Default Constructor
        AdafruitHelper() = default;

        // Methods
        void AdaMqttConnect(Adafruit_MQTT_Client& mqtt) {
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

        static Adafruit_MQTT_Publish CreateAdaMqttFeedPub(Adafruit_MQTT& mqttServer, const std::string& username, const std::string& feedName){
            std::string adafruitFeedPath = CreateAdaFeedPath(username, feedName);
            return Adafruit_MQTT_Publish(&mqttServer, adafruitFeedPath.c_str());
        }

        static Adafruit_MQTT_Subscribe CreateAdaMqttFeedSub(Adafruit_MQTT& mqttServer, const std::string& username, const std::string& feedName){
            std::string adafruitFeedPath = CreateAdaFeedPath(username, feedName);
            return Adafruit_MQTT_Subscribe(&mqttServer, adafruitFeedPath.c_str());
        }

        static void PublishToFeed(Adafruit_MQTT_Publish& adaMqttFeedPub, std::string& feedName, T value){
            Serial.print(F("\nSending val "));
            Serial.print(value);
            Serial.print(F(" to " + feedName + " feed..."));
            if (! adaMqttFeedPub.publish(value)) {
            Serial.println(F("Failed"));
            } else {
            Serial.println(F("OK!"));
            }
        }

        static void UpdateWithSubscribeFeed(Adafruit_MQTT& mqttServer, Adafruit_MQTT_Subscribe& adaMqttFeedSub, std::string& feedName, T& refVariable, std::string& varType, bool flip = false){
            //MQTT Subscribe ==> Retrieve data from MQTT broker
            Adafruit_MQTT_Subscribe *subscription;

            while ((subscription = mqttServer.readSubscription(1000))) {
                if (subscription == &adaMqttFeedSub) {
                    if(adaMqttFeedSub.lastread != null){
                        Serial.print(F("MQTT subscription value received for " + feedName + ": "));
                        Serial.println((char *)adaMqttFeedSub.lastread);

                        Serial.print("The current value for the referenced variable is: ");
                        Serial.println(refVariable);

                        switch (varType){
                            case "string":
                                refVariable = (char *)test_sub.lastread;
                                break;
                            case "bool":
                                refVariable = (bool)atoi((char *)test_sub.lastread);

                                if(flip){
                                    refVariable = !refVariable;
                                }
                                break;
                            case "int":
                                refVariable = atoi((char *)test_sub.lastread);
                                break;
                            case "float":
                                refVariable = std::stof((char *)test_sub.lastread);
                                break;
                            case "double":
                                refVariable = std::stod((char *)test_sub.lastread);
                                break;
                            default:
                                Serial.println("The value will not change");
                        }

                        Serial.print("The new value for the referenced variable is: ");
                        Serial.println(refVariable);
                    }
                }
            }
        }

    private:
        std::string CreateAdaFeedPath(const std::string& username, const std::string& feedName){
            return username + "/feed/" + feedName;
        }
};