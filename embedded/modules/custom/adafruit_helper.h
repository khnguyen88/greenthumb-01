// For Arduino projects using Arduino IDE only
#ifndef ADA_HELPER_H
#define ADA_HELPER_H

#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"
#include <string>
#include <cstring>

class Adafruit_MQTT_Client;

class AdafruitHelper {
    private:
    Adafruit_MQTT_Client& _mqtt;
    std::string CreateAdaFeedPath(const std::string &username, const std::string &feedName);

    public:
    // Default Constructor
    AdafruitHelper(Adafruit_MQTT_Client &mqtt);

    // Methods
    void AdaMqttClientConnect();
    Adafruit_MQTT_Publish CreateAdaMqttFeedPub(const std::string& username, const std::string& feedName);
    Adafruit_MQTT_Subscribe CreateAdaMqttFeedSub(const std::string& username, const std::string& feedName);

    template<typename T>
    void PublishToFeed(Adafruit_MQTT_Publish& adaMqttFeedPub, const std::string& feedName, T value){
        Serial.print(F("\nSending val "));
        Serial.print(value);
        std::string tmp = " to " + feedName + " feed...";
        Serial.print(tmp.c_str());
        if (!adaMqttFeedPub.publish(value)) {
        Serial.println(F("Failed"));
        } else {
        Serial.println(F("OK!"));
        }
    }
    
    void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, std::string& refVariable);
    void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, bool& refVariable, bool flip = false);
    void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, int& refVariable);
    void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, double& refVariable);
    void UpdateWithSubscribeFeed(Adafruit_MQTT_Subscribe& adaMqttFeedSub, const std::string& feedName, float& refVariable);
};

#endif