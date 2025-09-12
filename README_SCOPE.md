Embedded System
- DHT Sensor for Temp, Humidity readings
- Sonar Sensor

MQTT System
- Use Adafruit.IO MQTT Service
- Collect data of sensors
    - Data is stored in feeds
        - System Status
        - Unit Information
        - Temp
        - Humidity
        - Light Intensity
        - Plant Height
        - Water Level
        - Lighting Status = State: On/Off, Duration: second
        - Pumping StatIS
    - Allows different systems to retrieve 

Cloud

Front
- Simple Page
- Three Tabs
    - Tab One: Dashboard to display graphs data sensor
    - Tab Two: User and Device Information
    - Tab Three: (Beta) AI Assistance

Adafruit Microservice
- RESTful API that Connects to Adafruit
- To Retrieve Feed Name and Data

DateTime Microservice
- Gets the Date and Time of the current Day
- The data is used by the micros

Weather Microservice
- Connects to Weather API, obtains weather conditions of the current date and location

AI + RAG Orchestration Microservice
- ASP.NET Core Web API
- CQRS Design Pattern (for learning purposes)
- Allow Access to Weather Microservice
- Arduino Data
- Make decisions trigger water
- Text or message user

Database System
- Manages User Accounts
- Manages Devices Accounts (?)
    - Location
    - Type
    - ID