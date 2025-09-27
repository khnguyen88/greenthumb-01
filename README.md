# mc-greenthumb-01

Automated Gardening Microcontroller Project: IoT Assisted Gardening System

# Resource References

Helpful resources for others who want to try their hands at creating their Arduino projects

<!-- MICROCONTROLLERS SECTION -->

## Microcontrollers

### Microcontrollers and Coding Basics

-   [Arduino is easy, actually](https://www.youtube.com/watch?v=tiGw9PQbvrg)
-   [Arduino - How to Split a Program Into Different Files](https://www.youtube.com/watch?v=BdstuZP6l5E)
-   [Why do header files even exist?](https://www.youtube.com/watch?v=tOQZlD-0Scc)
-   [C++ Header Files](https://www.youtube.com/watch?v=9RJTQmK0YPI)
-   [Understanding header files and .cpp file](https://cplusplus.com/forum/beginner/135636/)
-   [Simplest way to initialize multiple related const properties in a constructor?](https://stackoverflow.com/questions/15535711/simplest-way-to-initialize-multiple-related-const-properties-in-a-constructor)
-   [How to print in C++](https://www.udacity.com/blog/2021/05/how-to-print-in-cpp.html)
-   [C++ Inheritance](https://www.w3schools.com/cpp/cpp_inheritance.asp)
-   [C++ Inheritance](https://www.geeksforgeeks.org/cpp/inheritance-in-c/)
-   [C++ Encapsulation](https://www.w3schools.com/cpp/cpp_encapsulation.asp)
-   [C++ Polymorphism](https://www.w3schools.com/cpp/cpp_polymorphism.asp)
-   [Chronos Cheat Sheet; Alternative to Millis(), a built-in arduino library function](https://gist.github.com/mortie/bf21c9d2d53b83f3be1b45b76845f090)
-   [Chronos Library Details](https://en.cppreference.com/w/cpp/header/chrono.html)
-   [C++ Pure vs Regular Virtual Functions](https://www.geeksforgeeks.org/cpp/difference-between-virtual-function-and-pure-virtual-function-in-c/)
-   [Arduino Map function - For Adjusting Upper/Lower Limit Values](https://docs.arduino.cc/language-reference/en/functions/math/map/)
-   [Arduino - Pin Mode](https://docs.arduino.cc/language-reference/en/functions/digital-io/pinMode/)
-   [Arduino - Pin Mode Types](https://docs.arduino.cc/learn/microcontrollers/digital-pins/)

### Tutorials and Sample Codes

#### Sensors or Modules or Hardware

-   [ELEGOO 37 in 1 Sensor Modules Kit - Tutorial, Datasheet & Code](https://drive.google.com/file/d/1EMtCczGjfEjxzH-RrLvcUSCd7XEdTBNh/view?usp=sharing)
-   [ELEGOO UNO R3 Project The Most Complete Starter Kit - Tutorial, Datasheet & Code](https://drive.google.com/file/d/1wiPBkznSR3HUtgtNdWXdeVlMMpOGftPV/view?usp=sharing)
-   [Using the Arduino UNO R4 WiFi LED Matrix](https://docs.arduino.cc/tutorials/uno-r4-wifi/led-matrix/)
-   [Using Soil Moisture Meter + Monitor](https://www.esclabs.in/soil-moisture-monitor-using-arduino/)
-   [Sun Founder - Lesson 23: Ultrasonic Sensor Module (HC-SR04)](https://docs.sunfounder.com/projects/umsk/en/latest/02_arduino/uno_lesson23_ultrasonic.html)
-   [Controlling 120-240 VAC With a Relay Using Arduino](https://www.instructables.com/Controlling-120-240-VAC-with-a-relay-using-arduino/)
    -   Note there are varying relay switches with different voltage trigger sizes: 12V, 5V, and 3V. Look into which your device supplies.
-   [Elearn - Relay Module](https://elearn.ellak.gr/mod/book/view.php?id=4568&chapterid=2440)
-   [Shallowsky - PhotoResistor](https://shallowsky.com/arduino/class/photocell.html)
    -   [Understanding this photoresistor circuit](https://forum.arduino.cc/t/understanding-this-photoresistor-circuit/517857/2)
    -   [Spark Fun - Voltage Divider](https://learn.sparkfun.com/tutorials/voltage-dividers)
    -   [Photocells](https://learn.adafruit.com/photocells/using-a-photocell)
    -   Analog Pin is placed at the voltage split at the center where two resistor is placed
-   [Photoresistor Module](https://arduinomodules.info/ky-018-photoresistor-module/)
-   [Capacitive Soil Moisture Sensor issue](https://forum.arduino.cc/t/capacitive-soil-moisture-sensor-v1-2-inconsistent/882137)
-   [How to fix faulty Capacitive soil moisture humidity sensor v1.2](https://www.youtube.com/watch?v=QGCrtXf8YSs)
-   [Capacitive Soil Moisture Sensor Quick Fix](https://www.youtube.com/watch?v=HB2Eky6czWM)
-   [Bypass LED Growlight Controllers](https://www.reddit.com/r/PlantedTank/comments/15j7jpp/how_to_bypass_hygger_planted_controller/)
-   [Bypass LED Growlight Controllers/Timer](https://www.reddit.com/r/AskElectronics/comments/nl426i/im_trying_to_bypass_my_little_growlight_timer/)
    -   [Bypass LED Growlight Controllers/Timer, Image](https://vgy.me/u/vJoD0X)

#### Web Servers + HTTP Protocol (LAN ONLY) and Other Bad IoT Workarounds

-   [ESP32 DHT11/DHT22 Asynchronous Web Server (LAN ONLY) & State Changes](https://www.youtube.com/watch?v=tDdL5urWvH4)
-   [ESP32 Async Web Server – Control Outputs with Arduino IDE (LAN ONLY)](https://randomnerdtutorials.com/esp32-async-web-server-espasyncwebserver-library/)
-   [ESP32 Adding Basic HTTP Authentication to your Web Server (LAN ONLY)](https://www.youtube.com/watch?v=1p6C-PNl0L0)
-   [Command Your Arduino from Anywhere, Part 1 – Port Forwarding (BAD! BIG SECURITY RISK)](https://kunzleigh.com/command-your-arduino-from-anywhere-part-1-port-forwarding/)
-   [Command Your Arduino from Anywhere, Part 2 – Ngrok + Middleman Server](https://kunzleigh.com/command-your-arduino-from-anywhere-part-2-ngrok-middleman-server/)

#### HTTP Protocol and Communication (IoT)

-   [ESP8266 + Arduino + database - Control Anything from Anywhere (IoT)](https://www.youtube.com/watch?v=6hpIjx8d15s)
-   [Control ESP32 and ESP8266 GPIOs from Anywhere in the World (IoT)](https://randomnerdtutorials.com/control-esp32-esp8266-gpios-from-anywhere/)
-   [Adafruit IO HTTP API](https://io.adafruit.com/api/docs/#adafruit-io-http-api)

#### MQTT Protocol and Communication (IoT)

-   [How to enable MQTT Authentication and Authorization on Mosquitto](https://cedalo.com/blog/mqtt-authentication-and-authorization-on-mosquitto/)
-   [ARDUINO IDE + ESP32 + Adafruit IO | Monitoring and Controlling the ESP32 with Adafruit IO](https://www.youtube.com/watch?v=H1ATqf4gBAU)
-   [Adafruit IO MQTT API](https://io.adafruit.com/api/docs/mqtt.html#adafruit-io-mqtt-api)
-   [Adafruit IO API Cookbook](https://io.adafruit.com/api/docs/cookbook.html#adafruit-io-api-cookbook)
-   [GitHub - Adafruit MQTT Library](https://github.com/adafruit/Adafruit_MQTT_Library/tree/master)
-   [Adafruit Tutorial - MQTT, Adafruit IO & You!](https://cdn-learn.adafruit.com/downloads/pdf/mqtt-adafruit-io-and-you.pdf)

### Arduino Mac Address (IoT)

### WiFi SSL

Note: This is needed because ESP32 and Arduino microcontrollers uses different libraries. Adafruit IO code is based around ESP32

-   [Arduino UNO R4 Wifi SSL will not connect to api.netatmo and others](https://forum.arduino.cc/t/arduino-uno-r4-wifi-ssl-will-not-connect-to-api-netatmo-and-others/1254103)

### Questions

-   [Arduino Forum - Question about Pin Numbers](https://forum.arduino.cc/t/how-to-identify-pins/862437)
-   [Arduino Forum - WebServer Library vs ESPAsyncWebServer Library Comparison](https://forum.arduino.cc/t/webserver-vs-espasyncwebserver/928293)
-   [Arduino Forum - Setting up Arduino Webserver for remote access](https://www.youtube.com/watch?v=1p6C-PNl0L0)
-   [Converting char[] to float or double in C++](https://stackoverflow.com/questions/50300851/converting-char-to-float-or-double-c)
-   [Passing a function as a parameter in C++](https://www.geeksforgeeks.org/cpp/passing-a-function-as-a-parameter-in-cpp/)
-   [Function Pointer in C++](https://www.geeksforgeeks.org/cpp/function-pointer-in-cpp/)
-   [Run C++ and C in Visual Studio Code](https://code.visualstudio.com/docs/languages/cpp)
-   [C++ Abstract Class / Interface](https://www.tutorialspoint.com/cplusplus/cpp_interfaces.htm)
-   [How do you add a timed delay to a C++ program?](https://stackoverflow.com/questions/158585/how-do-you-add-a-timed-delay-to-a-c-program)
-   [C++ chrono system time in milliseconds, time operations](https://stackoverflow.com/questions/9089842/c-chrono-system-time-in-milliseconds-time-operations)
-   [C++ Abstract Template Class](https://stackoverflow.com/questions/35905191/c-abstract-template-class)
-   [How to include just "parent.h" to use child class?](https://stackoverflow.com/questions/77830719/how-to-include-just-parent-h-to-use-child-class)
-   [C++ Abstract Class: constructor yes or no?](https://stackoverflow.com/questions/19808667/c-abstract-class-constructor-yes-or-no)
-   [Do I need to have a .cpp file for an abstract class?](https://stackoverflow.com/questions/14001356/do-i-need-to-have-a-cpp-file-for-an-abstract-class)
-   [Abstract class in .h or .cpp file or both?](https://stackoverflow.com/questions/52578416/abstract-class-in-h-or-cpp-file-or-both)
-   [How to declare an object of generic abstract class without specifying template type](https://stackoverflow.com/questions/56270381/how-to-declare-an-object-of-generic-abstract-class-without-specifying-template-t)
-   [When to use virtual destructors?](https://stackoverflow.com/questions/461203/when-to-use-virtual-destructors)
-   [How to create and use Static Template Functions in a Class?](https://stackoverflow.com/questions/9346076/static-template-functions-in-a-class)
-   [How to access members of objects using pointers](https://stackoverflow.com/questions/32476185/c-accessing-members-of-objects-using-pointers)

### Misc

-   [Convert SSL Certificate to Arduino ESP Variable](https://unreeeal.github.io/ssl_esp.html)

### Future Project Inspos

#### ESP-Now Protocol and Communication

-   [ESP-NOW Protocol](https://www.espressif.com/en/solutions/low-power-solutions/esp-now)
-   [Reddit - Espnow on arduino ](https://www.reddit.com/r/arduino/comments/17vbt3o/espnow_on_arduino/)

#### LoRa Protocol and Communication

-   [How Does LoRa Sensor Send and Receive data](https://www.mokosmart.com/how-does-lora-sensor-send-and-receive-data/)

#### Raspberry Pi

-   [Capture plant health with NDVI and Raspberry Pi](https://projects.raspberrypi.org/en/projects/astropi-ndvi)
-   [Raspberry Pi Forum - Take picture every day and upload/send it somewhere](https://forums.raspberrypi.com/viewtopic.php?t=369687)
-   [Flask, API, GET/POST, JSON](https://forums.raspberrypi.com/viewtopic.php?t=337112)

<!-- BACKEND SECTION -->

## Backend

### .NET

-   [Microsoft - Tutorial - First Web API + Entity Framework](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio)
-   [Microsoft - Entity Framework Core - Document](https://learn.microsoft.com/en-us/ef/core/)
-   [Microsoft - Entity Framework Core - Key](https://learn.microsoft.com/en-us/ef/core/modeling/keys?tabs=data-annotations)
-   [Microsoft - Entity Framework Core - Generated Value](https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations)
-   [Stackoverflow - Auto Increment on Partial Primary Key](https://stackoverflow.com/questions/36155429/auto-increment-on-partial-primary-key-with-entity-framework-core)
-   [Reddit - Entity Framework Core - Using Sequence to Genereate ID and Return on Add](https://www.reddit.com/r/dotnet/comments/wx90a6/ef_core_using_sequences_to_generate_ids_and/)

### Databases

-   [Youtube - Connect GCP MySQL VM Instance with MySQL Workbench](https://www.youtube.com/watch?v=uCPpPhdI6zA)
-   [Youtube - How to connect MySQL Database with Flask Tutorial](https://www.youtube.com/watch?v=14HTiBQEQ9M)
-   [Google - Cloud Firestore Documentation](https://firebase.google.com/docs/firestore)
-   [Google - Firestore Sample](https://cloud.google.com/firestore/docs/samples/firestore-data-set-id-specified)
-   [Create a database](https://learn.microsoft.com/en-us/azure/azure-sql/database/free-offer?view=azuresql)

<!-- FRONTEND SECTION -->

## Frontend

-   [Github - Ashan - Building Chat App with Angular Signal](https://github.com/AhsanAyaz/ng-signals-v17)
-   [Youtube - Ashan - Building Chat App with Angular Signal](https://www.youtube.com/watch?v=0f0Y7FgT6o8)
-   [FreeCodeCamp - How to Build a Realtime Chat Application with Angular 20 and Supabase](https://www.freecodecamp.org/news/how-to-build-a-realtime-chat-app-with-angular-20-and-supabase/)
-   [Mastering Frontend Observability in React with Grafana Faro](https://www.youtube.com/watch?v=IA_-zkpVhIU)
-   [Youtube - Fireship - Angular Chatbot with Dialogflow (API.ai)](https://www.youtube.com/watch?v=CKhV7-NF2OI)
-   [Angular Signal](https://angular.dev/guide/signals)
-   [Google Codelab - Building a web application with Angular and Firebase](https://developers.google.com/codelabs/building-a-web-app-with-angular-and-firebase#0)
-   [TailwindCSS - Install Tailwind CSS with Angular](https://tailwindcss.com/docs/installation/framework-guides/angular)

<!-- NETWORKING SECTION -->

## General Networking

-   [WAN for Dummies](https://www.aaronengineered.com/blog/wan-for-dummies)
-   [How to Access "localhost" from the Internet](https://www.youtube.com/watch?v=bxEmW1gAyRw)
-   [Accessing localhost from Anywhere](https://www.sitepoint.com/accessing-localhost-from-anywhere/)
-   [Using Caddy as a reverse proxy in a home network](https://caddy.community/t/using-caddy-as-a-reverse-proxy-in-a-home-network/9427)
-   [How To Setup An Caddy Server - The Ultimate Server](https://www.youtube.com/watch?v=G8Tsi9hQJxw)
-   [Reverse Proxy And Auto SSL Using Caddy And Docker Compose](https://www.youtube.com/watch?v=qj45uHP7Jmo)
-   [Self Host 101 - Run Multiple Apps with Caddy | DNS, Static Sites, Reverse Proxies and Let's Encrypt](https://www.youtube.com/watch?v=mLznVlBAtcg)

### Random

-   [University of Califorina - Soil Mixes Part 3: How much air and water?](https://ucanr.edu/blog/nursery-and-flower-grower/article/soil-mixes-part-3-how-much-air-and-water)
-   [Soil Moisture and Irrigation](https://soilsensor.com/soil/soil-moisture-and-irrigation/)
-   [University of Florida - Light for Houseplants](https://gardeningsolutions.ifas.ufl.edu/plants/houseplants/light-for-houseplants/)

### Security

-   [Youtube - Angular Firebase Authentication - Implement Auth In Minutes](https://www.youtube.com/watch?v=586O934xrhQ)
-   [Dev.IO - Firebase Authentication](https://dev.to/this-is-angular/firebase-authentication-with-angular-19-ief)
-   [Google - Cloud Firestore Documentation](https://firebase.google.com/docs/firestore)
-   [Youtube - Setting Up Google Authentication in Firebase 9: A Step-by-Step Guide](https://www.youtube.com/watch?v=-YA5kORugeI)

# TODO: Other Resources that need to be sorted out

-   [Microsoft - Tutorial: Create a controller-based web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio)
-   [Youtube - Swagger and .NET 9 - What is going on and How to get it back?](https://www.youtube.com/watch?v=3DHK8qWIFc8)
-   [Generate OpenAPI documents](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/aspnetcore-openapi?view=aspnetcore-9.0&tabs=visual-studio%2Cvisual-studio-code)
-   [Enable Cross-Origin Requests (CORS) in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0)
-   [Microsoft - Enable Cross-Origin Requests (CORS) in ASP.NET Core - 1](https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0#set-the-allowed-origins)
-   [Microsoft - Enable cross-origin requests in ASP.NET Web API 2](https://learn.microsoft.com/en-us/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api)
-   [Stackoverflow - How to Enable CORS in ASP.NET Core Web API](https://stackoverflow.com/questions/44379560/how-to-enable-cors-in-asp-net-core-webapi)
-   [C-Sharp Corner - Using CQRS Pattern in C-Sharp](https://www.c-sharpcorner.com/article/using-the-cqrs-pattern-in-c-sharp/)
-   [Milan Jovanovic - CQRS Pattern With MediatR](https://www.milanjovanovic.tech/blog/cqrs-pattern-with-mediatr)
-   [Code Maze - CQRS and MediatR in ASP.NET Core](https://code-maze.com/cqrs-mediatr-in-aspnet-core/)
-   [CQRS Validation Pipeline with MediatR and FluentValidation](https://code-maze.com/cqrs-mediatr-fluentvalidation/)
-   [Code Maze - Dependency Injection in ASP.NET Core](https://code-maze.com/dependency-injection-aspnet/)
-   [Code Maze - ASP.NET Core Series](https://code-maze.com/net-core-series/)
-   [Code Maze - ASP.NET Core Web API – Part 1 of 16 - Creating MySQL Database](https://code-maze.com/net-core-web-development-part1/)
-   [Code Maze - ASP.NET Core Web API – Part 2 of 16 - .NET Service Configuration](https://code-maze.com/net-core-web-development-part2/)
-   [Code Maze - ASP.NET Core Web API – Part 3 of 16 - Logging With NLog](https://code-maze.com/net-core-web-development-part3/)
-   [Code Maze - ASP.NET Core Web API – Part 4 of 16 - Repository Pattern](https://code-maze.com/net-core-web-development-part4/)
-   [Code Maze - ASP.NET Core Web API – Part 5 of 16 - How to Handle Get Request](https://code-maze.com/net-core-web-development-part5/)
-   [Code Maze - ASP.NET Core Web API – Part 5 of 16 - Post, Put, Delete](https://code-maze.com/net-core-web-development-part6/)
-   [Code Maze - ASP.NET Core Web API – Part 16 of 16 - IIS Deployment](https://code-maze.com/net-core-web-development-part16/)
-   [Deploy the ASP.NET Core Web App to Linux](https://code-maze.com/deploy-aspnetcore-linux-nginx/)
-   [Github Forums - CORS does not function on swagger.json endpoint](https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2641)
-   [Medium - Advanced Design Patterns in C#: Command Query Responsibility Segregation (CQRS)](https://krishan-samarawickrama.medium.com/advanced-design-patterns-in-c-command-query-responsibility-segregation-cqrs-d9ff598ccdf6)
-   [Microsoft - CQRS Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
-   [Dev.to - Mastering CQRS Design Pattern with MediaTr in C .NET](https://dev.to/dianaiminza/mastering-cqrs-design-pattern-with-mediatr-in-c-net-khk)
-   [OAuth Google Sign-In Setup - Youtube](https://www.youtube.com/watch?v=TjMhPr59qn4)
-   [OAuth Google Sign-In Setup - GitHub](https://github.com/divanov11/apple-signin/tree/google-signin)
-   [Microsoft - Tutorial: Host a RESTful API with CORS in Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-rest-api)
-   [Google - Quickstart: Build and deploy a .NET web app to Cloud Run](https://cloud.google.com/run/docs/quickstarts/build-and-deploy/deploy-dotnet-service)
-   [Google Authentication in ASP.NET Core with React (.NET 9) ](https://www.youtube.com/watch?v=SH-ABnaKSIM)
-   [AspNetCore WebAPI - Google Authenticatio](https://dvoituron.com/2019/08/09/webapi-google-auth/)
-   [External OAuth authentication providers](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-9.0)
-   [Entity Framework Tutorial](https://www.entityframeworktutorial.net/)
-   [Microsoft Tutorial - Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
-   [Tutorials Point - Entity Framework](https://www.tutorialspoint.com/entity_framework/index.htm)
-   [Medium - Step-by-Step Guide to Entity Framework in .NET](https://medium.com/@lucas.and227/step-by-step-guide-to-entity-framework-in-net-c629faf9f322)
-   [StackOverFlow - EntityFramework Core auto generate key id property](https://stackoverflow.com/questions/43899190/entityframework-core-auto-generate-key-id-property)
-   [Running A Worker Service As A Windows Service](https://medium.com/@adebanjoemmanuel01/running-a-worker-service-as-a-windows-service-c1d12a28a73c)
-   [Create a .NET Web API with Entity Framework](https://www.youtube.com/watch?v=A7nV0xF5vYg)

-   [Can someone explain when to use Singleton, Scoped and Transient with some real life examples?](https://www.reddit.com/r/csharp/comments/1acwtar/can_someone_explain_when_to_use_singleton_scoped/)

## Others:

-   [Flask OpenAPI Generation? ](https://www.reddit.com/r/flask/comments/1gkcdnu/flask_openapi_generation/)
-   [Fasi API Doc](https://pypi.org/project/fastapi/)
-   [Fast API Doc](https://fastapi.tiangolo.com/)
-   [Fast API - Generate OpenAPI Spec](https://www.speakeasy.com/openapi/frameworks/fastapi)

## Cloud Applications - Google Cloud Platform

-   [Youtube - GCP - How to SSH to Virtual Machine (VM) Linux on Google Cloud Platform](https://www.youtube.com/watch?v=dZ1LhEgoC_8)
-   [Youtube - How to connect to Google Cloud Virtual Machine using SSH from Terminal](https://www.youtube.com/watch?v=hP9B3xXP1Ts)
-   [Google Cloud - Google Cloud Run - Quickstart: Build and deploy a .NET web app to Cloud Run](https://cloud.google.com/run/docs/quickstarts/build-and-deploy/deploy-dotnet-service)

## Database

## Database + Entity Framework

-   [Entity Framework and SQLite, the ultimate how-to](https://stackoverflow.com/questions/63494481/entity-framework-and-sqlite-the-ultimate-how-to)

# APIs

-   [Open Weather API - Weather](https://openweathermap.org/api/one-call-3#concept)
-   [Open Weather API - GeoCoding](https://openweathermap.org/api/geocoding-api#direct_zip)

# TODO: OTHER RESOURCES THAT I NEED TO SORT OUT TOO

<!-- BACKEND SECTION: MICROSOFT SEMANTIC KERNEL -->

-   [Microsoft Website - Semantic Kernel - Documentation - Introduction to Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/overview/)

    -   [Microsoft Website - Semantic Kernel - Documentation - Understanding the kernel](https://learn.microsoft.com/en-us/semantic-kernel/overview/)
    -   [Microsoft Website - Semantic Kernel - Documentation - What is a Plugin?](https://learn.microsoft.com/en-us/semantic-kernel/concepts/plugins/?pivots=programming-language-csharp)
    -   [Microsoft Website - Semantic Kernel - Documentation - Using plugins for Retrieval Augmented Generation (RAG)](https://learn.microsoft.com/en-us/semantic-kernel/concepts/plugins/using-data-retrieval-functions-for-rag?source=recommendations)

-   [Let's Learn .NET Semantic Kernel - From MS DOTNET - Video](https://www.youtube.com/watch?v=lCQOCoH3Osk)

    -   [Let's Learn .NET Semantic Kernel - Training Module](https://learn.microsoft.com/en-us/collections/4wrote7dxq3mxx?WT.mc_id=dotnet-147962-juyoo)
    -   [Let's Learn .NET Semantic Kernel - Github.IO Instruction (HIGHLY RECOMMEND!)](https://microsoftlearning.github.io/mslearn-ai-semantic-kernel/)
    -   [Let's Learn .NET Semantic Kernel - Github Template](https://github.com/MicrosoftLearning/mslearn-ai-semantic-kernel)

-   [Develop AI Agents with Azure OpenAI and Semantic Kernel SDK - Github Template](https://github.com/MicrosoftLearning/MSLearn-Develop-AI-Agents-with-Azure-OpenAI-and-Semantic-Kernel-SDK)

-   [Semantic Kernel - Microsoft - Github](https://github.com/microsoft/semantic-kernel)

    -   [Semantic Kernel - Samples - Microsoft - Github](https://github.com/microsoft/semantic-kernel/tree/main/dotnet/samples)
    -   [Semantic Kernel - Samples - Agents - Microsoft - Github](https://github.com/microsoft/semantic-kernel/tree/main/dotnet/samples/GettingStartedWithAgents)

-   [Develop an AI Agent using Semantic Kernel AI-3026 - Youtube (Python)](https://www.youtube.com/watch?v=NFs4zXIhHKU)
-   [Orchestrate a multi-agent solution using Semantic Kernel - Microsoft (Python) - Great Read though](https://learn.microsoft.com/en-us/training/modules/orchestrate-semantic-kernel-multi-agent-solution/)

-   [Microsoft Website - Semantic Kernel - Documentation - Agents Framework](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/?pivots=programming-language-csharp)

    -   [Microsoft Website - Semantic Kernel - Documentation - Agent Orchestration Types + Sample Code](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-orchestration/?pivots=programming-language-csharp)

    -   [Microsoft Website - Semantic Kernel - Documentation - Agents Examples Sample Code](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/examples/example-chat-agent?pivots=programming-language-csharp)

    -   [Microsoft Website - Semantic Kernel - Documentation - Agents Memory](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-memory?pivots=programming-language-csharp)

    -   [Microsoft Website - Semantic Kernel - Documentation - Agents RAG Search + Sample Code](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-rag?pivots=programming-language-csharp)

-   [Microsoft Website - Semantic Kernel - Documentation - Proccess Framework](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/process/process-framework)

    -   [Microsoft Website - Semantic Kernel - Documentation - Process Sample Code](https://learn.microsoft.com/en-us/semantic-kernel/frameworks/process/examples/example-first-process?pivots=programming-language-csharp)

-   [Building AI Agent Workflows with Semantic Kernel - Microsoft Developer - Youtube (C# and .NET)](https://www.youtube.com/watch?v=3JFKwerYj04)

    -   [Sample of Complete Semantic Kernel Project (Semantic Clip) - Microsoft Developer - Github (C# and .NET)](https://github.com/vicperdana/SemantiClip)

-   [User MS Semantic Kernel Samples - Vinoth Rajendran - GitHub (C# and .NET)](https://github.com/rvinothrajendran/MicrosoftSemanticKernelSamples)

    -   [Sequential multi-agent orcestration - Process + Process Steps and States with Semantic Kernel - Vinoth Rajendran - Github](https://github.com/rvinothrajendran/MicrosoftSemanticKernelSamples/tree/main/SKSampleCSharp/ProcessSample)
    -   [](https://www.youtube.com/watch?v=xha6M8wK_AY)

<!-- BACKEND SECTION: MICROSOFT KERNEL MEMORY -->

# RESOURCES - NEED TO BE SORTED OUT

-   [Github - Microsoft - Kernel Memory](https://github.com/microsoft/kernel-memory)
-   [Youtube - Matt Eland's Document Search in .NET with Kernel Memory](https://www.youtube.com/watch?v=h8bKn1nzjrQ)
-   [Github - Matt Eland's Document Search with Kernel Memory Code](https://github.com/IntegerMan/DocumentSearchWithKernelMemory/blob/main/MattEland.Demos.KernelMemory.DocumentSearch/MattEland.Demos.KernelMemory.DocumentSearch/Program.cs)
-   [LeadingEDJE's Kernel Memory Search Blog Post](https://blog.leadingedje.com/post/ai/documents/kernelmemory.html)
-   [Johnny Reilly's Blog on Chunking Documents into Azure AI Search](https://johnnyreilly.com/using-kernel-memory-to-chunk-documents-into-azure-ai-search)
-   [StormHub's dev.to Post on Kernel Memory with Azure Services](https://dev.to/stormhub/kernel-memory-with-azure-openai-blob-storage-and-ai-search-services-1245)
-   [StormHub's GitHub Code for Azure Kernel Memory Integration](https://github.com/StormHub/stormhub/blob/main/resources/2024-11-18/ConsoleApp/ConsoleApp/Program.cs)
-   [Semantic Kernel with Azure Search - RAG + Kernel Memory](https://devblogs.microsoft.com/semantic-kernel/azure-openai-on-your-data-with-semantic-kernel/)

# GITIGNORE COLLECTION

-   [Github Default GitIgnore Collection](https://github.com/github/gitignore/tree/main)
    -   [Github Default GitIgnore - Visual Studio](https://github.com/github/gitignore/blob/main/VisualStudio.gitignore)
    -   [Github Default GitIgnore - Angular](https://github.com/github/gitignore/blob/main/Angular.gitignore)
