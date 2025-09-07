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

        static void PublishToFeed(Adafruit_MQTT_Publish& adaMqttFeedPub, T value){

        }

        static void SubscribeToFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, T& refVar){
            
        }

    private:
        std::string CreateAdaFeedPath(const std::string& username, const std::string& feedName){
            return username + "/feed/" + feedName;
        }
};